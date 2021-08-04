using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Services
{
    public class SecurityService : ISecurityService
    {
        public static IConfiguration Configuration;
        string sqlConnection = Configuration.GetConnectionString("EmployeeContext");

        public string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var asBytes = Encoding.Default.GetBytes(password);
            var hashed = sha.ComputeHash(asBytes);
            return Convert.ToBase64String(hashed);
        }

        public EmployeeModel FindMySalt(string username, EmployeeModel employee)
        {
            string queryString = "SELECT * FROM dbo.Employees WHERE Username = @Username";

            using (SqlConnection connection = new SqlConnection(sqlConnection))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                command.Parameters.AddWithValue("@Username", employee.Username);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            employee.Salt = Convert.ToString(reader.GetInt32(9));
                        }
                    }
                    reader.Close();
                    connection.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return employee;
        }

        public bool FindByUser(EmployeeModel employee)
        {
            bool success = false;

            string queryString = "SELECT * FROM dbo.Employees WHERE Username = @Username AND Password = @Password";

            using (SqlConnection connection = new SqlConnection(sqlConnection))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                employee = FindMySalt(employee.Username, employee);

                var hashPassword = HashPassword(employee.Password + employee.Salt);

                command.Parameters.AddWithValue("@Username", employee.Username);
                command.Parameters.AddWithValue("@Password", hashPassword);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        success = true;
                    }
                    else
                    {
                        success = false;
                    }
                    reader.Close();
                    connection.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return success;
        }

        public EmployeeModel AddRole(EmployeeModel employee)
        {
            string queryString = "SELECT * FROM dbo.Employees WHERE Username = @Username AND Password = @Password";

            using (SqlConnection connection = new SqlConnection(sqlConnection))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                employee = FindMySalt(employee.Username, employee);

                var hashPassword = HashPassword(employee.Password + employee.Salt);

                command.Parameters.AddWithValue("@Username", employee.Username);
                command.Parameters.AddWithValue("@Password", hashPassword);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    reader.Read();
                    employee.Role = reader.GetString(10);

                    reader.Close();
                    connection.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return employee;
        }

        public bool RegUser(EmployeeModel employee)
        {
            string queryString = "UPDATE dbo.Employees SET Username = @Username, Password = @Password, Salt = @Salt WHERE Email = @Email";

            using (SqlConnection connection = new SqlConnection(sqlConnection))
            {
                bool success = false;

                Random rand = new();
                int salt = rand.Next();
                string saltedPW = $"{employee.Password}{salt}";
                var hashPassword = HashPassword(saltedPW);

                SqlCommand command = new SqlCommand(queryString, connection);

                command.Parameters.AddWithValue("@Username", employee.Username);
                command.Parameters.AddWithValue("@Password", hashPassword);
                command.Parameters.AddWithValue("@Salt", salt);
                command.Parameters.AddWithValue("@Email", employee.Email);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                return success;
            }
        }
    }
}
