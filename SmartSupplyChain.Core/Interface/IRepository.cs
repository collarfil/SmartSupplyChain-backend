using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SmartSupplyChain.Core.Interface
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(
            int id, 
            params Expression<Func<T, object>>[] includes
        );

        Task<IEnumerable<T>> GetAllAsync(
            params Expression<Func<T, object>>[] includes
        );

        Task<IEnumerable<T>> FindAsync(
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includes
        );

        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}