using AutoMapper;
using GNB_TransRates.DAL.Models;
using GNB_TransRates.DL.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace GNB_TransRates.DL.Services
{
    public class RatesService : IRatesService
    {
        private readonly IBaseService<Rates> _ratesService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public RatesService(IBaseService<Rates> ratesService, IMapper mapper, ILoggerFactory loggerFactory)
        {
            this._ratesService = ratesService;
            this._mapper = mapper;
            this._logger = loggerFactory.CreateLogger("RatesLog");
        }

        #region public
        public async Task<IEnumerable<RatesResponseModel>> GetAsync()
        {
            await SetRates();

            var result = await _ratesService.GetAsync();
            var ret =  result.Select(t => _mapper.Map<Rates, RatesResponseModel>(t));

            _logger.LogInformation("GetAsyncRates - {DateTimeNow}", DateTime.Now);

            return ret;
        }

        public async Task<RatesResponseModel> GetById(int id)
        {
            var ret = _mapper.Map<Rates, RatesResponseModel>(await _ratesService.GetById(id));

            _logger.LogInformation("GetById {Id} - {DateTimeNow}", id, DateTime.Now);

            return ret;
        }

        public IEnumerable<RatesResponseModel> Where(Expression<Func<Rates, bool>> exp)
        {
            var whereResult = _ratesService.Where(exp).ToList();
            return _mapper.Map<List<Rates>, List<RatesResponseModel>>(whereResult).AsEnumerable();
        }

        public void AddOrUpdate(RatesResponseModel entry)
        {
            _ratesService.AddOrUpdate(_mapper.Map<RatesResponseModel, Rates>(entry));

            _logger.LogInformation("AddOrUpdate from: {From}, to: {To}, rate: {Rate} - {DateTime}", entry.From, entry.To, entry.Rate,DateTime.Now);
        }

        public void Remove(int id)
        {
            _ratesService.Remove(id);
        }
        #endregion

        #region private
        private async Task SetRates()
        {
            using HttpClient? client = new();

            var jsonString = await client.GetStringAsync("http://quiet-stone-2094.herokuapp.com/rates.json");

            var newRates = JsonConvert.DeserializeObject<List<RatesResponseModel>>(jsonString);

            if (newRates != null && newRates.Any())
            {
                var oldRates = await _ratesService.GetAsync();

                _ratesService.RemoveRange(oldRates);

                ConvEur(ref newRates);

                _ratesService.InsertRange(_mapper.Map<List<RatesResponseModel>, List<Rates>>(newRates));
            }
            else 
            { 
                
            }
        }

        private void ConvEur(ref List<RatesResponseModel> newRates)
        {
            List<RatesResponseModel> lstRatsAux = new();
            foreach (var lr in newRates.Where(w => w.From != "EUR" && w.To != "EUR"))
            {
                decimal? amnt = 0;

                amnt = lr.Rate * (newRates.FirstOrDefault(f => f.From == lr.To && f.To == "EUR")?.Rate);

                if (amnt.HasValue)
                {
                    lstRatsAux.Add(new RatesResponseModel()
                    {
                        From = lr.From,
                        To = "EUR",
                        Rate = Math.Round(amnt.Value, 2, MidpointRounding.ToEven)
                    });

                    lstRatsAux.Add(new RatesResponseModel()
                    {
                        From = "EUR",
                        To = lr.From,
                        Rate = Math.Round((1 / amnt.Value), 2, MidpointRounding.ToEven)
                    });
                }
            }
            newRates.AddRange(lstRatsAux);
        }
        #endregion
    }
}