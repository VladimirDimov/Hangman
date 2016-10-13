namespace Hangman.Web.ViewModels.Home
{
    using System;
    using System.Linq.Expressions;
    using Hangman.Data.Models;

    public class StatisticsViewModel
    {
        public static Expression<Func<User, StatisticsViewModel>> FromModel
        {
            get
            {
                return m => new StatisticsViewModel
                {
                    User = m.UserName,
                    NumberOfGames = m.NumberOfGames,
                    NumberOfGamesLost = m.NumberOfGamesLost,
                    NumberOfGamesWon = m.NumberOfGamesWon,
                    NumberOfGuesses = m.NumberOfGuesses,
                    NumberOfUnsuccessfulGuesses = m.NumberOfUnsuccessfulGuesses,
                    NumberOfSuccessfulGuesses = m.NumberOfGuesses - m.NumberOfUnsuccessfulGuesses,
                };
            }
        }

        public int NumberOfGames { get; private set; }

        public int NumberOfGamesLost { get; private set; }

        public int NumberOfGamesWon { get; private set; }

        public int NumberOfGuesses { get; private set; }

        public int NumberOfSuccessfulGuesses { get; private set; }

        public int NumberOfUnsuccessfulGuesses { get; private set; }

        public string User { get; set; }
    }
}
