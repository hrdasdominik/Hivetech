using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Services
{
    public interface ISecurityService
    {
        public string HashPassword(string password);
        public EmployeeModel FindMySalt(string username, EmployeeModel employee);
        public bool FindByUser(EmployeeModel employee);
        public EmployeeModel AddRole(EmployeeModel employee);
        public bool RegUser(EmployeeModel employee);
    }
}
