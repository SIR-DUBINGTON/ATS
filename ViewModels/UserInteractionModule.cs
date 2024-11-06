using ATS.DataAccess;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using MySqlConnector;
using System.Text;
using System.Threading.Tasks;



namespace ATS.ViewModels
{
    /// <summary>
    /// This class is responsible for handling user interactions with the system.
    /// </summary>
    public class UserInteractionModule
    {
        /// <summary>
        /// Database connection hub for establishing a connection to the database
        /// </summary>
        private readonly DatabaseConHub _databaseConHub;
        /// <summary>
        /// constructor for the UserInteractionModule class
        /// </summary>
        /// <param name="databaseConHub"></param>
        public UserInteractionModule(DatabaseConHub databaseConHub)
        {
            _databaseConHub = databaseConHub;
        }
        /// <summary>
        /// Method for registering a new user in the system
        /// </summary>
        /// <param name="username"></param>
        /// <param name="plainPassword"></param>
        public void RegisterUser(string username, string plainPassword)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(plainPassword);

            using (var connection = _databaseConHub.GetConnection())
            {
                string query = "INSERT INTO Users (username, passwordHash) VALUES (@username, @passwordHash)";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@passwordHash", passwordHash);

                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Method for validating a user's login credentials
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>Boolean</returns>
        public bool ValidateLogin(string username, string password)
        {
            using (var connection = _databaseConHub.GetConnection())
            {
                string query = "SELECT passwordHash FROM Users WHERE username = @username";
                var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@username", username);

                connection.Open();
                var storedHash = cmd.ExecuteScalar() as string;

                if (storedHash != null)
                {
                    return BCrypt.Net.BCrypt.Verify(password, storedHash);
                }
                else
                {
                    return false;
                }
            }
        }
    }

}