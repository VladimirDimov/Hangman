namespace Hangman.Services.Data.Contracts
{
    public interface IStatisticsService
    {
        void UpdateUserStatistics(string userId, bool isCurrentGameWinner, int numberOfErrors, int wordLength);
    }
}