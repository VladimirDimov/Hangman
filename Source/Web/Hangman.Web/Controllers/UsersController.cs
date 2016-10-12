namespace Hangman.Web.Controllers
{
    using System.Web.Mvc;
    using Services.Data.Contracts;

    public class UsersController : Controller
    {
        private IUsersService users;

        public UsersController(IUsersService users)
        {
            this.users = users;
        }

        public ActionResult Index()
        {
            var users = this.users.All();
            return this.View(users);
        }
    }
}
