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
    /// <summary>
    /// This class is in charge of handling interactions with the department table in the database
    /// </summary>
    public class DepartmentInteractionModule
    {
        /// <summary>
        /// Instance of the DatabaseConHub class
        /// </summary>
        private readonly DatabaseConHub _databaseConHub;
        /// <summary>
        /// Initializes the DepartmentInteractionModule class
        /// </summary>
        /// <param name="databaseConHub"></param>
        public DepartmentInteractionModule(DatabaseConHub databaseConHub)
        {
            _databaseConHub = databaseConHub;
        }
        /// <summary>
        /// Method to get the department ID from the department name
        /// </summary>
        /// <param name="departmentName"></param>
        /// <returns>departmentID</returns>
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
        /// <summary>
        /// Method to get all departments from the database
        /// </summary>
        /// <returns>departments</returns>
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