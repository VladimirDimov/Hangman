namespace Hangman.Web.Controllers
{
    using System.Web.Mvc;
    using AutoMapper;
    using Hangman.Services.Web;
    using Infrastructure.Mapping;

    public abstract class BaseController : Controller
    {
        public ICacheService Cache { get; set; }

        protected IMapper Mapper
        {
            get
            {
                return AutoMapperConfig.Configuration.CreateMapper();
            }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
#if !DEBUG
            filterContext.Controller.ControllerContext.HttpContext.Response.Redirect($"/errors/Error500");
#endif

            base.OnException(filterContext);
        }
    }
}
