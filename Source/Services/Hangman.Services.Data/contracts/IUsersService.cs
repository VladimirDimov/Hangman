namespace Hangman.Services.Data.Contracts
{
    using System.Linq;
    using Hangman.Data.Models;

    public interface IUsersService
    {
        IQueryable<User> All();
    }
}