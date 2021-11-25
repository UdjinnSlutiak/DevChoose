using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DevChoose.Domain.Repositories
{
    public interface IRepository<T> where T : class
    {
        public Task<IEnumerable<T>> GetAsync(int offset, int count);

        public Task<T> GetAsync(int id);

        public Task<T> CreateAsync(T entity);

        public Task UpdateAsync(T entity);

        public Task DeleteAsync(int id);

        public Task<IEnumerable<T>> FilterAsync(Expression<Func<T, bool>> predicate);

        public Task<T> FindAsync(Expression<Func<T, bool>> predicate);
    }
}
