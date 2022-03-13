﻿using Microsoft.AspNetCore.Mvc;
using StarterProject.Context.Contexts;
using StarterProject.WebSite.Models;
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
            var advertise = _context.AdvertisementConfig.FirstOrDefault();

            if(advertise != null)
             ViewBag.ProductAdvertise = advertise.Description;
            
            

            return View();
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