namespace Hangman.Services.Data.Contracts
{
    using System.Linq;
    using Hangman.Data.Models;

    public interface IStatisticsService
    {
        void UpdateUserStatistics(string userId, bool isCurrentGameWinner, int numberOfErrors, int wordLength);

        IQueryable<UserStatistics> All();
    }
}
