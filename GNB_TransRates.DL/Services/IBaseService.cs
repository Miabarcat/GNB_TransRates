using GNB_TransRates.DAL.Models;
using System.Linq.Expressions;

namespace GNB_TransRates.DL.Services
{
    public interface IBaseService<T> where T : BaseModel
    {

        Task<IEnumerable<T>> GetAsync();

        Task<T> GetById(int id);

        IEnumerable<T> Where(Expression<Func<T, bool>> exp);

        void AddOrUpdate(T entry);

        void Add(T entry);

        void Remove(int id);

        void RemoveRange(IEnumerable<T> entities);

        void InsertRange(IEnumerable<T> entities);
    }
}
