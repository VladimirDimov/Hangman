namespace Hangman.Services.Data
{
    using System;
    using System.Linq;
    using Contracts;
    using Hangman.Data.Common;
    using Hangman.Data.Models;

    public class UsersService : IUsersService
    {
        public UsersService()
        {
        }

        public IQueryable<User> All()
        {
            throw new NotImplementedException();
        }
    }
}
