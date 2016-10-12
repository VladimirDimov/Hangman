namespace MvcTemplate.Services.Data.Contracts
{
    using Hangman.Data.Common;
    using Hangman.Data.Common.Models;

    public interface IBaseService<T, TKey>
        where T : BaseModel<TKey>
    {

    }
}