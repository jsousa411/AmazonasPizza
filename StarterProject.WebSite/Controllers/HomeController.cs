using Microsoft.AspNetCore.Mvc;

using StarterProject.Context.Contexts;
using StarterProject.WebSite.Models;

using System.Collections.Generic;
using System.Diagnostics;


namespace StarterProject.WebSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var advertise = _context.AdvertisementConfig.ToList();

            var productAdConfigViewModel = new ProductAdvertiseConfigurationViewModel();

            if (advertise != null)
            {
                foreach (var ad in advertise)
                {
                    if (ad.Name == "variable1-s1")
                    {
                        productAdConfigViewModel.FirstCarrouselVariableOneS1 = ad.Type;
                    }
                    else if (ad.Name == "variable2-s1")
                    {
                        productAdConfigViewModel.FirstCarrouselVariableTwoS1 = ad.Type;
                    }
                    else if (ad.Name == "variable3-s1")
                    {
                        productAdConfigViewModel.FirstCarrouselVariableThreeS1 = ad.Type;
                    }
                    else if (ad.Name == "variable1-s2")
                    {
                        productAdConfigViewModel.FirstCarrouselVariableOneS2 = ad.Type;
                    }
                    else if (ad.Name == "variable2-s2")
                    {
                        productAdConfigViewModel.FirstCarrouselVariableTwoS2 = ad.Type;
                    }
                    else if (ad.Name == "variable3-s2")
                    {
                        productAdConfigViewModel.FirstCarrouselVariableThreeS2 = ad.Type;
                    }
                    else if (ad.Name == "variable4-s2")
                    {
                        productAdConfigViewModel.FirstCarrouselVariableFourS2 = ad.Type;
                    }
                    else if (ad.Name == "variable1-s3")
                    {
                        productAdConfigViewModel.FirstCarrouselVariableOneS3 = ad.Type;
                    }
                    else if (ad.Name == "variable2-s3")
                    {
                        productAdConfigViewModel.FirstCarrouselVariableTwoS3 = ad.Type;
                    }
                    else if (ad.Name == "variable3-s3")
                    {
                        productAdConfigViewModel.FirstCarrouselVariableThreeS3 = ad.Type;
                    }
                    else
                    {
                       // productAdConfigViewModel.FirstCarrouselVariableThreeS1 = ad.Type;
                    }
                }
            }

            return View(productAdConfigViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}