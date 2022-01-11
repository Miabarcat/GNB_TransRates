using GNB_TransRates.DAL.Models;
using System.Linq.Expressions;

namespace GNB_TransRates.DL.Repositories
{
    public interface IBaseRepository<T> where T : BaseModel
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        IEnumerable<T> Where(Expression<Func<T, bool>> exp);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        void InsertRange(IEnumerable<T> entities);
    }
}
