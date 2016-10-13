namespace Hangman.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Services.Data.Contracts;
    using ViewModels.Home;

    public class HomeController : BaseController
    {
        private IUsersService usersService;

        public HomeController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public ActionResult Index()
        {
            var statistics = this.usersService.All()
                .Select(StatisticsViewModel.FromModel);

            var model = new HomeViewModel
            {
                Statistics = statistics
            };

            return this.View(model);
        }

        //public ActionResult Log(/*string message,*/ string group)
        //{
        //    var logger = new ConsoleLogger();
        //    logger.Log(DateTime.Today.ToString(), group);

        //    return new HttpStatusCodeResult(HttpStatusCode.OK);
        //}
    }
}
