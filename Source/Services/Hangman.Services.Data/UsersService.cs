namespace Hangman.Services.Data
{
    using System.Linq;
    using Contracts;
    using Hangman.Data.Common;
    using Hangman.Data.Models;

    public class UsersService : IUsersService
    {
        private IDbGenericRepository<User, string> usersRepository;

        public UsersService(IDbGenericRepository<User, string> usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public IQueryable<User> All()
        {
            return this.usersRepository.AllWithDeleted();
        }
    }
}
