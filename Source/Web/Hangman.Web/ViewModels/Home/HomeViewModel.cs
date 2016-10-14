namespace Hangman.Web.ViewModels.Home
{
    using System.Linq;

    public class HomeViewModel
    {
        public IQueryable<StatisticsViewModel> Statistics { get; set; }
    }
}
