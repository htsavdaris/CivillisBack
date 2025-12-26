using Microsoft.AspNetCore.Mvc;
using CivilisDB;
using Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using CivilisDB.Models;


namespace CivilisApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvitationController : ControllerBase
    {

        private readonly IConfiguration configuration;
        private readonly ILogger<InvitationController> _logger;
        private DbConnectionStringResolver _dbConnectionStringResolver;

        public InvitationController(IConfiguration config, ILogger<InvitationController> logger)
        {
            this.configuration = config;
            _logger = logger;
            _logger.LogTrace(1, "Serilog injected into Invitation Controller");
            _dbConnectionStringResolver = new DbConnectionStringResolver(configuration);
        }



        // GET: api/<InvitationController>
        [HttpGet]
        public ActionResult<List<tblInvitation>> Get()
        {
            _logger.LogTrace(1, "tblInvitation - Get All is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            List<tblInvitation> objList;
            using (tblInvitationDac dac = new tblInvitationDac(connStr, _logger))
            {
                try
                {
                    objList = dac.GetAllSortedDescByUpdate();
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

        [HttpGet("{id}"), Authorize]
        public ActionResult<tblInvitation> Get(long id)
        {
            _logger.LogTrace(1, "tblInvitation - Get one is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            tblInvitation obj;
            using (tblInvitationDac dac = new tblInvitationDac(connStr, _logger))
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
                    _logger.LogError(1, "tblInvitation Controller - NpgSQL Error:" + ex.Message);
                    return BadRequest(ex.Message);
                }

            }
        }

        // POST api/<EntityController>
        [HttpPost, Authorize]
        public IActionResult Post([FromBody] tblInvitation obj)
        {
            _logger.LogTrace(1, "tblInvitation - Post is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            long userId = Convert.ToInt64(User.Identity.Name);
            using (tblInvitationDac dac = new tblInvitationDac(connStr, _logger))
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

        // PUT api/<InvitationController>/5
        [HttpPut("{id}"), Authorize]
        public IActionResult Put(int id, [FromBody] tblInvitation obj)
        {
            _logger.LogTrace(1, "tblInvitation - Put All is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            long userId = Convert.ToInt64(User.Identity.Name);
            using (tblInvitationDac dac = new tblInvitationDac(connStr, _logger))
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

        // DELETE api/<InvitationController>/5
        [HttpDelete("{id}"), Authorize]
        public void Delete(int id)
        {
        }
    }
}
