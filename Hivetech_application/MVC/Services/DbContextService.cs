using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Services
{
    public class DbContextService : IDbContextService
    {
        public static IConfiguration Configuration;

        string sqlConnection = Configuration.GetConnectionString("EmployeeContext");

        public List<EmployeeModel> FetchEmployees()
        {
            List<EmployeeModel> returnList = new List<EmployeeModel>();

            string queryString = "SELECT * FROM dbo.Employees";

            using (SqlConnection connection = new SqlConnection(sqlConnection))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            EmployeeModel employee = new EmployeeModel();
                            employee.Id = reader.GetInt32(0);
                            employee.Name = reader.GetString(1);
                            employee.Surname = reader.GetString(2);
                            employee.Birthdate = reader.GetDateTime(3);
                            employee.City = reader.GetString(4);
                            employee.Email = reader.GetString(5);
                            employee.Hours = reader.GetInt32(6);
                            if (reader.IsDBNull(7))
                            {
                                employee.Username = "0";
                            }
                            else
                            {
                                employee.Username = reader.GetString(7);
                            }
                            returnList.Add(employee);
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
            return returnList;
        }

        public EmployeeModel FetchByIdProc(int id)
        {
            EmployeeModel employee = new EmployeeModel();

            using (SqlConnection connection = new SqlConnection(sqlConnection))
            {
                string queryString = "SELECT * FROM dbo.Employees WHERE Id = @Id";

                SqlCommand command = new SqlCommand(queryString, connection);

                command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = id;

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        employee.Id = reader.GetInt32(0);
                        employee.Name = reader.GetString(1);
                        employee.Surname = reader.GetString(2);
                        employee.Birthdate = reader.GetDateTime(3);
                        employee.City = reader.GetString(4);
                        employee.Email = reader.GetString(5);
                        employee.Hours = reader.GetInt32(6);
                        if (reader.IsDBNull(7))
                        {
                            employee.Username = "0";
                        }
                        else
                        {
                            employee.Username = reader.GetString(7);
                        }
                    }
                }
                reader.Close();
                connection.Close();
            }

            return employee;
        }
        public EmployeeModel FetchById(int id)
        {
            EmployeeModel employee = new EmployeeModel();
            employee = FetchByIdProc(id);
            return employee;
        }
        public EmployeeModel FetchByUsername(EmployeeModel employee)
        {
            using (SqlConnection connection = new SqlConnection(sqlConnection))
            {
                string queryString = "SELECT * FROM dbo.Employees WHERE Username = @Username";

                SqlCommand command = new SqlCommand(queryString, connection);

                command.Parameters.Add("@Username", System.Data.SqlDbType.VarChar, 255).Value = employee.Username;

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        employee.Id = reader.GetInt32(0);
                        employee.Name = reader.GetString(1);
                        employee.Surname = reader.GetString(2);
                        employee.Birthdate = reader.GetDateTime(3);
                        employee.City = reader.GetString(4);
                        employee.Email = reader.GetString(5);
                        employee.Hours = reader.GetInt32(6);
                    }
                }
                reader.Close();
                connection.Close();
            }

            return employee;
        }

        public bool DoesUsernameExist(string username)
        {
            bool success = false;
            string queryString = "SELECT * FROM dbo.Employees WHERE Username = @Username";

            using (SqlConnection connection = new SqlConnection(sqlConnection))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add("@Username", System.Data.SqlDbType.VarChar, 255).Value = username;

                connection.Open();

                try
                {
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

        public EmployeeModel CreateOrUpdateEmployee(EmployeeModel employee)
        {
            using (SqlConnection connection = new SqlConnection(sqlConnection))
            {
                string queryString = "";

                if (employee.Id <= 0)
                {
                    queryString = "INSERT INTO dbo.Employees (Name, Surname, Birthdate, City, Email, Hours, Role) VALUES(@Name, @Surname, @Birthdate, @City, @Email, @Hours, @Role)";
                }
                else
                {
                    queryString = "UPDATE dbo.Employees SET Name = @Name, Surname = @Surname, Birthdate = @Birthdate, City = @City, Email = @Email, Hours = @Hours, Role = @Role WHERE Id = @Id";
                }

                SqlCommand command = new SqlCommand(queryString, connection);

                command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = employee.Id;
                command.Parameters.Add("@Name", System.Data.SqlDbType.VarChar, 255).Value = employee.Name;
                command.Parameters.Add("@Surname", System.Data.SqlDbType.VarChar, 255).Value = employee.Surname;
                command.Parameters.Add("@Birthdate", System.Data.SqlDbType.Date).Value = employee.Birthdate;
                command.Parameters.Add("@City", System.Data.SqlDbType.VarChar, 255).Value = employee.City;
                command.Parameters.Add("@Email", System.Data.SqlDbType.VarChar, 255).Value = employee.Email;
                command.Parameters.Add("@Hours", System.Data.SqlDbType.Int).Value = employee.Hours;
                command.Parameters.Add("@Role", System.Data.SqlDbType.VarChar, 255).Value = employee.Role;

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
            return employee;
        }

        public void DeleteEmployee(int id)
        {
            using (SqlConnection connection = new SqlConnection(sqlConnection))
            {
                string queryString = "DELETE FROM dbo.Employees WHERE Id = @Id";

                SqlCommand command = new SqlCommand(queryString, connection);

                command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = id;
                try
                {
                    connection.Open();

                    command.ExecuteNonQuery();

                    connection.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                
            }
        }

        public List<EmployeeModel> SearchForEmployee(string searchPhrase)
        {
            List<EmployeeModel> returnList = new List<EmployeeModel>();

            string queryString = "SELECT Name, Surname, City, Hours FROM dbo.Employees WHERE Name LIKE @searchPhrase OR Surname LIKE @searchPhrase OR City LIKE @searchPhrase OR Hours LIKE @searchPhrase";

            using (SqlConnection connection = new SqlConnection(sqlConnection))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                command.Parameters.Add("@searchPhrase", System.Data.SqlDbType.VarChar, 255).Value = "%" + searchPhrase + "%";

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            EmployeeModel employee = new EmployeeModel();
                            employee.Id = reader.GetInt32(0);
                            employee.Name = reader.GetString(1);
                            employee.Surname = reader.GetString(2);
                            employee.Birthdate = reader.GetDateTime(3);
                            employee.City = reader.GetString(4);
                            employee.Email = reader.GetString(5);
                            employee.Hours = reader.GetInt32(6);
                            if (reader.IsDBNull(7))
                            {
                                employee.Username = "0";
                            }
                            else
                            {
                                employee.Username = reader.GetString(7);
                            }

                            returnList.Add(employee);
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

            return returnList;

        }
    }
}
