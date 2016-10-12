namespace Hangman.Web.Controllers
{
    using System.Web.Mvc;

    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return this.View();
        }

        //public ActionResult Log(/*string message,*/ string group)
        //{
        //    var logger = new ConsoleLogger();
        //    logger.Log(DateTime.Today.ToString(), group);

        //    return new HttpStatusCodeResult(HttpStatusCode.OK);
        //}
    }
}
