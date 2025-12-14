using Microsoft.AspNetCore.Mvc;
using CivilisDB;
using Serilog;
using Microsoft.AspNetCore.Authorization;
using CivilisDB.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CivilisApp.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CivilisApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<RoleController> _logger;
        private DbConnectionStringResolver _dbConnectionStringResolver;
        private string connStr;

        public RoleController(IConfiguration config, ILogger<RoleController> logger)
        {
            this.configuration = config;
            _logger = logger;
            _logger.LogTrace(1, "Serilog injected into Role Controller");
            _dbConnectionStringResolver = new DbConnectionStringResolver(configuration);
            this.connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
        }


        // GET: api/<UserController>
        [HttpGet, Authorize]
        public ActionResult<List<tblRole>> Get()
        {
            this.connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            List<tblRole> objList;
            using (tblRoleDac dac = new tblRoleDac(this.connStr, _logger))
            {
                objList = dac.GetAll();
                if (objList == null)
                {
                    return NotFound();
                }
                return Ok(objList);
            }
        }

        // GET: api/<UserController>/5
        [HttpGet("{id}"), Authorize]
        public ActionResult<tblRole> Get(long id)
        {
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            tblRole obj;
            using (tblRoleDac dac = new tblRoleDac(connStr, _logger))
            {
                obj = dac.Get(id);
                if (obj != null)
                    return Ok(obj);
                else
                    return NotFound();
            }
        }


        [HttpGet("user/{id}"), Authorize]
        public ActionResult<List<vwUserRole>> GetRolesForUser(long id)
        {
            this.connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            List<vwUserRole> objList;
            using (vwUserRoleDac dac = new vwUserRoleDac(this.connStr, _logger))
            {
                objList = dac.GetAllForUserId(id);
                if (objList == null)
                {
                    return NotFound();
                }
                return Ok(objList);
            }
        }

        
    }
}
