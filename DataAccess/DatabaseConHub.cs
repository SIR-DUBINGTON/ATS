using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ATS.DataAccess
{
    public class DatabaseConHub
    {
            private string server;
            private string database;
            private string uid;
            private string password;

            public DatabaseConHub()
            {
                Initialize();
            }

            private void Initialize()
            {
                server = "localhost";
                database = "ats";
                uid = "root";
                password = "root"; // Plain text for development and testing purposes. Will be using a more secure way to store these
            }

            public MySqlConnection GetConnection()
            {
                string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";
                return new MySqlConnection(connectionString);
            }

            public bool ExecuteQuery(string query)
            {
                using (MySqlConnection connection = GetConnection())
                {
                    try
                    {
                        connection.Open();
                        using (MySqlCommand cmd = new MySqlCommand(query, connection))
                        {
                            cmd.ExecuteNonQuery();
                            return true;
                        }
                    }
                    catch (MySqlException ex)
                    {
                        switch (ex.Number)
                        {
                            case 0:
                                Console.WriteLine("Cannot connect to server. Contact administrator.");
                                break;
                            case 1045:
                                Console.WriteLine("Invalid username/password, please try again.");
                                break;
                            default:
                                Console.WriteLine(ex.Message);
                                break;
                        }
                        return false;
                    }
                }
            }
        }
    }