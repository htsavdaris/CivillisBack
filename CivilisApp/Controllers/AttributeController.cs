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
    public class AttributeController : ControllerBase
    {

        private readonly IConfiguration configuration;
        private readonly ILogger<AttributeController> _logger;
        private DbConnectionStringResolver _dbConnectionStringResolver;


        public AttributeController(IConfiguration config, ILogger<AttributeController> logger)
        {
            this.configuration = config;
            _logger = logger;
            _logger.LogTrace(1, "Serilog injected into AttributeController");
            _dbConnectionStringResolver = new DbConnectionStringResolver(configuration);
        }

        // GET: api/<AttributeController>
        [HttpGet, Authorize]
        public ActionResult<List<tblAttribute>> Get()
        {
            _logger.LogTrace(1, "tblAttribute - Get All is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            List<tblAttribute> objList;
            using (tblAttributeDac dac = new tblAttributeDac(connStr, _logger))
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

        // GET api/<AttributeController>/5
        [HttpGet("{id}"),Authorize]
        public ActionResult<tblAttribute> Get(int id)
        {
            _logger.LogTrace(1, "tblAttribute - Get one is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            tblAttribute obj;
            using (tblAttributeDac dac = new tblAttributeDac(connStr, _logger))
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
                    _logger.LogError(1, "tblAttribute Controller - NpgSQL Error:" + ex.Message);
                    return BadRequest(ex.Message);
                }

            }
        }

        // POST api/<Controller>
        [HttpPost, Authorize]
        public IActionResult Post([FromBody] tblAttribute obj)
        {
            _logger.LogTrace(1, "tblAttribute - Post/Insert is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            using (tblAttributeDac dac = new tblAttributeDac(connStr, _logger))
            {
                try
                {
                    obj.fldcreatedat = DateTime.Now;
                    obj.fldupdatedat = DateTime.Now;
                    var res = dac.Insert(obj);
                    if (res)
                    {
                        //obj.fldattributeid = id.Value;
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
        public IActionResult Put(int id, [FromBody] tblAttribute obj)
        {
            _logger.LogTrace(1, "tblAttribute - Put/Update is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            using (tblAttributeDac dac = new tblAttributeDac(connStr, _logger))
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

        // DELETE api/<AttributeController>/5
        [HttpDelete("{id}"), Authorize]
        public IActionResult Delete(long id)
        {
            _logger.LogTrace(1, "tblAttribute - Delete is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            using (tblAttributeDac dac = new tblAttributeDac(connStr, _logger))
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
