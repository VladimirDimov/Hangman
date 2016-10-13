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
                    NumberOfGames = m.UserStatistics.NumberOfGames,
                    NumberOfGamesLost = m.UserStatistics.NumberOfGamesLost,
                    NumberOfGamesWon = m.UserStatistics.NumberOfGamesWon,
                    NumberOfGuesses = m.UserStatistics.NumberOfGuesses,
                    NumberOfUnsuccessfulGuesses = m.UserStatistics.NumberOfUnsuccessfulGuesses,
                };
            }
        }

        public int NumberOfGames { get; private set; }

        public int NumberOfGamesLost { get; private set; }

        public int NumberOfGamesWon { get; private set; }

        public int NumberOfGuesses { get; private set; }

        public int NumberOfUnsuccessfulGuesses { get; private set; }

        public string User { get; set; }
    }
}
