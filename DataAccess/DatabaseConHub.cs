using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace ATS.DataAccess
{
    /// <summary>
    /// This class is in charge of establishing a connection to the database and executing queries
    /// </summary>
    public class DatabaseConHub
    {
        /// <summary>
        /// Connection string for the database
        /// </summary>
        private readonly string connectionString;

        /// <summary>
        /// Constructor for the DatabaseConHub class
        /// </summary>
        public DatabaseConHub()
        {
            connectionString = "Server=localhost;Database=ats;User ID=root;Password='';";
        }
        /// <summary>
        ///  Returns a MySqlConnection object for establishing a connection to the database
        /// </summary>
        /// <returns>MySqlConnection</returns>
        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
        /// <summary>
        /// Executes a non-query SQL statement asynchronously and returns a boolean indicating success or failure
        /// </summary>
        /// <param name="query"></param>
        /// <returns>Boolean</returns>
        public async Task<bool> ExecuteQueryAsync(string query)
        {
            using (var connection = GetConnection())
            {
                try
                {
                    await connection.OpenAsync();
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }
                    return true;
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"Database error: {ex.Message}");
                    return false;
                }
            }
        }
        /// <summary>
        /// Executes a SQL query asynchronously and returns a MySqlDataReader for reading the results
        /// </summary>
        /// <param name="query"></param>
        /// <returns>MySqlDataReader</returns>
        public async Task<MySqlDataReader> ExecuteReaderAsync(string query)
        {
            var connection = GetConnection();
            try
            {
                await connection.OpenAsync();
                var cmd = new MySqlCommand(query, connection);
                return await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.CloseConnection);
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                connection.Close();
                throw;
            }
        }
    }
}
