namespace Hangman.Data.Common
{
    using System.Linq;

    using Hangman.Data.Common.Models;

    public interface IDbRepository<T>
        where T : BaseModel<int>
    {
    }

    public interface IDbRepository<T, in TKey> : IDbGenericRepository<T, TKey>
        where T : BaseModel<TKey>
    {
        IQueryable<T> AllWithDeleted();

        void HardDelete(T entity);
    }
}
