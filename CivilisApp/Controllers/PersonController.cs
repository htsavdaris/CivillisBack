using Microsoft.AspNetCore.Mvc;
using CivilisDB;
using Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using CivilisDB.Models;
using Npgsql;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;
using CivilisApp.Models;
using System.Security.Claims;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CivilisApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<PersonController> _logger;
        private DbConnectionStringResolver _dbConnectionStringResolver;
       // private readonly IHttpContextAccessor _httpContextAccessor;

        public PersonController(IConfiguration config, ILogger<PersonController> logger)
        {
            //_httpContextAccessor = httpContextAccessor;
            this.configuration = config;
            _logger = logger;
            _logger.LogTrace(1, "Serilog injected into Person Controller");
            _dbConnectionStringResolver = new DbConnectionStringResolver(configuration);
            //Console.WriteLine("User Id: " +
            //_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        // GET: api/<PersonController>/id
        [HttpGet("{id}"), Authorize]
        public ActionResult<tblPerson> Get(long id)
        {
            _logger.LogTrace(1, "tblPerson - Get one is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            tblPerson obj;
            using (tblPersonDac dac = new tblPersonDac(connStr, _logger))
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
                    _logger.LogError(1, "Person Controller - NpgSQL Error:" + ex.Message);
                    return BadRequest(ex.Message);
                }

            }
        }

        // GET api/<PersonController>
        [HttpGet, AllowAnonymous]
        public ActionResult<List<tblPerson>> Get()
        {
            _logger.LogTrace(1, "tblPerson - Get All is called");
            string userId = User.Identity.Name;
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            List<tblPerson> personList;
            using (tblPersonDac dac = new tblPersonDac(connStr, _logger))
            {
                try
                {
                    personList = dac.GetAllSortedDesc();
                    _logger.LogTrace(1, "DAC GetAll Called");
                    if (personList == null)
                    {
                        _logger.LogTrace(1, "Not Found");
                        return NotFound();
                    }
                    _logger.LogTrace(1, "No of Items:" + personList.Count().ToString());
                    return Ok(personList);
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    _logger.LogError(1, "NpgsqlException Code:" + ex.ErrorCode + " Message :" + ex.Message);
                    return BadRequest(ex.Message);
                }
            }
        }

        // This is the basic search from the Person Dialog and the table
        [HttpPost("search1"), Authorize]
        public ActionResult<List<tblPerson>> GetSearch1([FromBody] SearchPerson1 obj)
        {
            _logger.LogTrace(1, "tblPerson - GetSearch1 is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            List<tblPerson> objList;
            var connec = new NpgsqlConnection(connStr);

            var db = new QueryFactory(connec, new PostgresCompiler());
            var compiler = new PostgresCompiler();
            var query = db.Query("tblperson");
            if (obj.fldpersonfirstname != "")
            {
                query.WhereLike("fldpersonfirstname", obj.fldpersonfirstname + "%");
            }
            if (obj.fldpersonlastname != "")
            {
                query.WhereLike("fldpersonlastname", obj.fldpersonlastname + "%");
            }
            if (obj.fldpersonmobile != "")
            {
                query.Where("fldpersonmobile", obj.fldpersonmobile);
            }
            if (obj.fldcity != "")
            {
                query.Where("fldcity", obj.fldcity);
            }
            if (obj.fldattributeid != 0)
            {
                query.Where("fldattributeid", obj.fldattributeid);
            }
            query.Limit(10);
            SqlResult result = compiler.Compile(query);

            try
            {

                objList = query.Get<tblPerson>().ToList();
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

        //This is the search with the Person Advanced Search
        [HttpPost("search2"), Authorize]
        public ActionResult<List<SearchPersonResults>> GetSearch2([FromBody] SearchPerson1 obj)
        {
            _logger.LogTrace(1, "tblPerson - GetSearch2 is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            List<SearchPersonResults> objList;
            var connec = new NpgsqlConnection(connStr);

            var db = new QueryFactory(connec, new PostgresCompiler());
            var compiler = new PostgresCompiler();
            Query query;
           
            if (obj.fldattributeid != 0)
                query = db.Query("vwpersonattributeall");
            else
            {
                query = db.Query("tblperson");
            }
            query.Select("fldpersonid", "fldpersonlastname", "fldpersonfirstname", "fldpersonmobile", "fldpersonemail", "fldcity");

            if (obj.fldpersonfirstname != "")
            {
                query.WhereLike("fldpersonfirstname", obj.fldpersonfirstname );
            }
            if (obj.fldpersonlastname != "")
            {
                query.WhereLike("fldpersonlastname", obj.fldpersonlastname );
            }
            if (obj.fldpersonmobile != "")
            {
                query.Where("fldpersonmobile", obj.fldpersonmobile);
            }
            if (obj.fldcity != "")
            {
                query.Where("fldcity", obj.fldcity);
            }
            if (obj.fldattributeid != 0)
            {
                query.Where("fldattributeid", obj.fldattributeid);
            }
            query.Limit(100);
            
            SqlResult result = compiler.Compile(query);

            try
            {

                objList = query.Get<SearchPersonResults>().ToList();
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

        [HttpPost("search"), Authorize]
        public ActionResult<List<tblPerson>> GetParameterized()
        {
            _logger.LogTrace(1, "tblPerson - GetParameterized All is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            List<tblPerson> personList;
            using (tblPersonDac dac = new tblPersonDac(connStr, _logger))
            {
                try
                {
                    personList = dac.GetAllSortedDesc();
                    _logger.LogTrace(1, "DAC GetAll Called");
                    if (personList == null)
                    {
                        _logger.LogTrace(1, "Not Found");
                        return NotFound();
                    }
                    _logger.LogTrace(1, "No of Items:" + personList.Count().ToString());
                    return Ok(personList);
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    _logger.LogError(1, "NpgsqlException Code:" + ex.ErrorCode + " Message :" + ex.Message);
                    return BadRequest(ex.Message);
                }
            }
        }

        // POST api/<PersonController>
        [HttpPost, Authorize]
        public IActionResult Post([FromBody] tblPerson obj)
        {
            _logger.LogTrace(1, "tblPerson - Post is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            long userId = Convert.ToInt64(User.Identity.Name);
            using (tblPersonDac dac = new tblPersonDac(connStr, _logger))
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
                        //obj.fldpersonid = id.Value;
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

        // PUT api/<PersonController>/5
        [HttpPut("{id}"), Authorize]
        public IActionResult Put(int id, [FromBody] tblPerson obj)
        {
            _logger.LogTrace(1, "tblPerson - Put All is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            long userId = Convert.ToInt64(User.Identity.Name);
            using (tblPersonDac dac = new tblPersonDac(connStr, _logger))
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

        // DELETE api/<PersonController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
