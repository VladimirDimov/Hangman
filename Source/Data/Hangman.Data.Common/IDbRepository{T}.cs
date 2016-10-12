namespace Hangman.Data.Common
{
    using System.Linq;

    using Hangman.Data.Common.Models;

    public interface IDbRepository<T>
        where T : BaseModel<int>
    {
    }

    public interface IDbRepository<T, in TKey>
        where T : BaseModel<TKey>
    {
        IQueryable<T> All();

        T GetById(int id);

        void Add(T entity);

        void Delete(T entity);

        void HardDelete(T entity);

        IQueryable<T> AllWithDeleted();
    }
}
