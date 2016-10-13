namespace Hangman.Services.Data
{
    using System.Linq;
    using Common;
    using Contracts;
    using Hangman.Data.Common;
    using Hangman.Data.Models;
    using Web;

    public class StatisticsService : IStatisticsService
    {
        private readonly IDbGenericRepository<User, string> usersRepository;
        private ICacheService cacheService;

        public StatisticsService(IDbGenericRepository<User, string> usersRepository, ICacheService cacheService)
        {
            this.usersRepository = usersRepository;
            this.cacheService = cacheService;
        }

        public void UpdateUserStatistics(string userId, bool isCurrentGameWinner, int numberOfErrors, int numberOfGuesses)
        {
            var user = this.usersRepository.GetById(userId);

            user.NumberOfGames++;
            if (isCurrentGameWinner)
            {
                user.NumberOfGamesWon++;
            }
            else
            {
                user.NumberOfGamesLost++;
            }

            user.NumberOfGuesses += numberOfGuesses;
            user.NumberOfSuccessfulGuesses += numberOfGuesses - numberOfErrors;
            user.NumberOfUnsuccessfulGuesses += numberOfErrors;

            this.usersRepository.Save();
        }

        public IQueryable<User> All()
        {
            return this.cacheService.Get("statistics", () =>
            {
                var statistics = this.usersRepository.AllWithDeleted();

                return statistics.ToList();
            },
            GlobalConstants.TimeToCacheStatisticsInSeconds)
            .AsQueryable();
        }
    }
}
