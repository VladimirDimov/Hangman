namespace MvcTemplate.Services.Data
{
    using System.Linq;
    using Contracts;
    using Hangman.Data.Common;
    using Hangman.Data.Common.Models;

    public class BaseService<T> : BaseService<T, int>
        where T : BaseModel<int>
    {
        public BaseService(IDbRepository<T, int> categoriesRepository)
            : base(categoriesRepository)
        {
        }
    }

    public class BaseService<T, TKey> : IBaseService<T, TKey>
        where T : BaseModel<TKey>
    {
        private IDbRepository<T, TKey> categoriesRepository;

        public BaseService(IDbRepository<T, TKey> categoriesRepository)
        {
            this.categoriesRepository = categoriesRepository;
        }

        public IQueryable<T> All()
        {
            return this.categoriesRepository.AllWithDeleted();
        }
    }
}
