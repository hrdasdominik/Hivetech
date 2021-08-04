using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly IDbContextService _dbContextService;
        private readonly ISecurityService _securityService;

        public AdminController(IDbContextService dbContextService, ISecurityService securityService)
        {
            _dbContextService = dbContextService;
            _securityService = securityService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Employees(string sortOrder)
        {
            List<EmployeeModel> employees = _dbContextService.FetchEmployees();
            return View(employees);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult SearchForEmployee(string searchPhrase)
        {
            List<EmployeeModel> searchResults = _dbContextService.SearchForEmployee(searchPhrase);
            return View("Employees", searchResults);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult ResetEmployees()
        {
            List<EmployeeModel> employees = _dbContextService.FetchEmployees();
            return View("Employees", employees);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CreateEmployee()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult RegisterEmployee(EmployeeModel employee)
        {
            _dbContextService.CreateOrUpdateEmployee(employee);
            return RedirectToAction("Employees");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult EditEmployee(int id)
        {
            return View("EditEmployee", _dbContextService.FetchById(id));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult EditEmployee(EmployeeModel employee)
        {

            _dbContextService.CreateOrUpdateEmployee(employee);
            return RedirectToAction("Employees");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult DeleteEmployee(int id)
        {
            _dbContextService.DeleteEmployee(id);
            return RedirectToAction("Employees");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult DetailsEmployee(int id)
        {
            return View("DetailsEmployee", _dbContextService.FetchById(id));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult UserForm()
        {
            return View();

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult RegisterUser(EmployeeModel employee)
        {
            _securityService.RegUser(employee);
            return View("Employees");
        }
    }
}
