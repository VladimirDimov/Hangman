namespace Hangman.Data.Models
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Hangman.Data.Models;

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class User : IdentityUser
    {
        //public virtual UserStatistics UserStatistics { get; set; }

        public int NumberOfGuesses { get; set; }

        public int NumberOfSuccessfulGuesses { get; set; }

        public int NumberOfUnsuccessfulGuesses { get; set; }

        public int NumberOfGames { get; set; }

        public int NumberOfGamesWon { get; set; }

        public int NumberOfGamesLost { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            // Add custom user claims here
            return userIdentity;
        }
    }
}
