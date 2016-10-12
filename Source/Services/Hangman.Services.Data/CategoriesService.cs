namespace MvcTemplate.Services.Data
{
    using Contracts;
    using Hangman.Data.Common;
    using Hangman.Data.Models;

    public class CategoriesService : BaseService<Category>, ICategoriesService
    {
        public CategoriesService(IDbRepository<Category, int> categoriesRepository)
            : base(categoriesRepository)
        {
        }
    }
}
