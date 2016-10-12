namespace MvcTemplate.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using Hangman.Data.Models;

    public class UserStatistics
    {
        public int NumberOfGuesses { get; set; }

        public int NumberOfSuccessfulGuesses { get; set; }

        public int NumberOfUnsuccessfulGuesses { get; set; }

        public int NumberOfGames { get; set; }

        public int NumberOfGamesWon { get; set; }

        public int NumberOfGamesLost { get; set; }

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }
    }
}
