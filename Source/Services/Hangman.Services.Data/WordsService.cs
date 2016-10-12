namespace Hangman.Services.Data
{
    using System;
    using System.Linq;
    using Contracts;
    using Hangman.Data.Common;
    using Hangman.Data.Models;

    public class WordsService : BaseService<Word>, IWordsService
    {
        public WordsService(IDbRepository<Word, int> wordsRepository)
            : base(wordsRepository)
        {
        }

        public string GetRandomFromCategory(int categoryId)
        {
            var wordsFromCategory = this.All()
                .Where(w => w.CategoryId == categoryId)
                .Select(w => w.Content)
                .ToList();

            var numberOfWordsInCategory = wordsFromCategory.Count;

            if (numberOfWordsInCategory == 0)
            {
                return null;
            }

            var rnd = new Random();
            var word = wordsFromCategory[rnd.Next(0, numberOfWordsInCategory)];

            return word;
        }
    }
}
