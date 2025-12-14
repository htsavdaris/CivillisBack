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
    public class EklogikiDomiController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<EklogikiDomiController> _logger;
        private DbConnectionStringResolver _dbConnectionStringResolver;

        public EklogikiDomiController(IConfiguration config, ILogger<EklogikiDomiController> logger)
        {
            this.configuration = config;
            _logger = logger;
            _logger.LogTrace(1, "Serilog injected into EklogikiDomiController Controller");
            _dbConnectionStringResolver = new DbConnectionStringResolver(configuration);
        }

        // GET: api/<EklogikiDomiController>
        [HttpGet, Authorize]
        public ActionResult<List<tblEklogikiDomi>> Get()
        {
            _logger.LogTrace(1, "tblEklogikiDomi - Get All is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            List<tblEklogikiDomi> objList;
            using (tblEklogikiDomiDac dac = new tblEklogikiDomiDac(connStr, _logger))
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

        // GET api/<EklogikiDomi>/5
        [HttpGet("{id}"), Authorize]
        public ActionResult<tblEklogikiDomi> Get(string id)
        {
            _logger.LogTrace(1, "tblEklogikiDomi - Get one is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            tblEklogikiDomi obj;
            using (tblEklogikiDomiDac dac = new tblEklogikiDomiDac(connStr, _logger))
            {
                try
                {
                    obj = dac.GetByDomiId(id);
                    if (obj != null)
                        return Ok(obj);
                    else
                        return NotFound();
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    _logger.LogError(1, "tblEklogikiDomi Controller - NpgSQL Error:" + ex.Message);
                    return BadRequest(ex.Message);
                }

            }
        }


        [HttpGet("type/{id}"), Authorize]
        public ActionResult<List<tblEklogikiDomi>> GetListofType(long id)
        {
            _logger.LogTrace(1, "tblEklogikiDomi - GetListofType one is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            List<tblEklogikiDomi> objList;
            using (tblEklogikiDomiDac dac = new tblEklogikiDomiDac(connStr, _logger))
            {
                try
                {
                    objList = dac.GetByDomiTypeId(id);
                    if (objList != null)
                        return Ok(objList);
                    else
                        return NotFound();
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    _logger.LogError(1, "tblEklogikiDomi Controller - NpgSQL Error:" + ex.Message);
                    return BadRequest(ex.Message);
                }

            }
        }

        // POST api/<EklogikiDomi>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<EklogikiDomi>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<EklogikiDomi>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
