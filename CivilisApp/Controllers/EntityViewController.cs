using CivilisApp.Models;
using CivilisDB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace CivilisApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntityViewController : ControllerBase
    {

        private readonly IConfiguration configuration;
        private readonly ILogger<EntityViewController> _logger;
        private DbConnectionStringResolver _dbConnectionStringResolver;

        public EntityViewController(IConfiguration config, ILogger<EntityViewController> logger)
        {
            this.configuration = config;
            _logger = logger;
            _logger.LogTrace(1, "Serilog injected into Entity Controller");
            _dbConnectionStringResolver = new DbConnectionStringResolver(configuration);
        }



        [HttpGet, Authorize]
        public ActionResult<List<vwEntity>> Get()
        {
            _logger.LogTrace(1, "vwEntityController - Get All Sorted is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            List<vwEntity> objList;
            using (vwEntityDac dac = new(connStr, _logger))
            {
                try
                {
                    objList = dac.GetAllSortedByID();
                    _logger.LogTrace(1, "DAC GetAll Called");
                    if (objList == null)
                    {
                        _logger.LogTrace(1, "Not Found");
                        return NotFound();
                    }
                    _logger.LogTrace(1, "No of Items:" + objList.Count.ToString());
                    return Ok(objList);
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    _logger.LogError(1, "NpgsqlException Code:" + ex.ErrorCode + " Message :" + ex.Message);
                    return BadRequest(ex.Message);
                }
            }
        }


        

        // GET api/<EntityEntityTypeViewController>/5
        [HttpGet("{id}"), Authorize]
        public ActionResult<vwEntity> Get(int id)
        {
            _logger.LogTrace(1, "vwEntityController - Get one is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            vwEntity obj;
            using (vwEntityDac dac = new vwEntityDac(connStr, _logger))
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
                    _logger.LogError(1, "vwEntityController Controller - NpgSQL Error:" + ex.Message);
                    return BadRequest(ex.Message);
                }

            }
        }


       

    }
}
