namespace Hangman.Services.Data.Contracts
{
    using Hangman.Data.Models;
    using Hangman.Services.Data.Contracts;

    public interface IWordsService : IBaseService<Word, int>
    {
        string GetRandomFromCategory(int categoryId);
    }
}
