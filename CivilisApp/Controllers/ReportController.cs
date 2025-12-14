using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClosedXML.Excel;
using CivilisDB.Models;
using Npgsql;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Web;
using CivilisApp.Utils;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;
using Microsoft.AspNetCore.SignalR;
using CivilisApp.Hubs;
using Microsoft.AspNetCore.Cors;
using DocumentFormat.OpenXml.Presentation;
using Azure;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CivilisApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<ReportController> _logger;
        private readonly DbConnectionStringResolver _dbConnectionStringResolver;
        private readonly IHubContext<CivilisSignalrHub, ICivilisSignalrClient> _hubContext;

        public ReportController(IConfiguration config, ILogger<ReportController> logger, IHubContext<CivilisSignalrHub, ICivilisSignalrClient> hubContext)
        {
            this.configuration = config;
            _hubContext = hubContext;
            _logger = logger;
            _logger.LogTrace(1, "Serilog injected into Report Controller");
            _dbConnectionStringResolver = new DbConnectionStringResolver(configuration);
        }


        [HttpGet, Authorize]
        public async Task<ActionResult<List<tblReport>>> GetAsync()
        {
            //QueueManager qm = new QueueManager();
            //await qm.writeToQueueAsync("Hello");

            _logger.LogTrace(1, "tblReport - Get All is called");
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            List<tblReport> objList;
            using (tblReportDac dac = new tblReportDac(connStr, _logger))
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


        // GET: api/<ReportController>
        [HttpGet("personReport"), Authorize]
        public async Task<IActionResult> RunPersonReport()
        {
            var tempFolder = Directory.GetCurrentDirectory();// System.IO.Path.GetTempPath();
            _logger.LogInformation(1, "Temp Folder :" + tempFolder);
            int curRow = 2;
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            List<tblPerson> personList;
            string nowstr = DateTime.Now.ToBinary().ToString();
            string outputFile = tempFolder + @"/Reports/reportPersons" + nowstr + ".xlsx";
            const string templateFile = @"./Templates/PersonsTemplate.xlsx";
            using var wbook = new XLWorkbook(templateFile);
            var ws1 = wbook.Worksheet(1);

            tblReport obj = new tblReport();
            tblReportDac dacr = new tblReportDac(connStr, _logger);

            using (tblPersonDac dac = new tblPersonDac(connStr, _logger))
            {
                try
                {
                    personList = dac.GetAllSortedDesc();
                    _logger.LogTrace(1, "DAC GetAll Called");
                    if (personList == null)
                    {
                        _logger.LogTrace(1, "Not Found");
                    }
                    else
                    {
                        foreach (var person in personList)
                        {
                            ws1.Cell(curRow, 1).Value = person.fldpersonid;
                            ws1.Cell(curRow, 2).Value = person.fldpersonprefix;
                            ws1.Cell(curRow, 3).Value = person.fldpersonlastname;
                            ws1.Cell(curRow, 4).Value = person.fldpersonfirstname;
                            ws1.Cell(curRow, 5).Value = person.fldpersonmobile;
                            ws1.Cell(curRow, 6).Value = person.fldpersonemail;
                            ws1.Cell(curRow, 7).Value = person.fldpersonstreet1;
                            ws1.Cell(curRow, 8).Value = person.fldpersonnumber1;
                            ws1.Cell(curRow, 9).Value = person.fldpersontk1;
                            ws1.Cell(curRow, 10).Value = person.fldcity;
                            ws1.Cell(curRow, 11).Value = person.fldprofession;
                            curRow++;
                        }
                        obj.fldreportname = "Report Φυσικών Προσώπων";
                        obj.fldreportdate = DateTime.Now;
                        obj.fldfilename = outputFile;
                        obj.fldcreatedat = DateTime.Now;
                        obj.fldupdatedat = DateTime.Now;
                        var res = dacr.Insert(obj);
                        wbook.SaveAs(outputFile);
                        if (res && obj.fldreportid > 0)
                        {
                            DeleteAllReportUptoIDMinusX(obj.fldreportid, 3);
                        }
                        await _hubContext.Clients.All.ReportCompletedNotify("Hello from Server;");
                    }
                   
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    _logger.LogError(1, "NpgsqlException Code:" + ex.ErrorCode + " Message :" + ex.Message);
                }
            }
            return Ok();

        }

        private void DeleteAllReportUptoIDMinusX(long id, long x)
        {
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            tblReportDac dacr = new tblReportDac(connStr, _logger);
            long limit = id - x;
            if (limit > 0)
            {
                var obj = dacr.GetReportsOlderThanId(limit);
                foreach (var report in obj)
                {
                    DeleteReportID(report.fldreportid);
                }    
            }
        }

        private void DeleteReportID(long id)
        {
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            
            tblReportDac dacr = new tblReportDac(connStr, _logger);
            tblReport obj = dacr.Get(id);
            if (obj != null)
            {
                System.IO.File.Delete(obj.fldfilename);
                dacr.Delete(obj.fldreportid);
            }

        }

        [HttpGet("personReportDownload/{id}"), Authorize]
        [EnableCors("MyPolicy")]
        public async Task<FileResult> DonwloadPersonReport(long id)
        {
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            tblReportDac dacr = new tblReportDac(connStr, _logger);
            const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var obj = dacr.Get(id);
            FileInfo file = new FileInfo(obj.fldfilename);
            if (file.Exists)
            {
                var bytes = System.IO.File.ReadAllBytes(file.FullName);
                HttpContext.Response.ContentType = contentType;
                HttpContext.Response.Headers.Append("Access-Control-Expose-Headers", "Content-Disposition");
                HttpContext.Response.Headers.Append("Access-Control-Allow-Origin", "*");
                HttpContext.Response.Headers.Append("Cache-Control", "no-cache");

                var fileContentResult = new FileContentResult(bytes, contentType)
                {
                    FileDownloadName = file.Name
                };
                return fileContentResult;
            }
            else
            {
                HttpContext.Response.ContentType = contentType;
                return new  FileContentResult(null, contentType);
            }

            
        }



        //-------------------------------------------------------------
        [HttpGet("entityReport"), Authorize]
        public IActionResult RunEntityReport()
        {
            var tempFolder = System.IO.Path.GetTempPath();
            _logger.LogInformation(1, "Temp Folder :" + tempFolder);
            int curRow = 2;
            string connStr = _dbConnectionStringResolver.GetNameOrConnectionString();
            List<tblEntity> entityList;
            string nowstr = DateTime.Now.ToBinary().ToString();
            string outputFile = tempFolder + @"Reports\reportEntities" + nowstr + ".xlsx";
            const string templateFile = @".\Templates\EntitiesTemplate.xlsx";
            using var wbook = new XLWorkbook(templateFile);
            var ws1 = wbook.Worksheet(1);

            tblReport obj = new tblReport();
            tblReportDac dacr = new tblReportDac(connStr, _logger);

            using (tblEntityDac dac = new tblEntityDac(connStr, _logger))
            {
                try
                {
                    entityList = dac.GetAll();
                    _logger.LogTrace(1, "DAC GetAll Called");
                    if (entityList == null)
                    {
                        _logger.LogTrace(1, "Not Found");
                    }
                    else
                    {
                        foreach (var entity in entityList)
                        {
                            ws1.Cell(curRow, 1).Value = entity.fldentityid;
                            ws1.Cell(curRow, 2).Value = entity.fldentityname;
                            ws1.Cell(curRow, 3).Value = entity.fldentitytypeid;
                            ws1.Cell(curRow, 4).Value = entity.fldaddress;
                            ws1.Cell(curRow, 5).Value = entity.fldnumber;
                            ws1.Cell(curRow, 6).Value = entity.fldtk;
                            ws1.Cell(curRow, 7).Value = entity.fldcity;
                            ws1.Cell(curRow, 8).Value = entity.fldcountry;
                            ws1.Cell(curRow, 9).Value = entity.fldfixedphone;
                            ws1.Cell(curRow, 10).Value = entity.fldmobile;
                            ws1.Cell(curRow, 11).Value = entity.fldmail;
                            curRow++;
                        }
                        obj.fldreportname = "Report Νομικών Προσώπων";
                        obj.fldreportdate = DateTime.Now;
                        obj.fldfilename = outputFile;
                        obj.fldcreatedat = DateTime.Now;
                        obj.fldupdatedat = DateTime.Now;
                        var res = dacr.Insert(obj);
                        wbook.SaveAs(outputFile);
                        if (res && obj.fldreportid > 0)
                        {
                            DeleteAllReportUptoIDMinusX(obj.fldreportid, 3);
                        }

                    }

                }
                catch (Npgsql.NpgsqlException ex)
                {
                    _logger.LogError(1, "NpgsqlException Code:" + ex.ErrorCode + " Message :" + ex.Message);
                }
            }
            return Ok();

        }


        // GET api/<ReportController>/5
        [HttpGet("{id}"), Authorize]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ReportController>
        [HttpPost, Authorize]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ReportController>/5
        [HttpPut("{id}"), Authorize]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ReportController>/5
        [HttpDelete("{id}"), Authorize]
        public void Delete(int id)
        {
        }
    }
}
