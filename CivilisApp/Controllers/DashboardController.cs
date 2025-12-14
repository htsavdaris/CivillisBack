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
    public class DashboardController : ControllerBase
    {

        private readonly IConfiguration configuration;
        private readonly ILogger<PersonController> _logger;
        private DbConnectionStringResolver _dbConnectionStringResolver;

        public DashboardController(IConfiguration config, ILogger<PersonController> logger)
        {
            this.configuration = config;
            _logger = logger;
            _logger.LogTrace(1, "Serilog injected into Dashboard Controller");
            _dbConnectionStringResolver = new DbConnectionStringResolver(configuration);
        }

        [HttpGet("generatedash/{personid}"), Authorize]
        // GET api/<DashboardController>/5
        public ActionResult<int> GenerateDash(long personid)
        {
            _logger.LogTrace(1, "Dashboard - GenerateDash All is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            long obj;
            using (tblDashboardDac dac = new tblDashboardDac(connStr, _logger))
            {
                try
                {
                    obj = dac.GenerateDash(personid);
                    _logger.LogTrace(1, "DAC GenerateDash Called");
                    return Ok(obj);
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    _logger.LogError(1, "NpgsqlException Code:" + ex.ErrorCode + " Message :" + ex.Message);
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpGet("getdashforperson/{personid}"), Authorize]
        // GET api/<DashboardController>/5
        public ActionResult<int> GetDashForPerson(long personid)
        {
            _logger.LogTrace(1, "Dashboard - GetDashForPerson All is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            tblDashboard obj;
            using (tblDashboardDac dac = new tblDashboardDac(connStr, _logger))
            {
                try
                {
                    obj = dac.Get(personid);
                    _logger.LogTrace(1, "DAC Get Called");
                    return Ok(obj);
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    _logger.LogError(1, "NpgsqlException Code:" + ex.ErrorCode + " Message :" + ex.Message);
                    return BadRequest(ex.Message);
                }
            }
        }

        // GET: api/<DashboardController>
        [HttpGet("opencases"), Authorize]
        // GET api/<DashboardController>/5
        public ActionResult<long> GetOpenCases()
        {
            _logger.LogTrace(1, "Dashboard - GetOpenCases All is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            long obj;
            using (DashboardDac dac = new DashboardDac(connStr, _logger))
            {
                try
                {
                    obj = dac.GetNoOfOpenCases();
                    _logger.LogTrace(1, "DAC GetNoOfOpenCases Called");
                    return Ok(obj);
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    _logger.LogError(1, "NpgsqlException Code:" + ex.ErrorCode + " Message :" + ex.Message);
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpGet("cases"), Authorize]
        // GET api/<DashboardController>/5
        public ActionResult<long> GetCases()
        {
            _logger.LogTrace(1, "Dashboard - GetOpenCases All is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            long obj;
            using (DashboardDac dac = new DashboardDac(connStr, _logger))
            {
                try
                {
                    obj = dac.GetNoOfCases();
                    _logger.LogTrace(1, "DAC GetNoOfOpenCases Called");
                    return Ok(obj);
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    _logger.LogError(1, "NpgsqlException Code:" + ex.ErrorCode + " Message :" + ex.Message);
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpGet("pendinginvitations"), Authorize]
        // GET api/<DashboardController>/5
        public ActionResult<long> GetPendingInvitations()
        {
            _logger.LogTrace(1, "Dashboard - GetPendingInvitations All is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            long obj;
            using (DashboardDac dac = new DashboardDac(connStr, _logger))
            {
                try
                {
                    obj = dac.GetNoOfPendingInvitations();
                    _logger.LogTrace(1, "DAC GetPendingInvitations Called");
                    return Ok(obj);
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    _logger.LogError(1, "NpgsqlException Code:" + ex.ErrorCode + " Message :" + ex.Message);
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpGet("invitations"), Authorize]
        // GET api/<DashboardController>/5
        public ActionResult<long> GetInvitations()
        {
            _logger.LogTrace(1, "Dashboard - GetInvitations All is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            long obj;
            using (DashboardDac dac = new DashboardDac(connStr, _logger))
            {
                try
                {
                    obj = dac.GetNoOfInvitations();
                    _logger.LogTrace(1, "DAC GetInvitations Called");
                    return Ok(obj);
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    _logger.LogError(1, "NpgsqlException Code:" + ex.ErrorCode + " Message :" + ex.Message);
                    return BadRequest(ex.Message);
                }
            }
        }


        [HttpGet("futureinvitations"), Authorize]
        // GET api/<DashboardController>/5
        public ActionResult<long> GetFutureInvitations()
        {
            _logger.LogTrace(1, "Dashboard - GetFutureInvitations All is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            long obj;
            using (DashboardDac dac = new DashboardDac(connStr, _logger))
            {
                try
                {
                    obj = dac.GetNoOfInvitations();
                    _logger.LogTrace(1, "DAC GetInvitations Called");
                    return Ok(obj);
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
