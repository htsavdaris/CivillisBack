using Microsoft.AspNetCore.Mvc;
using CivilisApp.Services;
using CivilisApp.Interfaces;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CivilisApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportPersonController : ControllerBase
    {
        readonly IBufferedFileUploadService _bufferedFileUploadService;

        public ImportPersonController(IBufferedFileUploadService bufferedFileUploadService)
        {
            _bufferedFileUploadService = bufferedFileUploadService;
        }


        // GET: api/<ImportPersonController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ImportPersonController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ImportPersonController>
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Post(IFormFile file)
        {
            string message;
            try
            {
                if (await _bufferedFileUploadService.UploadFile(file))
                {
                    message = "File Upload Successful";
                }
                else
                {
                    message = "File Upload Failed";
                }
            }
            catch (Exception ex)
            {
                message = "File Upload Failed";
            }
            return Content(message);

        }

        // PUT api/<ImportPersonController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ImportPersonController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
