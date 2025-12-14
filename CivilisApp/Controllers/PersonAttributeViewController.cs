using CivilisDB.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using CivilisDB;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CivilisApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonAttributeViewController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<PersonAttributeViewController> _logger;
        private DbConnectionStringResolver _dbConnectionStringResolver;

        public PersonAttributeViewController(IConfiguration config, ILogger<PersonAttributeViewController> logger)
        {
            this.configuration = config;
            _logger = logger;
            _logger.LogTrace(1, "Serilog injected into PersonAttribute Controller");
            _dbConnectionStringResolver = new DbConnectionStringResolver(configuration);
        }

        // GET: api/<PersonAttributeController>
        [HttpGet("personid/{personid}")]
        public ActionResult<List<vwPersonAttribute>> GetAllForPersonId(long personid)
        {
            _logger.LogTrace(1, "vwPersonAttribute - GetAllForPersonId is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            List<vwPersonAttribute> obj;
            using (vwPersonAttributeDac dac = new vwPersonAttributeDac(connStr, _logger))
            {
                try
                {
                    obj = dac.GetAllForPersonId(personid);
                    if (obj != null)
                        return Ok(obj);
                    else
                        return NotFound();
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    _logger.LogError(1, "vwPersonAttribute Controller - NpgSQL Error:" + ex.Message);
                    return BadRequest(ex.Message);
                }

            }
        }

        // GET api/<PersonAttributeController>/5
        [HttpGet("attrid/{attrid}")]
        public ActionResult<List<vwPersonAttribute>> GetAllForAttrId(int attrid)
        {
            _logger.LogTrace(1, "vwPersonAttribute - GetAllForAttrId is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            List<vwPersonAttribute> obj;
            using (vwPersonAttributeDac dac = new vwPersonAttributeDac(connStr, _logger))
            {
                try
                {
                    obj = dac.GetAllForAttrId(attrid);
                    if (obj != null)
                        return Ok(obj);
                    else
                        return NotFound();
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    _logger.LogError(1, "vwPersonAttribute Controller - NpgSQL Error:" + ex.Message);
                    return BadRequest(ex.Message);
                }

            }
        }
    }
}
