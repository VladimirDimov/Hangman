namespace Hangman.Services.Data
{
    using System;
    using Contracts;
    using Hangman.Data.Common;
    using Hangman.Data.Models;

    public class StatisticsService : IStatisticsService
    {
        private readonly IDbGenericRepository<User, string> usersRepository;

        public StatisticsService(IDbGenericRepository<User, string> usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public void UpdateUserStatistics(string userId, bool isCurrentGameWinner, int numberOfErrors, int numberOfGuesses)
        {
            var user = this.usersRepository.GetById(userId);
            if (user.UserStatistics == null)
            {
                user.UserStatistics = new UserStatistics();
            }

            user.UserStatistics.NumberOfGames++;
            if (isCurrentGameWinner)
            {
                user.UserStatistics.NumberOfGamesWon++;
            }
            else
            {
                user.UserStatistics.NumberOfGamesLost++;
            }

            user.UserStatistics.NumberOfGuesses += numberOfGuesses;

            this.usersRepository.Save();
        }
    }
}
