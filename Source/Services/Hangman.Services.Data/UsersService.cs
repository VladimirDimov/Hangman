namespace Hangman.Services.Data
{
    using System.Linq;

    using Hangman.Data.Common;
    using Hangman.Data.Models;

    public class UsersService : IUsersService
    {
        private IDbGenericRepository<User, string> users;

        public UsersService(IDbGenericRepository<User, string> users)
        {
            this.users = users;
        }

        public IQueryable<User> All()
        {
            return this.users.All();
        }
    }
}
