using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace ATS.DataAccess
{
    public class DatabaseConHub
    {

        private readonly string connectionString;


        public DatabaseConHub()
        {
            connectionString = "Server=localhost;Database=ats;User ID=root;Password='';";
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

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
