namespace Hangman.Web.Areas.Administration.Controllers
{
    using System.Web.Mvc;

    using Hangman.Common;
    using Hangman.Web.Controllers;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public class AdministrationController : BaseController
    {
    }
}
