using Domain.Services.Enums;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IUnitOfWork
    {
        IQueryable<T> Query<T>() where T : class;
        //IQueryable<T> Query<T, TResult>(Expression<Func<T, TResult>> include) where T : class;
        Task<int> SaveChangesAsync();
        void Add<T>(T entity) where T : class;
        void Update<T>(T entity);
        void Delete<T>(T entity);
        void SetEntityState<T>(T entity, State entityState);
    }
}