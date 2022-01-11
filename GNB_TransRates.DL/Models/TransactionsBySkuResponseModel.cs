using GNB_TransRates.DAL.Models;

namespace GNB_TransRates.DL.Models
{
    public class TransactionsBySkuResponseModel
    {
        public decimal Total { get; set; }
        public List<TransactionsResponseModel>? Transactions { get; set; }
    }
}