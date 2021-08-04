using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Services;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly ISecurityService _securityService;
        private readonly IDbContextService _dbContextService;

        public LoginController(ISecurityService securityService, IDbContextService dbContextService)
        {
            _securityService = securityService;
            _dbContextService = dbContextService;
        }

        public IActionResult Index()
        {
            return View("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(EmployeeModel employee)
        {
            bool success = _securityService.FindByUser(employee);
            employee = _securityService.AddRole(employee);
            if(success)
            {
                var myClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, employee.Username),
                    new Claim(ClaimTypes.Role, employee.Role)
                };

                var myIdentity = new ClaimsIdentity(myClaims, "My identity");

                var userPrincipal = new ClaimsPrincipal(new[] { myIdentity });

                HttpContext.SignInAsync(userPrincipal);

                employee = _dbContextService.FetchByUsername(employee);

                if(employee.Role == "Admin")
                {
                    return RedirectToAction("Employees", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "Employee", employee);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Incorrect username or password.");
                return View("Login");
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            foreach(var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult DoesUserExist(EmployeeModel employee)
        {
            bool status = _dbContextService.DoesUsernameExist(employee.Username);

            if(status == false)
            {
                return Json(true);
            }
            else
            {
                return Json($"Username already in use.");
            }
        }
    }
}
