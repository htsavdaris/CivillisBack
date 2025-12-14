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
    public class CrudBaseController<T,S> : ControllerBase 
            where T : BaseEntity
            where S: DapperBaseRepository<T>, new()
    {

        private readonly IConfiguration configuration;
        private readonly ILogger<CrudBaseController<T,S>> _logger;
        private DbConnectionStringResolver _dbConnectionStringResolver;

        public CrudBaseController(IConfiguration config, ILogger<CrudBaseController<T,S>> logger)
        {
            this.configuration = config;
            _logger = logger;
            _logger.LogTrace(1, "Serilog injected into" + typeof(T).FullName + " Controller" );
            _dbConnectionStringResolver = new DbConnectionStringResolver(configuration);
        }


        
        [HttpGet, Authorize]
        public ActionResult<List<T>> Get()
        {
            _logger.LogTrace(1, typeof(T).FullName + " - Get All is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            List<T> objectList = null;

            using (S dac = new ()) 
            {
                dac.ConnectionString = connStr;
                //        try
                //        {
                //            objectList = dac.GetAll();
                //            _logger.LogTrace(1, "DAC GetAll Called");
                //            if (personList == null)
                //            {
                //                _logger.LogTrace(1, "Not Found");
                //                return NotFound();
                //            }
                //            _logger.LogTrace(1, "No of Items:" + personList.Count().ToString());
                //            return Ok(personList);
                //        }
                //        catch (Npgsql.NpgsqlException ex)
                //        {
                //            _logger.LogError(1, "NpgsqlException Code:" + ex.ErrorCode + " Message :" + ex.Message);
                //            return BadRequest(ex.Message);
                //        }
            }
            return Ok(objectList);
        }

    }
}
