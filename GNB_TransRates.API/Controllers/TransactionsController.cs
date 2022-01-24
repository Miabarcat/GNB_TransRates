using GNB_TransRates.DL.Models;
using GNB_TransRates.DL.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GNB_TransRates.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : Controller
    {
        private readonly ITransactionsService _service;
        private readonly ILogger _logger;

        public TransactionsController(ITransactionsService service, ILoggerFactory loggerFactory)
        {
            this._service = service;
            _logger = loggerFactory.CreateLogger("TransactionsConLog");
        }

        [HttpGet("list")]
        public async Task<ActionResult> Get()
        {
            try
            {
                var ret = await _service.GetAsync();

                return Ok(JsonConvert.SerializeObject(ret, Formatting.Indented));
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception: {ex} ", ex.Message);

                return BadRequest(ex.Message);
            }

            
        }

        [HttpGet("listbysku")]
        public async Task<ActionResult> GetTransactionsBySku(string sku)
        {
            try { 
                var ret = await _service.GetTransactionsSkuAsync(sku);

                return Ok(JsonConvert.SerializeObject(ret, Formatting.Indented));
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(ArgumentNullException))
                {
                    _logger.LogError("Exception: {ex} ", ex.Message);

                    return NotFound(ex.Message);
                }
                _logger.LogError("Exception: {ex} ", ex.Message);

                return BadRequest(ex.Message);
    }
}
    }
}

