using CivilisApp.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace CivilisApp.ApiTests;

[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class ApiDatabaseCollection : ICollectionFixture<PostgreSqlApiFixture>
{
    public const string Name = "PostgreSQL API tests";
}

public sealed class PostgreSqlApiFixture : IAsyncLifetime
{
    private const string JwtSecret = "ThisismyUltrasuperSecretKey@345!";
    private const string WorkingDirectoryEnvironmentVariable = "CIVILIS_TEST_WORKING_DIRECTORY";
    private const string AdminConnectionEnvironmentVariable = "CIVILIS_TEST_DB_ADMIN_CONNECTION";
    private const string ApplicationConnectionEnvironmentVariable = "ConnectionStrings__DBConnection";
    private const string MigrationEnvironmentVariable = "Testing__RunMigrations";

    private string? _previousApplicationConnection;
    private string? _previousMigrationSetting;
    private string? _previousWorkingDirectory;
    private string? _originalWorkingDirectory;
    private string? _adminConnectionString;
    private string? _testConnectionString;
    private bool _databaseCreated;

    public WebApplicationFactory<Program> Factory { get; private set; } = null!;
    public HttpClient Client { get; private set; } = null!;
    public string DatabaseName { get; private set; } = string.Empty;
    public string WorkingDirectory { get; private set; } = string.Empty;

    public async Task InitializeAsync()
    {
        var configuredAdminConnection = Environment.GetEnvironmentVariable(AdminConnectionEnvironmentVariable);
        if (string.IsNullOrWhiteSpace(configuredAdminConnection))
        {
            throw new InvalidOperationException(
                $"Set {AdminConnectionEnvironmentVariable} to a PostgreSQL superuser connection string for API integration tests.");
        }

        var adminBuilder = new NpgsqlConnectionStringBuilder(configuredAdminConnection)
        {
            Database = "postgres",
            Pooling = false
        };
        _adminConnectionString = adminBuilder.ConnectionString;
        DatabaseName = $"civilis_api_tests_{Guid.NewGuid():N}";
        var testBuilder = new NpgsqlConnectionStringBuilder(adminBuilder.ConnectionString)
        {
            Database = DatabaseName,
            Pooling = true
        };
        _testConnectionString = testBuilder.ConnectionString;

        _previousApplicationConnection = Environment.GetEnvironmentVariable(ApplicationConnectionEnvironmentVariable);
        _previousMigrationSetting = Environment.GetEnvironmentVariable(MigrationEnvironmentVariable);
        _previousWorkingDirectory = Environment.GetEnvironmentVariable(WorkingDirectoryEnvironmentVariable);
        _originalWorkingDirectory = Directory.GetCurrentDirectory();

        try
        {
            await using (var connection = new NpgsqlConnection(_adminConnectionString))
            {
                await connection.OpenAsync();
                await using var createDatabase = new NpgsqlCommand($"CREATE DATABASE \"{DatabaseName}\"", connection);
                await createDatabase.ExecuteNonQueryAsync();
                _databaseCreated = true;
            }

            Environment.SetEnvironmentVariable(ApplicationConnectionEnvironmentVariable, _testConnectionString);
            Environment.SetEnvironmentVariable(MigrationEnvironmentVariable, "true");
            WorkingDirectory = Path.Combine(Path.GetTempPath(), $"civilis-api-tests-{Guid.NewGuid():N}");
            Directory.CreateDirectory(Path.Combine(WorkingDirectory, "Templates"));
            Directory.CreateDirectory(Path.Combine(WorkingDirectory, "Reports"));
            CopyTemplate("PersonsTemplate.xlsx");
            CopyTemplate("EntitiesTemplate.xlsx");
            Environment.SetEnvironmentVariable(WorkingDirectoryEnvironmentVariable, WorkingDirectory);
            Directory.SetCurrentDirectory(WorkingDirectory);

            Factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");
                builder.ConfigureTestServices(services =>
                {
                    services.RemoveAll<IBufferedFileUploadService>();
                    services.AddSingleton<IBufferedFileUploadService, TestBufferedFileUploadService>();
                });
            });
            Client = CreateAuthenticatedClient(1, "ADMIN");
            await ReseedIdentitySequencesAsync();
        }
        catch
        {
            await DropDatabaseAsync();
            RestoreEnvironment();
            if (Directory.Exists(WorkingDirectory))
            {
                Directory.Delete(WorkingDirectory, recursive: true);
            }
            throw;
        }
    }

    public async Task DisposeAsync()
    {
        Client?.Dispose();
        Factory?.Dispose();
        NpgsqlConnection.ClearAllPools();
        await DropDatabaseAsync();
        RestoreEnvironment();
        if (Directory.Exists(WorkingDirectory))
        {
            Directory.Delete(WorkingDirectory, recursive: true);
        }
    }

    private void CopyTemplate(string templateName)
    {
        var template = Path.Combine(_originalWorkingDirectory!, "Templates", templateName);
        if (!File.Exists(template))
        {
            throw new FileNotFoundException($"Required report template '{templateName}' was not found.", template);
        }

        File.Copy(template, Path.Combine(WorkingDirectory, "Templates", templateName));
    }

    public HttpClient CreateAnonymousClient() => Factory.CreateClient();

    public HttpClient CreateAuthenticatedClient(long userId = 1, params string[] roles)
    {
        var client = Factory.CreateClient();
        var claims = new List<Claim> { new(ClaimTypes.Name, userId.ToString()) };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSecret)),
            SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: "http://localhost:4200",
            audience: "http://localhost:4200",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: credentials);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            new JwtSecurityTokenHandler().WriteToken(token));
        return client;
    }

    public async Task<NpgsqlConnection> OpenDatabaseConnectionAsync()
    {
        var connection = new NpgsqlConnection(_testConnectionString);
        await connection.OpenAsync();
        return connection;
    }

    private async Task ReseedIdentitySequencesAsync()
    {
        await using var connection = await OpenDatabaseConnectionAsync();
        const string sql = """
            DO $$
            DECLARE identity_column RECORD;
            BEGIN
                FOR identity_column IN
                    SELECT table_schema, table_name, column_name
                    FROM information_schema.columns
                    WHERE table_schema = 'public' AND is_identity = 'YES'
                LOOP
                    EXECUTE format(
                        'SELECT setval(pg_get_serial_sequence(%L, %L), COALESCE(MAX(%I), 0) + 1, false) FROM %I.%I',
                        identity_column.table_schema || '.' || identity_column.table_name,
                        identity_column.column_name,
                        identity_column.column_name,
                        identity_column.table_schema,
                        identity_column.table_name);
                END LOOP;
            END
            $$;
            """;
        await using var command = new NpgsqlCommand(sql, connection);
        await command.ExecuteNonQueryAsync();
    }

    private async Task DropDatabaseAsync()
    {
        if (!_databaseCreated || string.IsNullOrWhiteSpace(_adminConnectionString))
        {
            return;
        }

        await using var connection = new NpgsqlConnection(_adminConnectionString);
        await connection.OpenAsync();
        await using (var terminateConnections = new NpgsqlCommand(
            "SELECT pg_terminate_backend(pid) FROM pg_stat_activity WHERE datname = @databaseName AND pid <> pg_backend_pid()",
            connection))
        {
            terminateConnections.Parameters.AddWithValue("databaseName", DatabaseName);
            await terminateConnections.ExecuteNonQueryAsync();
        }

        await using var dropDatabase = new NpgsqlCommand($"DROP DATABASE IF EXISTS \"{DatabaseName}\"", connection);
        await dropDatabase.ExecuteNonQueryAsync();
        _databaseCreated = false;
    }

    private void RestoreEnvironment()
    {
        Environment.SetEnvironmentVariable(ApplicationConnectionEnvironmentVariable, _previousApplicationConnection);
        Environment.SetEnvironmentVariable(MigrationEnvironmentVariable, _previousMigrationSetting);
        Environment.SetEnvironmentVariable(WorkingDirectoryEnvironmentVariable, _previousWorkingDirectory);
        if (!string.IsNullOrWhiteSpace(_originalWorkingDirectory) && Directory.Exists(_originalWorkingDirectory))
        {
            Directory.SetCurrentDirectory(_originalWorkingDirectory);
        }
    }
}
