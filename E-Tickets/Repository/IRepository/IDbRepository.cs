using System.Linq.Expressions;

namespace E_Tickets.Repository.IRepository
{
    public interface IDbRepository<T> where T : class
    {
        List<T> GetAll(IEnumerable<Expression<Func<T, object>>>? includeExpressions = null);
        T? GetById(int id);
        void Create(T entity);
        void Update(T entity);
        void Delete(int id);
        void Commit();
    }
}
