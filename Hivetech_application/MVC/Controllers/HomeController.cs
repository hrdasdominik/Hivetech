using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC.Models;
using MVC.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Security.Claims;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDbContextService _dbContextService;

        public HomeController(IDbContextService dbContextService)
        {
            _dbContextService = dbContextService;
        }

        public PartialViewResult Weather()
        {
            return PartialView("_Weather");
        }

        public IActionResult Index()
        {
            return RedirectToAction("Login", "Login");
        }

        [HttpGet]
        public IActionResult Employees(string sortOrder)
        {
            return View(_dbContextService.FetchEmployees());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
