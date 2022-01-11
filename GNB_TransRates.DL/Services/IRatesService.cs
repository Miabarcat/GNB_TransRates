using GNB_TransRates.DAL.Models;
using GNB_TransRates.DL.Models;
using System.Linq.Expressions;

namespace GNB_TransRates.DL.Services 
{ 
    public interface IRatesService
    {
        void AddOrUpdate(RatesResponseModel entry);
        Task<IEnumerable<RatesResponseModel>> GetAsync();
        Task<RatesResponseModel> GetById(int id);
        void Remove(int id);
        IEnumerable<RatesResponseModel> Where(Expression<Func<Rates, bool>> exp);        
    }
}