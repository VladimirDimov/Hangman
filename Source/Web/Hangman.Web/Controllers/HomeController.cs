namespace Hangman.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using Hubs;
    using MvcTemplate.Services.Data.Contracts;
    public class HomeController : BaseController
    {
        private ICategoriesService categoriesService;

        public HomeController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult Log(/*string message,*/ string group)
        {
            var logger = new ConsoleLogger();
            logger.Log(DateTime.Today.ToString(), group);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult SinglePlayer()
        {
            var categories = this.categoriesService.All().ToList();

            return new HttpStatusCodeResult(200);
        }

        public ActionResult MultiPlayer()
        {
            return this.View("MultiPlayer");
        }
    }
}
