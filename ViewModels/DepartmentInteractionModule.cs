using ATS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using ATS.Models;

namespace ATS.ViewModels
{
    public class DepartmentInteractionModule
    {
        private readonly DatabaseConHub _databaseConHub;

        public DepartmentInteractionModule(DatabaseConHub databaseConHub)
        {
            _databaseConHub = databaseConHub;
        }

        public int GetDepartmentID(string departmentName)
        {
            int departmentID = -1;
            string query = "SELECT id FROM Departments WHERE dName = @departmentName";

            using (var connection = _databaseConHub.GetConnection())
            {
                connection.Open();
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@departmentName", departmentName);
                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        departmentID = Convert.ToInt32(result);
                    }
                }
            }

            return departmentID;
        }

        public List<Department> GetAllDepartments()
        {
            List<Department> departments = new List<Department>();

            using (var connection = _databaseConHub.GetConnection())
            {
                connection.Open();
                string query = "SELECT id, dName FROM departments";
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32("id");
                            string departmentNameStr = reader.GetString("dName");

                            string enumName = departmentNameStr.Replace(" ", "");

                            if (Enum.TryParse(typeof(DepartmentName), enumName, true, out var departmentName))
                            {
                                departments.Add(new Department(id, (DepartmentName)departmentName, new List<User>()));
                            }
                            else
                            {
                                Console.WriteLine($"Department '{departmentNameStr}' not found in enum.");
                            }
                        }
                    }
                }
            }

            return departments;
        }
    }
}