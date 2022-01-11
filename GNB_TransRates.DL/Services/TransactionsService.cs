using AutoMapper;
using GNB_TransRates.DAL.Models;
using GNB_TransRates.DL.Models;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace GNB_TransRates.DL.Services
{
    public class TransactionsService : ITransactionsService
    {
        private readonly IBaseService<Transactions> _transactionsService;
        private readonly IBaseService<Rates> _ratesService;
        private readonly IMapper _mapper;

        public TransactionsService(IBaseService<Transactions> transactionsService, IBaseService<Rates> ratesService, IMapper mapper)
        {
            this._transactionsService = transactionsService;
            this._ratesService = ratesService;
            this._mapper = mapper;
        }

        #region public
        public async Task<IEnumerable<TransactionsResponseModel>> GetAsync()
        {
            await SetTransactions();

            var result = await _transactionsService.GetAsync();
            return result.Select(t => _mapper.Map<Transactions, TransactionsResponseModel>(t));
        }

        public async Task<TransactionsBySkuResponseModel> GetTransactionsSkuAsync(string sku)
        {                         
            var lsTrans = _transactionsService.Where(w => w.Sku == sku).ToList();

            var res = await TransactionsConversion(lsTrans);

            decimal total = Math.Round(res.Sum(s => s.amount), 2, MidpointRounding.ToEven);

            return new TransactionsBySkuResponseModel()
            {
                Total = total,
                Transactions = res.Select(t => _mapper.Map<Transactions, TransactionsResponseModel>(t)).ToList()
            };
        }

        public async Task<TransactionsResponseModel> GetById(int id)
        {
            return _mapper.Map<Transactions, TransactionsResponseModel>(await _transactionsService.GetById(id));
        }

        public IEnumerable<TransactionsResponseModel> Where(Expression<Func<Transactions, bool>> exp)
        {
            var whereResult = _transactionsService.Where(exp).ToList();
            return _mapper.Map<List<Transactions>, List<TransactionsResponseModel>>(whereResult).AsEnumerable();
        }

        public void AddOrUpdate(TransactionsResponseModel entry)
        {
            _transactionsService.AddOrUpdate(_mapper.Map<TransactionsResponseModel, Transactions>(entry));
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