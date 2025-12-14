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
    public class PersonAttributeController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<PersonAttributeController> _logger;
        private DbConnectionStringResolver _dbConnectionStringResolver;

        public PersonAttributeController(IConfiguration config, ILogger<PersonAttributeController> logger)
        {
            this.configuration = config;
            _logger = logger;
            _logger.LogTrace(1, "Serilog injected into PersonAttribute Controller");
            _dbConnectionStringResolver = new DbConnectionStringResolver(configuration);
        }

        [HttpGet, Authorize]
        public ActionResult<List<tblPersonAttribute>> Get()
        {
            _logger.LogTrace(1, "tblPersonAttribute - Get All is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            List<tblPersonAttribute> objList;
            using (tblPersonAttributeDac dac = new tblPersonAttributeDac(connStr, _logger))
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
        public ActionResult<tblPersonAttribute> Get(long id)
        {
            _logger.LogTrace(1, "tblPersonAttribute - Get one is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            tblPersonAttribute obj;
            using (tblPersonAttributeDac dac = new tblPersonAttributeDac(connStr, _logger))
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
                    _logger.LogError(1, "tblPersonAttribute Controller - NpgSQL Error:" + ex.Message);
                    return BadRequest(ex.Message);
                }

            }
        }


        [HttpPost, Authorize]
        public IActionResult Post([FromBody] tblPersonAttribute obj)
        {
            _logger.LogTrace(1, "tblPersonAttribute - Post is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            tblPersonAttributesAggregate personAgg; 

            using (tblPersonAttributeDac dac = new tblPersonAttributeDac(connStr, _logger))
            {
                try
                {
                    obj.fldcreatedat = DateTime.Now;
                    obj.fldupdatedat = DateTime.Now;
                    bool res = dac.Insert(obj);
                    if (res)
                    {
                        tblPersonAttributesAggregateDac dac2 = new tblPersonAttributesAggregateDac(connStr, _logger);
                        dac2.SavInsert(obj.fldpersonid);

                        //obj.fldpersonattrid = id.Value;
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
        public IActionResult Put(int id, [FromBody] tblPersonAttribute obj)
        {
            _logger.LogTrace(1, "tblPersonAttribute - Put All is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            using (tblPersonAttributeDac dac = new tblPersonAttributeDac(connStr, _logger))
            {
                try
                {
                    obj.fldupdatedat = DateTime.Now;
                    bool isSuccess = dac.Update(obj);
                    if (isSuccess)
                    {
                        tblPersonAttributesAggregateDac dac2 = new tblPersonAttributesAggregateDac(connStr, _logger);
                        dac2.SavInsert(obj.fldpersonid);
                        return Ok();
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


        [HttpDelete("{id}"), Authorize]
        public IActionResult Delete(int id)
        {
            _logger.LogTrace(1, "tblPersonAttribute - Delete is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            tblPersonAttribute obj;
            using (tblPersonAttributeDac dac = new tblPersonAttributeDac(connStr, _logger))
            {
                try
                {
                    obj = dac.Get(id);
                    bool isSuccess = dac.Delete(id);
                    if (isSuccess)
                    {
                        tblPersonAttributesAggregateDac dac2 = new tblPersonAttributesAggregateDac(connStr, _logger);
                        dac2.SavInsert(obj.fldpersonid);
                        return Ok();
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



    }
}
