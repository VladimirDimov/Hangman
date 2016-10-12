namespace MvcTemplate.Services.Data.Contracts
{
    using System.Linq;
    using Hangman.Data.Common;
    using Hangman.Data.Common.Models;

    public interface IBaseService<T, TKey>
        where T : BaseModel<TKey>
    {
        IQueryable<T> All();
    }
}
