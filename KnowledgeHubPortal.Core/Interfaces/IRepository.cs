using System.Collections.Generic;
using System.Linq.Expressions;

namespace KnowledgeHubPortal.Core.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        IQueryable<T> GetAllAsQueryable();
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
    }
}