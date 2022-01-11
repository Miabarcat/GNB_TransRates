using GNB_TransRates.DL.Infrastructure;
using GNB_TransRates.DL.Models;
using GNB_TransRates.DL.Services;
using Microsoft.AspNetCore.Mvc;

namespace GNB_TransRates.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatesController : Controller
    {
        private readonly IRatesService _service;
        private readonly IErrorHandler _errorHandler;

        public RatesController(IRatesService service, IErrorHandler errorHandler)
        {
            _service = service;
            _errorHandler = errorHandler;
                
        }

        [HttpGet("list")]
        public async Task<IEnumerable<RatesResponseModel>> GetRates()
        {
            return await _service.GetAsync();            
        }
    }
}