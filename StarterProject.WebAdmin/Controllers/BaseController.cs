using Microsoft.AspNetCore.Mvc;
using StarterProject.Context.Contexts;

namespace StarterProject.WebAdmin.Controllers
{
	public class BaseController : Controller
	{
        internal AppDbContext Context { get; set; }

        public BaseController()
        {
            Context = new AppDbContext();
        }

        public ActionResult JavaScript(string js)
        {
            return Content($"<script language='javascript' type='text/javascript'>{js}</script>");
        }
    }
}
