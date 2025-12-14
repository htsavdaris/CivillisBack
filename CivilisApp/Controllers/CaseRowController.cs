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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CivilisApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaseRowController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<CaseRowController> _logger;
        private DbConnectionStringResolver _dbConnectionStringResolver;

        public CaseRowController(IConfiguration config, ILogger<CaseRowController> logger)
        {
            this.configuration = config;
            _logger = logger;
            _logger.LogTrace(1, "Serilog injected into CaseRow Controller");
            _dbConnectionStringResolver = new DbConnectionStringResolver(configuration);
        }


        [HttpGet("{id}"), Authorize]
        public ActionResult<tblCaseRow> Get(long id)
        {
            _logger.LogTrace(1, "CaseRow - Get one is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            tblCaseRow obj;
            using (tblCaseRowDac dac = new tblCaseRowDac(connStr, _logger))
            {
                try
                {
                    obj = dac.Get(id);
                    if (obj != null)
                        return Ok(obj);
                    else
                        return NoContent();
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    _logger.LogError(1, "CaseRow Controller - NpgSQL Error:" + ex.Message);
                    return BadRequest(ex.Message);
                }

            }
        }

        [HttpGet("withCaseid/{id}"), Authorize]
        public ActionResult<tblCaseRow> GetWithCaseID(long id)
        {
            _logger.LogTrace(1, "CaseRow - GetWithCaseID is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            List<tblCaseRow> objList;
            using (tblCaseRowDac dac = new tblCaseRowDac(connStr, _logger))
            {
                try
                {
                    objList = dac.GetByCaseID(id);
                    if (objList != null)
                        return Ok(objList);
                    else
                        return NoContent();
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    _logger.LogError(1, "CaseRow - GetWithCaseID Controller - NpgSQL Error:" + ex.Message);
                    return BadRequest(ex.Message);
                }

            }
        }

        // GET api/<PersonController>
        [HttpGet, Authorize]
        public ActionResult<List<tblCaseRow>> Get()
        {
            _logger.LogTrace(1, "CaseRow - Get All is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            List<tblCaseRow> objList;
            using (tblCaseRowDac dac = new tblCaseRowDac(connStr, _logger))
            {
                try
                {
                    objList = dac.GetAll();
                    _logger.LogTrace(1, "DAC GetAll Called");
                    if (objList == null)
                    {
                        _logger.LogTrace(1, "Not Found");
                        return NoContent();
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

        // POST api/<PersonController>
        [HttpPost, Authorize]
        public IActionResult Post([FromBody] tblCaseRow obj)
        {
            _logger.LogTrace(1, "tblCaseRow - Post is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            using (tblCaseRowDac dac = new tblCaseRowDac(connStr, _logger))
            {
                try
                {
                    obj.fldcreatedat = DateTime.Now;
                    obj.fldupdatedat = DateTime.Now;
                    bool res = dac.Insert(obj);
                    if (res)
                    {
                        //obj.fldcaserowid = id.Value;
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

        // PUT api/<PersonController>/5
        [HttpPut("{id}"), Authorize]
        public IActionResult Put(int id, [FromBody] tblCaseRow obj)
        {
            _logger.LogTrace(1, "tblCaseRow - Put All is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            using (tblCaseRowDac dac = new tblCaseRowDac(connStr, _logger))
            {
                try
                {
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

        // DELETE api/<PersonController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }



    }
}
