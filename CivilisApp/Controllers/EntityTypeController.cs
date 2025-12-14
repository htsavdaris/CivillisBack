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
    public class EntityTypeController : ControllerBase
    {

        private readonly IConfiguration configuration;
        private readonly ILogger<tblEntityType> _logger;
        private DbConnectionStringResolver _dbConnectionStringResolver;


        public EntityTypeController(IConfiguration config, ILogger<tblEntityType> logger)
        {
            this.configuration = config;
            _logger = logger;
            _logger.LogTrace(1, "Serilog injected into Person Controller");
            _dbConnectionStringResolver = new DbConnectionStringResolver(configuration);
        }

        [HttpGet, Authorize]
        public ActionResult<List<tblEntityType>> Get()
        {
            _logger.LogTrace(1, "tblEntityType - Get All is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            List<tblEntityType> objList;
            using (tblEntityTypeDac dac = new tblEntityTypeDac(connStr, _logger))
            {
                try
                {
                    objList = dac.GetAllSortedDesc();
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

        // GET api/<EntityController>/5
        [HttpGet("{id}"), Authorize]
        public ActionResult<tblEntityType> Get(long id)
        {
            _logger.LogTrace(1, "tblEntityType - Get one is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            tblEntityType obj;
            using (tblEntityTypeDac dac = new tblEntityTypeDac(connStr, _logger))
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
                    _logger.LogError(1, "Entity Controller - NpgSQL Error:" + ex.Message);
                    return BadRequest(ex.Message);
                }

            }
        }


        // POST api/<Controller>
        [HttpPost, Authorize]
        public IActionResult Post([FromBody] tblEntityType obj)
        {
            _logger.LogTrace(1, "tblEntityType - Post/Insert is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            using (tblEntityTypeDac dac = new tblEntityTypeDac(connStr, _logger))
            {
                try
                {
                    obj.fldcreatedat = DateTime.Now;
                    obj.fldupdatedat = DateTime.Now;
                    bool res = dac.Insert(obj);
                    if (res)
                    {
                        //obj.fldentitytypeid = id.Value;
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
        public IActionResult Put(int id, [FromBody] tblEntityType obj)
        {
            _logger.LogTrace(1, "tblEntityType - Put/Update is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            using (tblEntityTypeDac dac = new tblEntityTypeDac(connStr, _logger))
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

        //// DELETE api/<EntityController>/5
        [HttpDelete("{id}"),Authorize]
        public IActionResult Delete(long id)
        {
            _logger.LogTrace(1, "tblEntityType - Delete is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            using (tblEntityTypeDac dac = new tblEntityTypeDac(connStr, _logger))
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
