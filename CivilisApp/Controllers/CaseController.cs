using Microsoft.AspNetCore.Mvc;
using Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using CivilisDB;
using CivilisDB.Models;



namespace CivilisApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaseController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<CaseController> _logger;
        private DbConnectionStringResolver _dbConnectionStringResolver;

        public CaseController(IConfiguration config, ILogger<CaseController> logger)
        {
            this.configuration = config;
            _logger = logger;
            _logger.LogTrace(1, "Serilog injected into Case Controller");
            _dbConnectionStringResolver = new DbConnectionStringResolver(configuration);
        }


        // GET: api/<CaseController>
        [HttpGet, Authorize]
        public ActionResult<List<tblCase>> Get()
        {
            _logger.LogTrace(1, "tblCase - Get All is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            List<tblCase> objList;
            using (tblCaseDac dac = new tblCaseDac(connStr, _logger))
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

        // GET api/<CaseController>/5
        [HttpGet("{id}"), Authorize]
        public ActionResult<tblCase> Get(long id)
        {
            _logger.LogTrace(1, "tblCase - Get one is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            tblCase obj;
            using (tblCaseDac dac = new tblCaseDac(connStr, _logger))
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
                    _logger.LogError(1, "tblCase Controller - NpgSQL Error:" + ex.Message);
                    return BadRequest(ex.Message);
                }

            }
        }

        [HttpGet("status/{id}"), Authorize]
        public ActionResult<List<tblCase>> GetByStatusandDate(long statusid)
        {
            _logger.LogTrace(1, "tblCase - GetByStatusandDate one is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            List<tblCase> obj;
            using (tblCaseDac dac = new tblCaseDac(connStr, _logger))
            {
                try
                {
                    obj = dac.GetByStatusByDate(statusid);
                    if (obj != null)
                        return Ok(obj);
                    else
                        return NotFound();
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    _logger.LogError(1, "tblCase Controller - NpgSQL Error:" + ex.Message);
                    return BadRequest(ex.Message);
                }

            }
        }

        // POST api/<Controller>
        [HttpPost, Authorize]
        public IActionResult Post([FromBody] tblCase obj)
        {
            _logger.LogTrace(1, "tblCase - Post is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            long userId = Convert.ToInt64(User.Identity.Name);
            using (tblCaseDac dac = new tblCaseDac(connStr, _logger))
            {
                try
                {
                    obj.fldcreatedby = userId;
                    obj.fldupdatedby = userId;
                    obj.fldcreatedat = DateTime.Now;
                    obj.fldupdatedat = DateTime.Now;
                    bool res = dac.Insert(obj);
                    if (res)
                    {
                        //obj.fldcasetypeid = id.Value;
                        return Ok(obj);
                    }
                    else
                        return Conflict();
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        // PUT api/<Controller>/5
        [HttpPut("{id}"), Authorize]
        public IActionResult Put(int id, [FromBody] tblCase obj)
        {
            _logger.LogTrace(1, "tblCase - Put All is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            long userId = Convert.ToInt64(User.Identity.Name);
            using (tblCaseDac dac = new tblCaseDac(connStr, _logger))
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

        // DELETE api/<EntityController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

    }
}
