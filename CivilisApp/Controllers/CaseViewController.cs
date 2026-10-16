using Microsoft.AspNetCore.Mvc;
using Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using CivilisDB;
using CivilisDB.Models;
using CivilisApp.Models;
using Npgsql;
using SqlKata.Compilers;
using SqlKata.Execution;
using SqlKata;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CivilisApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaseViewController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<CaseViewController> _logger;
        private DbConnectionStringResolver _dbConnectionStringResolver;


        public CaseViewController(IConfiguration config, ILogger<CaseViewController> logger)
        {
            this.configuration = config;
            _logger = logger;
            _logger.LogTrace(1, "Serilog injected into CaseView Controller");
            _dbConnectionStringResolver = new DbConnectionStringResolver(configuration);
        }

        [HttpGet, Authorize]
        public ActionResult<List<vwCase>> Get()
        {
            _logger.LogTrace(1, "vwCase - Get All is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            List<vwCase> objList;
            using (vwCaseDac dac = new vwCaseDac(connStr, _logger))
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


        [HttpGet("{id}"), Authorize]
        public ActionResult<List<vwCase>> Get(long id)
        {
            _logger.LogTrace(1, "vwCase - Get All is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            vwCase obj;
            using (vwCaseDac dac = new vwCaseDac(connStr, _logger))
            {
                try
                {
                    obj = dac.Get(id);
                    _logger.LogTrace(1, "DAC Get Called");
                    if (obj == null)
                    {
                        _logger.LogTrace(1, "Not Found");
                        return NotFound();
                    }
                    
                    return Ok(obj);
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    _logger.LogError(1, "NpgsqlException Code:" + ex.ErrorCode + " Message :" + ex.Message);
                    return BadRequest(ex.Message);
                }
            }
        }


        [HttpGet("status/{id}"), Authorize]
        public ActionResult<List<vwCase>> GetByStatusandDate(long id)
        {
            _logger.LogTrace(1, "vwCase - GetByStatusandDate one is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            List<vwCase> obj;
            using (vwCaseDac dac = new vwCaseDac(connStr, _logger))
            {
                try
                {
                    obj = dac.GetByStatusByDate(id);
                    if (obj != null)
                        return Ok(obj);
                    else
                        return NotFound();
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    _logger.LogError(1, "vwCase Controller - NpgSQL Error:" + ex.Message);
                    return BadRequest(ex.Message);
                }

            }
        }


        [HttpGet("manager/{id}"), Authorize]
        public ActionResult<List<vwCase>> GetByManager(long managerid)
        {
            _logger.LogTrace(1, "vwCase - GetByManger one is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            List<vwCase>? obj;
            using (vwCaseDac dac = new vwCaseDac(connStr, _logger))
            {
                try
                {
                    obj = dac.GetbyManager(managerid);
                    if (obj != null)
                        return Ok(obj);
                    else
                        return NotFound();
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    _logger.LogError(1, "vwCase Controller - NpgSQL Error:" + ex.Message);
                    return BadRequest(ex.Message);
                }

            }
        }

        [HttpPost("search2"), Authorize]
        public ActionResult<List<vwCase>> GetSearch2([FromBody] SearchCase1 obj)
        {
            _logger.LogTrace(1, "vwCase - GetSearch2 is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            //Get the user ID
            long userId = Int32.Parse(User.Identity.Name);


            //Find the Roles
            vwUserRoleDac userRoleDac = new vwUserRoleDac(connStr, _logger);
            var allRoles = new List<vwUserRole>();
            allRoles = userRoleDac.GetAllForUserId(userId);

            var adminRec = allRoles.Find(x => x.fldroledesc == "ADMIN");
            bool isAdmin = (adminRec != null);

            
            //Find the Person
            tblUserDac tblUserDac = new tblUserDac(connStr, _logger);
            tblUser user = tblUserDac.Get(userId);
            long personid = user.fldpersonid ;

            List<vwCase> objList;
            var connec = new NpgsqlConnection(connStr);

            var db = new QueryFactory(connec, new PostgresCompiler());
            var compiler = new PostgresCompiler();
            Query query;
            query = db.Query("vwcase");
            //Mine is 0
            if (obj.pAllOrMine == 0)
            {
                query.Where(q=> q.Where("fldmanagerid", personid).OrWhere("fldreceptionid", personid));
            }  //All is 1
            else if (obj.pAllOrMine == 1)
            {
                if (!isAdmin) {
                    query.Where(q => q.Where("fldconfidential", false).OrWhere(s=>s.Where("fldconfidential", true).Where(t=>t.Where("fldmanagerid", personid).OrWhere("fldreceptionid", personid))));
                }
            }
            if ((obj.pOpen == true) && (obj.pClosed == true))
            {
                //query.Where("fldcasestatusid", 0);
                //query.OrWhere("fldcasestatusid", 1);
                //query.OrWhere("fldcasestatusid", 2);
            }
            else if ((obj.pOpen == true) && (obj.pClosed == false))
            {
                query.Where("fldcasestatusid", 0);
                
            }
            else if((obj.pOpen == false) && (obj.pClosed == true))
            {
                query.Where("fldcasestatusid", 1);
                query.OrWhere("fldcasestatusid", 2);
            }
            else if((obj.pOpen == false) && (obj.pClosed == false))
            {
                query.Where("fldcasestatusid", 3);
            }
            
            try
            {

                objList = query.Get<vwCase>().ToList();
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
