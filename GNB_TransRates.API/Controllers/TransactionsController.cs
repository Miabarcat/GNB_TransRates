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

        public TransactionsController(ITransactionsService service)
        {
            this._service = service;
        }

        [HttpGet("list")]
        public async Task<IEnumerable<TransactionsResponseModel>> Get()
        {
            return await _service.GetAsync();
        }

        [HttpGet("listbysku")]
        public async Task<TransactionsBySkuResponseModel> GetTransactionsBySku(string sku)
        {
            return  await _service.GetTransactionsSkuAsync(sku);
        }
    }
}

