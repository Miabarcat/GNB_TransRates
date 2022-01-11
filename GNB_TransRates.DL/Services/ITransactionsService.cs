using GNB_TransRates.DAL.Models;
using GNB_TransRates.DL.Models;
using System.Linq.Expressions;

namespace GNB_TransRates.DL.Services
{
    public interface ITransactionsService
    {
        void AddOrUpdate(TransactionsResponseModel entry);
        Task<IEnumerable<TransactionsResponseModel>> GetAsync();
        Task<TransactionsBySkuResponseModel> GetTransactionsSkuAsync(string sku);
        Task<TransactionsResponseModel> GetById(int id);
        void Remove(int id);
        IEnumerable<TransactionsResponseModel> Where(Expression<Func<Transactions, bool>> exp);
    }
}    