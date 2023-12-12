using System.Linq.Expressions;

namespace Harmony.Common.Contracts
{
    public interface IBase
    {
    }

    public interface IBase<T> : IBase
        where T : class, new()
    {
        Task<T> Add(T entity);

        Task Remove(T entity);

        Task Remove(long id);

        Task<T> Update(T entity);

        IQueryable<T> GetAll(string sortExpression = null);

        IPagedList<T> GetPaged(int startRowIndex, int pageSize, string sortExpression = null);

        IQueryable<T> GetAll(Func<IQueryable<T>, IQueryable<T>> transform, Expression<Func<T, bool>> filter = null, string sortExpression = null);

        IQueryable GetAll<TResult>(Func<IQueryable<T>, IQueryable<TResult>> transform, Expression<Func<T, bool>> filter = null, string sortExpression = null);

        int GetCount<TResult>(Func<IQueryable<T>, IQueryable<TResult>> transform, Expression<Func<T, bool>> filter = null);

        IPagedList<T> GetPaged(Func<IQueryable<T>, IQueryable<T>> transform, Expression<Func<T, bool>> filter = null, int startRowIndex = -1, int pageSize = -1, string sortExpression = null);

        IPagedList<TResult> GetPaged<TResult>(Func<IQueryable<T>, IQueryable<TResult>> transform, Expression<Func<T, bool>> filter = null, int startRowIndex = -1, int pageSize = -1, string sortExpression = null);

        Task<T> Get(object id);

        TResult Get<TResult>(Func<IQueryable<T>, IQueryable<TResult>> transform, Expression<Func<T, bool>> filter = null, string sortExpression = null);

        bool Exists(long id);

        bool Exists(Func<IQueryable<T>, IQueryable<T>> query, Expression<Func<T, bool>> filter = null);
    }
}
