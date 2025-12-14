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
    public class UserRoleController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<RoleController> _logger;
        private DbConnectionStringResolver _dbConnectionStringResolver;
        private string connStr;


        public UserRoleController(IConfiguration config, ILogger<RoleController> logger)
        {
            this.configuration = config;
            _logger = logger;
            _logger.LogTrace(1, "Serilog injected into UserRoleController Controller");
            _dbConnectionStringResolver = new DbConnectionStringResolver(configuration);
            this.connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
        }

        // GET: api/<UserRoleController>
        [HttpGet, Authorize]
        public ActionResult<List<tblUserRole>> Get()
        {
            _logger.LogTrace(1, "tblUserRole - Get All is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            List<tblUserRole> objList;
            using (tblUserRoleDac dac = new tblUserRoleDac(connStr, _logger))
            {
                try
                {
                    objList = dac.GetAll();
                    _logger.LogTrace(1, "DAC GetAll Called");
                    if (objList == null)
                    {
                        _logger.LogTrace(1, "Not Found");
                        return NotFound();
                    }
                    _logger.LogTrace(1, "No of Items:" + objList.Count().ToString());
                    return Ok(objList);
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    _logger.LogError(1, "NpgsqlException Code:" + ex.ErrorCode + " Message :" + ex.Message);
                    return BadRequest(ex.Message);
                }
            }
        }

        // GET api/<UserRoleController>/5
        [HttpGet("{id}"), Authorize]
        public ActionResult<tblUserRole> Get(long id)
        {
            _logger.LogTrace(1, "tblUserRole - Get one is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            tblUserRole obj;
            using (tblUserRoleDac dac = new tblUserRoleDac(connStr, _logger))
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
                    _logger.LogError(1, "tblUserRole Controller - NpgSQL Error:" + ex.Message);
                    return BadRequest(ex.Message);
                }

            }
        }

        // POST api/<UserRoleController>
        [HttpPost, Authorize]
        public IActionResult Post([FromBody] tblUserRole obj)
        {
            _logger.LogTrace(1, "tblUserRole - Post is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            long userId = Convert.ToInt64(User.Identity.Name);
            using (tblUserRoleDac dac = new tblUserRoleDac(connStr, _logger))
            {
                try
                {
                    obj.fldcreatedby = userId;
                    obj.fldupdatedby = userId;
                    obj.fldcreatedat = DateTime.Now;
                    obj.fldupdatedat = DateTime.Now;
                    bool res = dac.Insert(obj);
                    if (res)
                        return Ok(obj);
                    else
                        return Conflict();
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        // PUT api/<UserRoleController>/5
        [HttpPut("{id}"), Authorize]
        public IActionResult Put(int id, [FromBody] tblUserRole obj)
        {
            _logger.LogTrace(1, "tblUserRole - Put All is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            long userId = Convert.ToInt64(User.Identity.Name);
            using (tblUserRoleDac dac = new tblUserRoleDac(connStr, _logger))
            {
                try
                {
                    obj.fldupdatedby = userId;
                    obj.fldupdatedat = DateTime.Now;
                    bool isSuccess = dac.Update(obj);
                    if (isSuccess)
                        return Ok(obj);
                    else
                        return Conflict();
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        // DELETE api/<UserRoleController>/5
        [HttpDelete("{id}"), Authorize]
        public IActionResult Delete(int id)
        {
            _logger.LogTrace(1, "tblUserRole - Delete is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            using (tblUserRoleDac dac = new tblUserRoleDac(connStr, _logger))
            {
                try
                {
                    bool isSuccess = dac.Delete(id);
                    if (isSuccess)
                        return Ok();
                    else
                        return NotFound();
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}
