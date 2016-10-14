namespace Hangman.Web.Controllers
{
    using System.Web.Mvc;

    public class ErrorsController : BaseController
    {
        public ActionResult Error404()
        {
            return this.View("Error404");
        }

        public ActionResult Error401()
        {
            return this.View("Error401");
        }

        public ActionResult Error400()
        {
            return this.View("Error400");
        }

        public ActionResult Error500()
        {
            return this.View("Error500");
        }

        public ActionResult UnhandledError()
        {
            return this.View("UnhandledError");
        }
    }
}
