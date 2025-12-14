using Microsoft.AspNetCore.Mvc;
using Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using CivilisDB;
using CivilisDB.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CivilisApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CaseTypeController : ControllerBase
    {


        private readonly IConfiguration configuration;
        private readonly ILogger<CaseTypeController> _logger;
        private DbConnectionStringResolver _dbConnectionStringResolver;


        public CaseTypeController(IConfiguration config, ILogger<CaseTypeController> logger)
        {
            this.configuration = config;
            _logger = logger;
            _logger.LogTrace(1, "Serilog injected into CaseStatus Controller");
            _dbConnectionStringResolver = new DbConnectionStringResolver(configuration);
        }

        [HttpGet, Authorize]
        public ActionResult<List<tblCaseType>> GetAll()
        {
            _logger.LogTrace(1, "tblCaseType - Get All is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            List<tblCaseType> objList;
            using (tblCaseTypeDac dac = new tblCaseTypeDac(connStr, _logger))
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


        [HttpGet("{id}"), Authorize]
        public ActionResult<List<tblCaseType>> GetOne(long id)
        {
            _logger.LogTrace(1, "tblCaseType - Get one is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            tblCaseType obj;
            using (tblCaseTypeDac dac = new tblCaseTypeDac(connStr, _logger))
            {
                try
                {
                    obj = dac.Get(id);
                    _logger.LogTrace(1, "DAC GetOne Called");
                    if (obj == null)
                    {
                        _logger.LogTrace(1, "Not Found");
                        return NotFound();
                    }
                    return Ok(obj);
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    _logger.LogError(1, "NpgsqlException Code:" + ex.ErrorCode + " Message :" + ex.Message);
                    return BadRequest(ex.Message);
                }
            }
        }

        // POST api/<Controller>
        [HttpPost, Authorize]
        public IActionResult Post([FromBody] tblCaseType obj)
        {
            _logger.LogTrace(1, "tblCaseType - Post is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            using (tblCaseTypeDac dac = new tblCaseTypeDac(connStr, _logger))
            {
                try
                {
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
        public IActionResult Put(int id, [FromBody] tblCaseType obj)
        {
            _logger.LogTrace(1, "tblCaseType - Put All is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            using (tblCaseTypeDac dac = new tblCaseTypeDac(connStr, _logger))
            {
                try
                {
                    obj.fldupdatedat = DateTime.Now;
                    bool isSuccess = dac.Update(obj);
                    if (isSuccess)
                        return Ok();
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
