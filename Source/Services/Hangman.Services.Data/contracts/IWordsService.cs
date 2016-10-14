namespace Hangman.Services.Data.Contracts
{
    using Hangman.Data.Models;
    using Hangman.Services.Data.Contracts;

    public interface IWordsService : IBaseService<Word, int>
    {
        Word GetRandomFromCategory(int categoryId);
    }
}
