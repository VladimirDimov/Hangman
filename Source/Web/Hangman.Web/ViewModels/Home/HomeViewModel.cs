using System.Linq;

namespace Hangman.Web.ViewModels.Home
{
    public class HomeViewModel
    {
        public IQueryable<StatisticsViewModel> Statistics { get; set; }
    }
}
