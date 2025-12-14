using Microsoft.AspNetCore.Mvc;
using CivilisDB;
using Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using CivilisDB.Models;


namespace CivilisApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<SettingsController> _logger;
        private DbConnectionStringResolver _dbConnectionStringResolver;

        public SettingsController(IConfiguration config, ILogger<SettingsController> logger)
        {
            this.configuration = config;
            _logger = logger;
            _logger.LogTrace(1, "Serilog injected into Settings Controller");
            _dbConnectionStringResolver = new DbConnectionStringResolver(configuration);
        }


        // GET: api/<SettingsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            _logger.LogTrace(1, "Setting Get All is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            //List<tblEntity> objList;
            //long count = 0;
            //string message = "";
            //using (tblEntityDac dac = new tblEntityDac(connStr, _logger))
            //{
            //    try
            //    {
            //        objList = dac.GetAll();
            //        count += objList.Count;
            //    }

            //    catch (Exception ex) {
            //        message = message + ex.Message + "/" + ex.InnerException + "/" + ex.Source;
            //        _logger.LogError(1, "Setting Get All Failed");
            //    }
            //}

            return new string[] { "Version 0.92 ", " Connection " };
        }


    }
}
