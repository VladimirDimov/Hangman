namespace Hangman.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Data.Models;
    using Hubs;
    using Services.Data.Contracts;
    using ViewModels.Home;

    public class HomeController : BaseController
    {
        private IStatisticsService statisticsService;

        public HomeController(IStatisticsService statisticsService)
        {
            this.statisticsService = statisticsService;
        }

        public ActionResult Index()
        {
            var notifier = new Notifier();

            var statistics = this.statisticsService.All()
                .Select(StatisticsViewModel.FromModel)
                .OrderBy(x => x.NumberOfGamesWon);

            var model = new HomeViewModel
            {
                Statistics = statistics
            };

            return this.View(statistics);
        }
    }
}
