using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StarterProject.WebSite.Controllers
{
    [Authorize]
    public class ShopCartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
   }
}