using GNB_TransRates.DAL.Models;
using GNB_TransRates.DL.Repositories;
using System.Linq.Expressions;

namespace GNB_TransRates.DL.Services
{
    public class BaseService<T> : IBaseService<T> where T : BaseModel
    {
        private readonly IBaseRepository<T> _repository;

        public BaseService(IBaseRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<T>> GetAsync()
        {
            return await _repository.GetAll();
        }

        public async Task<T> GetById(int id)
        {
            return await _repository.GetById(id);
        }

        public IEnumerable<T> Where(Expression<Func<T, bool>> exp)
        {
            return _repository.Where(exp);
        }

        public void AddOrUpdate(T entry)
        {
            var targetRecord = _repository.GetById(entry.Id).Result;
            var exists = targetRecord != null;

            if (exists)
            {
                _repository.Update(entry);
                return;
            }

            _repository.Insert(entry);
        }

        public void Add(T entry)
        {
            _repository.Insert(entry);
        }

        public void Remove(int id)
        {
            var label = _repository.GetById(id).Result;
            _repository.Delete(label);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _repository.DeleteRange(entities);
        }

        public void InsertRange(IEnumerable<T> entities)
        {
            _repository.InsertRange(entities);
        }
    }
}
