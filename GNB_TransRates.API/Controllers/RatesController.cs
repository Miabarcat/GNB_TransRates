using GNB_TransRates.DL.Infrastructure;
using GNB_TransRates.DL.Models;
using GNB_TransRates.DL.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GNB_TransRates.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatesController : Controller
    {
        private readonly IRatesService _service;
        private readonly ILogger _logger;

        public RatesController(IRatesService service, ILoggerFactory loggerFactory)
        {
            _service = service;
            _logger = loggerFactory.CreateLogger("RatesConLog");

        }

        [HttpGet("list")]
        public async Task<ActionResult> GetRates()
        {
            try
            {
                var ret = await _service.GetAsync();

                return Ok(JsonConvert.SerializeObject(ret,Formatting.Indented));
            }
            catch (Exception ex) 
            {
                _logger.LogError("Exception: {ex} ", ex.Message);

                return BadRequest(ex.Message);
            }
            
        }
    }
}