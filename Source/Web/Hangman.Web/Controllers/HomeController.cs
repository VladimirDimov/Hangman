namespace Hangman.Web.Controllers
{
    using System;
    using System.Net;
    using System.Web.Mvc;
    using Hubs;

    public class HomeController : BaseController
    {
        public HomeController()
        {
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
    }
}
