using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Services
{
    public interface IDbContextService
    {
        List<EmployeeModel> FetchEmployees();
        EmployeeModel FetchById(int id);
        EmployeeModel FetchByUsername(EmployeeModel employee);
        bool DoesUsernameExist(string username);
        EmployeeModel CreateOrUpdateEmployee(EmployeeModel employee);
        void DeleteEmployee(int id);
        List<EmployeeModel> SearchForEmployee(string searchPhrase);
    }
}
