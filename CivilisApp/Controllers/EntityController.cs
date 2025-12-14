using Microsoft.AspNetCore.Mvc;
using Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using CivilisDB;
using CivilisDB.Models;
using Npgsql;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;
using CivilisApp.Models;
using System.Xml;

namespace CivilisApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntityController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<PersonController> _logger;
        private DbConnectionStringResolver _dbConnectionStringResolver;

        public EntityController(IConfiguration config, ILogger<PersonController> logger)
        {
            this.configuration = config;
            _logger = logger;
            _logger.LogTrace(1, "Serilog injected into Entity Controller");
            _dbConnectionStringResolver = new DbConnectionStringResolver(configuration);
        }

        
        [HttpGet, Authorize]
        public ActionResult<List<tblEntity>> Get()
        {
            _logger.LogTrace(1, "tblEntity - Get All is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            List<tblEntity> objList;
            using (tblEntityDac dac = new tblEntityDac(connStr, _logger))
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

        // GET api/<EntityController>/5
        [HttpGet("{id}"), Authorize]
        public ActionResult<tblEntity> Get(long id)
        {
            _logger.LogTrace(1, "tblEntity - Get one is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            tblEntity obj;
            using (tblEntityDac dac = new tblEntityDac(connStr, _logger))
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
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }

            }
        }

        [HttpGet("getByName/{name}"), Authorize]
        public ActionResult<tblEntity> GetByName(string name)
        {
            _logger.LogTrace(1, "tblEntity - GetByName one is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            tblEntity obj;
            using (tblEntityDac dac = new tblEntityDac(connStr, _logger))
            {
                try
                {
                    obj = dac.GetbyName(name);
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

        [HttpPost("search1"), Authorize]
        public ActionResult<List<tblEntity>> GetSearch1([FromBody] SearchEntity1 obj)
        {
            _logger.LogTrace(1, "tblPerson - GetSearch1 is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            List<tblEntity> objList;
            var connec = new NpgsqlConnection(connStr);

            var db = new QueryFactory(connec, new PostgresCompiler());
            var compiler = new PostgresCompiler();
            var query = db.Query("tblentity");
            if (obj.fldentityname != "")
            {
                query.WhereLike("fldentityname", "%" + obj.fldentityname + "%");
            }
            if (obj.fldmobile != "")
            {
                query.WhereLike("fldmobile", "%" + obj.fldmobile + "%");
            }
            if (obj.fldcity != "")
            {
                query.WhereLike("fldcity", obj.fldcity + "%");
            }
            if (obj.fldentitytypeid != 0)
            {
                query.Where("fldentitytypeid", obj.fldentitytypeid);
            }
            SqlResult result = compiler.Compile(query);
            try
            {

                objList = query.Get<tblEntity>().ToList();
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









        // POST api/<EntityController>
        [HttpPost, Authorize]
        public IActionResult Post([FromBody] tblEntity obj)
        {
            _logger.LogTrace(1, "tblEntity - Post is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            long userId = Convert.ToInt64(User.Identity.Name);
            using (tblEntityDac dac = new tblEntityDac(connStr, _logger))
            {
                try
                {
                    obj.fldcreatedby = userId;
                    obj.fldupdatedby = userId;
                    obj.fldcreatedat = DateTime.Now;
                    obj.fldupdatedat = DateTime.Now;
                    var res = dac.Insert(obj);
                    if (res)
                    {
                        // obj.fldentityid = id.Value;
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

        // PUT api/<EntityController>/5
        [HttpPut("{id}"), Authorize]
        public IActionResult Put(int id, [FromBody] tblEntity obj)
        {
            _logger.LogTrace(1, "tblEntity - Put All is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            long userId = Convert.ToInt64(User.Identity.Name);
            using (tblEntityDac dac = new tblEntityDac(connStr, _logger))
            {
                try
                {
                    obj.fldupdatedby = userId;
                    obj.fldupdatedat = DateTime.Now;
                    bool isSuccess = dac.Update(obj);
                    if (isSuccess)
                        return Ok(obj);
                    else
                        return Conflict();
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        // DELETE api/<EntityController>/5
        [HttpDelete("{id}"), Authorize]
        public IActionResult Delete(int id)
        {
            _logger.LogTrace(1, "tblEntity - Delete is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            using (tblEntityDac dac = new tblEntityDac(connStr, _logger))
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
