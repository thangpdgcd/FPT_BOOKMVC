﻿using FPT_BOOKMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
namespace FPT_BOOKMVC.Areas.UnAuthenticated.Controllers
{
    [Area(Utils.SD.UnAuthenticatedArea)]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
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
