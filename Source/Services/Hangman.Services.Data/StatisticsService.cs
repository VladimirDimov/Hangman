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
            user.UserStatistics.NumberOfSuccessfulGuesses += numberOfGuesses - numberOfErrors;
            user.UserStatistics.NumberOfUnsuccessfulGuesses += numberOfErrors;

            this.usersRepository.Save();
        }

        public IQueryable<UserStatistics> All()
        {
            return this.cacheService.Get("statistics", () =>
            {
                var statistics = this.usersRepository.AllWithDeleted()
                .Where(u => u.UserStatistics != null)
                .Select(u => u.UserStatistics);

                return statistics.ToList();
            },
            GlobalConstants.TimeToCacheStatisticsInSeconds)
            .AsQueryable();
        }
    }
}
