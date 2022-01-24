using AutoMapper;
using GNB_TransRates.DAL.Models;
using GNB_TransRates.DL.Infrastructure;
using GNB_TransRates.DL.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace GNB_TransRates.DL.Services
{
    public class TransactionsService : ITransactionsService
    {
        private readonly IBaseService<Transactions> _transactionsService;
        private readonly IBaseService<Rates> _ratesService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IErrorHandler _errorHandler;        

        public TransactionsService(IBaseService<Transactions> transactionsService, IBaseService<Rates> ratesService, IMapper mapper, ILoggerFactory loggerFactory, IErrorHandler errorHandler)
        {
            _transactionsService = transactionsService;
            _ratesService = ratesService;
            _mapper = mapper;
            _errorHandler = errorHandler;
            _logger = loggerFactory.CreateLogger("TransactionsLog");
        }

        #region public
        public async Task<IEnumerable<TransactionsResponseModel>> GetAsync()
        {
            await SetTransactions();

            var result = await _transactionsService.GetAsync();
            var ret = result.Select(t => _mapper.Map<Transactions, TransactionsResponseModel>(t));

            _logger.LogInformation("GetAsyncTransactions - {DateTimeNow}", DateTime.Now);

            return ret;
        }

        public async Task<TransactionsBySkuResponseModel> GetTransactionsSkuAsync(string sku)
        {                         
            var lsTrans = _transactionsService.Where(w => w.Sku == sku).ToList();

            var res = await TransactionsConversion(lsTrans);

            if (!res.Any()) throw new ArgumentNullException(string.Format(_errorHandler.GetMessage(ErrorMessagesEnum.NotFound), "Not data found from " + sku + " sku"));

            decimal total = Math.Round(res.Sum(s => s.amount), 2, MidpointRounding.ToEven);

            var ret = new TransactionsBySkuResponseModel()
            {
                Total = total,
                Transactions = res.Select(t => _mapper.Map<Transactions, TransactionsResponseModel>(t)).ToList()
            };

            _logger.LogInformation("GetBySku {sku} - {DateTimeNow}", sku, DateTime.Now);

            return ret;
        }

        public async Task<TransactionsResponseModel> GetById(int id)
        {
            var ret = _mapper.Map<Transactions, TransactionsResponseModel>(await _transactionsService.GetById(id));

            _logger.LogInformation("GetById {id} - {DateTimeNow}", id, DateTime.Now);

            return ret;
        }

        public IEnumerable<TransactionsResponseModel> Where(Expression<Func<Transactions, bool>> exp)
        {
            var whereResult = _transactionsService.Where(exp).ToList();
            return _mapper.Map<List<Transactions>, List<TransactionsResponseModel>>(whereResult).AsEnumerable();
        }

        public void AddOrUpdate(TransactionsResponseModel entry)
        {
            _transactionsService.AddOrUpdate(_mapper.Map<TransactionsResponseModel, Transactions>(entry));

            _logger.LogInformation("AddOrUpdate Sku: {Sku}, amount: {amount}, Currency: {Currency} - {DateTime}", entry.Sku, entry.amount, entry.Currency, DateTime.Now);
        }

        public void Remove(int id)
        {
            _transactionsService.Remove(id);
        }
        #endregion

        #region Private
        private async Task<List<Transactions>> TransactionsConversion(List<Transactions> lstTran)
        {
            List<Transactions> lTrans = new();
            lTrans.AddRange(lstTran.Where(w => w.Currency == "EUR").ToList());

            var lRates = await _ratesService.GetAsync();

            foreach (var tr in lstTran.Where(w => w.Currency != "EUR").ToList())
            {
                decimal? amnt = 0;

                amnt = tr.amount * (lRates.FirstOrDefault(f => f.FromCurr == tr.Currency && f.ToCurr == "EUR")?.Amount);

                if (amnt.HasValue)
                {
                    lTrans.Add(new Transactions()
                    {
                        Sku = tr.Sku,
                        Currency = "EUR",
                        amount = Math.Round(amnt.Value, 2, MidpointRounding.ToEven)
                    });
                }
            }

            return lTrans;
        }

        private async Task SetTransactions()
        {

            using var client = new HttpClient();

            var jsonString = await client.GetStringAsync("http://quiet-stone-2094.herokuapp.com/transactions.json");

            var newTransactions = JsonConvert.DeserializeObject<List<Transactions>>(jsonString);

            if (newTransactions != null && newTransactions.Any())
            {
                var oldTransactions = await _transactionsService.GetAsync();
                                
                _transactionsService.RemoveRange(oldTransactions);
                                
                _transactionsService.InsertRange(newTransactions);                
            }

        }
        #endregion
    }
}