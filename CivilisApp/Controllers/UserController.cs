using Azure.Core;
using CivilisApp.Models;
using CivilisDB;
using CivilisDB.Models;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;


namespace CivilisApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<UserController> _logger;
        private DbConnectionStringResolver _dbConnectionStringResolver;
        private string connStr;
        private const int AccessTokenDurationInMinutes = 1;
        private const int AccessTokenDurationInMinutesRememberMe = 1;
        public UserController(IConfiguration config, ILogger<UserController> logger)
        {
            this.configuration = config;
            _logger = logger;
            _logger.LogTrace(1, "Serilog injected into User Controller");
            _dbConnectionStringResolver = new DbConnectionStringResolver(configuration);
            this.connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
        }

        // GET: api/<UserController>
        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public ActionResult<List<tblUser>> Get()
        {
            this.connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            List<tblUser> userList;
            using (tblUserDac dac = new tblUserDac(this.connStr, _logger))
            {
                userList = dac.GetAll();
                if (userList == null)
                {
                    return NotFound();
                }
                return Ok(userList);
            }
        }

        // GET: api/<UserController>/5
        [HttpGet("{id}"), Authorize]
        public ActionResult<tblUser> Get(long id)
        {
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            tblUser obj;
            using (tblUserDac dac = new tblUserDac(connStr, _logger))
            {
                obj = dac.Get(id);
                if (obj != null)
                    return Ok(obj);
                else
                    return NotFound();
            }
        }


        // GET: api/<UserController>/login/loginname
        [HttpGet("login/{login}"), Authorize]
        public ActionResult<tblUser> GetByLogin(string login)
        {
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            tblUser obj;
            using (tblUserDac dac = new tblUserDac(connStr, _logger))
            {
                obj = dac.GetByLogin(login);
                if (obj != null)
                    return Ok(obj);
                else
                    return NotFound();
            }
        }

        // POST api/<UserController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }


        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserAuth userDto)
        {
            //this.connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            tblUser user;
            bool resInsert;
            bool resUpdate;
            string accessToken;
            using (tblUserDac dac = new tblUserDac(connStr, _logger))
            {
                user = dac.Authenticate(userDto.login, userDto.password);
            }

            if (user == null)
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "Username or password is incorrect" });

            if (userDto.rememberMe)
            {
                // Implement remember me functionality if needed
                accessToken = GenerateToken(user, AccessTokenDurationInMinutesRememberMe); // 7 days

            } else
            {
                // Implement non-remember me functionality if needed
                accessToken = GenerateToken(user, AccessTokenDurationInMinutes); // 4 hours
            }

            
            tblRefreshToken refreshtoken = new tblRefreshToken(user.flduserid);
            if (userDto.rememberMe)
            { 
                refreshtoken.fldexpires = DateTime.Now.AddMinutes(60*24*7); 
            }
             else
            {
                refreshtoken.fldexpires = DateTime.Now.AddMinutes(60 * 4);
            }
            using (tblRefreshTokenDac dac2 = new tblRefreshTokenDac(connStr, _logger))
            {
                var res1 = dac2.Get(refreshtoken.flduserid);

                if (res1 == null)
                    resInsert = dac2.Insert(refreshtoken);
                else
                    resUpdate = dac2.Update(refreshtoken);
            }
            //For every login cleart the log directory
            Utils.FileOperations.DeleteFileOlderThanXDays(Path.Combine(Directory.GetCurrentDirectory(),"logs"), 10);

            //SetTokensInsideCookie(tokenString, refreshtoken.fldrefreshtoken, HttpContext);
            return Ok(new
            {
                userid = user.flduserid,
                login = user.fldlogin,
                accesstoken = accessToken,
                refreshtoken = refreshtoken.fldrefreshtoken,
                personid = user.fldpersonid
            }); 
        }


        [Authorize]
        [HttpPost("changepassword")]
        public IActionResult ChangePassword([FromBody] ChangePass changePass)
        {
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            bool res;
            using (tblUserDac dac = new tblUserDac(connStr, _logger))
            {
                res = dac.ChangePassword(changePass.login, changePass.oldpassword, changePass.newpassword);
            }
            if (res)
            {
                return Ok();
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = "Password Change Failed" });
            }

        }


        [Authorize(Roles = "ADMIN")]
        [HttpPost("changepasswordbyadmin")]
        public IActionResult AdminChangePassword([FromBody] ChangePassByAdmin changePassByAdmin)
        {
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            bool res;
            using (tblUserDac dac = new tblUserDac(connStr, _logger))
            {
                res = dac.ChangePasswordByAdmin(changePassByAdmin.flduserid, changePassByAdmin.newpassword);
            }
            if (res)
            {
                return Ok();
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = "Password Change Failed" });
            }

        }


        [AllowAnonymous]
        [HttpPost("reenter")]
        public IActionResult ReEnter([FromBody] RefreshApiModel refreshApi)
        {
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            tblUser user;
            tblRefreshToken dbrefreshToken;
            using (tblRefreshTokenDac dac2 = new tblRefreshTokenDac(connStr, _logger))
            {
                dbrefreshToken = dac2.GetbyRefreshToken(refreshApi.refreshtoken);
            }
            if (dbrefreshToken is null)
                return BadRequest("Invalid client request");
            else if (dbrefreshToken.fldexpires <= DateTime.Now)
                return BadRequest("Refresh token has expired");
            else
            {
                using (tblUserDac dac = new tblUserDac(connStr, _logger))
                {
                    user = dac.Get(dbrefreshToken.flduserid);
                }

                var newAccessToken = GenerateToken(user, AccessTokenDurationInMinutes);
                return Ok(new
                {
                    userid = user.flduserid,
                    login = user.fldlogin,
                    accesstoken = newAccessToken,
                    refreshtoken = refreshApi.refreshtoken,
                    personid = user.fldpersonid
                });
            }

            //var principal = GetPrincipalFromExpiredToken(accessToken);
            //var username = principal.Identity.Name;

            //Retrieve user




            //if (user is null || dbrefreshToken.fldrefreshtoken != refreshToken || dbrefreshToken.fldexpires <= DateTime.Now)
            //    return BadRequest("Invalid client request");



            //var refreshtokenModel = new tblRefreshToken(user.flduserid);
            //using (tblRefreshTokenDac dac4 = new tblRefreshTokenDac(connStr, _logger))
            //{
            //    var res = dac4.Update(refreshtokenModel);

            //}

            //var newRefreshToken = refreshtokenModel.fldrefreshtoken;
            //var newAccessToken = GenerateToken(user,120);
            ////SetTokensInsideCookie(newAccessToken, newRefreshToken, HttpContext);
            //return Ok(new
            //{
            //    id = user.flduserid,
            //    login = user.fldlogin,
            //    token = newAccessToken,
            //    refreshtoken = newRefreshToken
            //});
        }


        [AllowAnonymous]
        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] RefreshApiModel refreshApi)
        {
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            tblUser user;
            tblRefreshToken dbrefreshToken;
            using (tblRefreshTokenDac dac2 = new tblRefreshTokenDac(connStr, _logger))
            {
                dbrefreshToken = dac2.GetbyRefreshToken(refreshApi.refreshtoken);
            }
            if (dbrefreshToken is null)
                return BadRequest("Invalid client request");
            else
            {
                using (tblUserDac dac = new tblUserDac(connStr, _logger))
                {
                    user = dac.Get(dbrefreshToken.flduserid);
                }
            }
            var newAccessToken = GenerateToken(user, AccessTokenDurationInMinutes);
            return Ok(new
            {
                userid = user.flduserid,
                login = user.fldlogin,
                accesstoken = newAccessToken,
                refreshtoken = refreshApi.refreshtoken,
                personid = user.fldpersonid
            });





            //var principal = GetPrincipalFromExpiredToken(accessToken);
            //var username = principal.Identity.Name;

            //Retrieve user




            //if (user is null || dbrefreshToken.fldrefreshtoken != refreshToken || dbrefreshToken.fldexpires <= DateTime.Now)
            //    return BadRequest("Invalid client request");



            //var refreshtokenModel = new tblRefreshToken(user.flduserid);
            //using (tblRefreshTokenDac dac4 = new tblRefreshTokenDac(connStr, _logger))
            //{
            //    var res = dac4.Update(refreshtokenModel);

            //}

            //var newRefreshToken = refreshtokenModel.fldrefreshtoken;
            //var newAccessToken = GenerateToken(user,120);
            ////SetTokensInsideCookie(newAccessToken, newRefreshToken, HttpContext);
            //return Ok(new
            //{
            //    id = user.flduserid,
            //    login = user.fldlogin,
            //    token = newAccessToken,
            //    refreshtoken = newRefreshToken
            //});
        }


        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            if (token.Length > 0)
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = "http://localhost:4200",
                    ValidAudience = "http://localhost:4200",
                    ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisismyUltrasuperSecretKey@345!")),
                    ValidateLifetime = true //here we are saying that we don't care about the token's expiration date
                };
                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    SecurityToken securityToken;
                    var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
                    var jwtSecurityToken = securityToken as JwtSecurityToken;
                    if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                        throw new SecurityTokenException("Invalid token");
                    return principal;
                }
                catch (Exception ex)
                {
                    throw new SecurityTokenException("Invalid token");
                }
                //var tokenHandler = new JwtSecurityTokenHandler();
                //SecurityToken securityToken;
                //var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
                //var jwtSecurityToken = securityToken as JwtSecurityToken;
                //if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                //    throw new SecurityTokenException("Invalid token");
                //return principal;
            }
            else
            {
                return null;
            }
        }

        private string GenerateToken(tblUser user, int durationInMinutes)
        {
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisismyUltrasuperSecretKey@345!"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            List<vwUserRole> resRoles;
            using (vwUserRoleDac dacUserRole = new vwUserRoleDac(connStr, _logger))
            {
                resRoles = dacUserRole.GetAllForUserId(user.flduserid);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.flduserid.ToString()),
                //new Claim(ClaimTypes.Role, "Admin")
            };
            foreach (var claim in resRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, claim.fldroledesc));
            }

            var tokeOptions = new JwtSecurityToken(
                issuer: "http://localhost:4200",
                audience: "http://localhost:4200",
                claims: claims,
                //expires: DateTime.Now.AddMinutes(durationInMinutes),
                expires: DateTime.UtcNow.AddMinutes(durationInMinutes),
                signingCredentials: signinCredentials 
                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;

        }

        //private void SetTokensInsideCookie(string accessToken, string refreshToken, HttpContext context)
        //{
        //    context.Response.Cookies.Append("accessToken", accessToken,
        //        new CookieOptions
        //        {
        //            Expires = DateTimeOffset.UtcNow.AddMinutes(15),
        //            HttpOnly = true,
        //            IsEssential = true,
        //            Secure = false,
        //            SameSite = SameSiteMode.None
        //        });
        //    context.Response.Cookies.Append("refreshToken", refreshToken,
        //        new CookieOptions
        //        {
        //            Expires = DateTimeOffset.UtcNow.AddDays(7),
        //            HttpOnly = true,
        //            IsEssential = true,
        //            Secure = false,
        //            SameSite = SameSiteMode.None
        //        });
        //}


    }

    
}
