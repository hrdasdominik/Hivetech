using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Controllers
{
    public class EmployeeController : Controller
    {
        [Authorize(Roles = "Employee")]
        public IActionResult Index(EmployeeModel employee)
        {
            return View(employee);
        }
    }
}
