namespace Hangman.Services.Data
{
    using System.Linq;
    using Hangman.Data.Models;
    
    public interface IUsersService
    {
        IQueryable<User> All();
    }
}