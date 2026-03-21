using CivilisApp.Hubs;
using CivilisApp.Interfaces;
using CivilisApp.Services;
using CivilisApp.Utils;
using DbUp;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using static System.Net.WebRequestMethods;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DBConnection");
RunMigrationsforProd(connectionString);

if (!builder.Environment.IsDevelopment())
{

}
// Add services to the container.
var jwtOptions = builder.Configuration
    .GetSection("JwtOptions")
    .Get<JwtOptions>();

builder.Services.AddSingleton(jwtOptions);
builder.Services.AddSignalR();
builder.Services.AddTransient<IBufferedFileUploadService, BufferedFileUploadLocalService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var secret = System.Text.Encoding.UTF8.GetBytes("ThisismyUltrasuperSecretKey@345!");
// This is the old configuration section

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "http://localhost:4200",
        ValidAudience = "http://localhost:4200",
        IssuerSigningKey = new SymmetricSecurityKey(secret),
        ClockSkew = TimeSpan.Zero
    };
});

//builder.Services.AddAuthentication(c =>
//{
//    c.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; 
//    c.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; 
//})
////    .AddCookie(x =>
////{
////    x.Cookie.Name = "token";
////})
//    .AddJwtBearer(c => {
//    //c.Authority = "http://localhost:4200";
//    c.RequireHttpsMetadata = false;
//    c.SaveToken = true;
//    c.TokenValidationParameters = new TokenValidationParameters
//    {

//        ValidateIssuerSigningKey = true,
//        IssuerSigningKey = new SymmetricSecurityKey(secret),
//        ValidateIssuer = true,
//        ValidateAudience = false,
//        ValidateLifetime = true,
//        ValidIssuer = "http://localhost:4200",
//        ValidAudience = "http://localhost:4200",
//        ClockSkew = TimeSpan.Zero        
//    };
//    c.Events = new JwtBearerEvents
//    {
//        OnMessageReceived = context =>
//        {
//            context.Token = context.Request.Cookies["accessToken"];
//            //context.Request.Cookies.TryGetValue("accessToken", out var accessToken);
//            //if (!string.IsNullOrEmpty(accessToken))
//            //    context.Token = accessToken;
//            return Task.CompletedTask;

//        }
//    };
//});
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(options =>
//{
//    options.Authority = "https://civilis-test.eu.auth0.com/"; //builder.Configuration["JwtOptions:Authority"]; // "https://civilis-test.eu.auth0.com/";
//    options.Audience = "https://localhost:44330/"; //builder.Configuration["JwtOptions:Audience"]; // "https://localhost:44330/";

//});


builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.WithOrigins(new string[] { "https://localhost:53094", "https://localhost:4200", "http://localhost:4200",
        })
           .AllowAnyMethod()
           .AllowAnyHeader()
           .AllowCredentials();
}));

//builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
//{
//    builder.AllowAnyOrigin()
//           .AllowAnyMethod()
//              .AllowCredentials()
//           .AllowAnyHeader();
//}));

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
//builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
app.UseCors("MyPolicy"); // Before or after UseHttpsRedirection

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<CivilisSignalrHub>("/signalrhub");
app.Run();


static void RunMigrationsforProd(string? ConnectionString)
{
    EnsureDatabase.For.PostgresqlDatabase(ConnectionString);

    var upgrader = DeployChanges.To
        .PostgresqlDatabase(ConnectionString)
        .WithScriptsFromFileSystem("/Scripts")
        //.WithScriptsEmbeddedInAssembly(System.Reflection.Assembly.GetExecutingAssembly())
        .LogToConsole()
        .Build();

    var result = upgrader.PerformUpgrade();

    if (!result.Successful)
    {
        throw new Exception(result.Error.ToString());
    }

}

static void RunMigrationsforDev(string? ConnectionString)
{
    EnsureDatabase.For.PostgresqlDatabase(ConnectionString);

    var upgrader = DeployChanges.To
        .PostgresqlDatabase(ConnectionString)
        .WithScriptsEmbeddedInAssembly(System.Reflection.Assembly.GetExecutingAssembly())
        .LogToConsole()
        .Build();

    var result = upgrader.PerformUpgrade();

    if (!result.Successful)
    {
        throw new Exception(result.Error.ToString());
    }

}