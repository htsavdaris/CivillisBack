using Microsoft.AspNetCore.Mvc;
using CivilisDB;
using Serilog;
using Microsoft.AspNetCore.Authorization;
using CivilisDB.Models;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CivilisApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleViewController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<RoleController> _logger;
        private DbConnectionStringResolver _dbConnectionStringResolver;
        private string connStr;

        public UserRoleViewController(IConfiguration config, ILogger<RoleController> logger)
        {
            this.configuration = config;
            _logger = logger;
            _logger.LogTrace(1, "Serilog injected into UserRoleViewController Controller");
            _dbConnectionStringResolver = new DbConnectionStringResolver(configuration);
            this.connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
        }

        // GET: api/<UserRoleViewController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UserRoleViewController>/5
        [HttpGet("{id}"), Authorize]
        public ActionResult<vwUserRole> Get(int id)
        {
            _logger.LogTrace(1, "vwUserRole - Get is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            vwUserRole obj;
            using (vwUserRoleDac dac = new vwUserRoleDac(connStr, _logger))
            {
                try
                {
                    obj = dac.Get(id);
                    if (obj != null)
                        return Ok(obj);
                    else
                        return NotFound();
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    _logger.LogError(1, "UserRoleView Controller - NpgSQL Error:" + ex.Message);
                    return BadRequest(ex.Message);
                }

            }
        }

        [HttpGet("user/{id}"), Authorize]
        public ActionResult<List<vwUserRole>> GetAllForUserId(int id)
        {
            _logger.LogTrace(1, "vwUserRole - GetAllForUserId is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            List<vwUserRole> obj;
            using (vwUserRoleDac dac = new vwUserRoleDac(connStr, _logger))
            {
                try
                {
                    obj = dac.GetAllForUserId(id);
                    if (obj != null)
                        return Ok(obj);
                    else
                        return NotFound();
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    _logger.LogError(1, "UserRoleView Controller - NpgSQL Error:" + ex.Message);
                    return BadRequest(ex.Message);
                }

            }
        }

        // POST api/<UserRoleViewController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UserRoleViewController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserRoleViewController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
