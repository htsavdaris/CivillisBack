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
    public class CaseStatusController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<CaseStatusController> _logger;
        private DbConnectionStringResolver _dbConnectionStringResolver;

        public CaseStatusController(IConfiguration config, ILogger<CaseStatusController> logger)
        {
            this.configuration = config;
            _logger = logger;
            _logger.LogTrace(1, "Serilog injected into CaseStatus Controller");
            _dbConnectionStringResolver = new DbConnectionStringResolver(configuration);
        }

        [HttpGet]
        public ActionResult<List<tblCaseStatus>> Get()
        {
            _logger.LogTrace(1, "tblCaseStatus - Get All is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            List<tblCaseStatus> objList;
            using (tblCaseStatusDac dac = new tblCaseStatusDac(connStr, _logger))
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

        
    }
}
