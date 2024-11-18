using ATS.DataAccess;
using ATS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using Windows.System;

namespace ATS.ViewModels
{
    public class SoftwareAssetInteractionModule
    {
        private readonly DatabaseConHub _databaseConHub;

        public SoftwareAssetInteractionModule(DatabaseConHub databaseConHub)
        {
            _databaseConHub = databaseConHub;
        }

        public List<SoftwareAsset> GetSoftwareAssetsForUser(int userId, string searchTerm = "")
        {
            List<SoftwareAsset> userAssets = new List<SoftwareAsset>();

            string query = "SELECT * FROM SoftwareAssets WHERE userId = @userId AND (osName LIKE @searchTerm OR osVersion LIKE @searchTerm OR manufacturer LIKE @searchTerm)";

            using (var connection = _databaseConHub.GetConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@searchTerm", $"%{searchTerm}%");

                connection.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32("id");
                        string osName = reader.GetString("osName");
                        string osVersion = reader.GetString("osVersion");
                        string manufacturer = reader.GetString("manufacturer");

                        SoftwareAsset softwareAsset = new SoftwareAsset(id, userId, osName, osVersion, manufacturer);
                        userAssets.Add(softwareAsset);
                    }
                }
            }
            return userAssets;
        }

        public bool RegisterSoftwareAsset(SoftwareAsset softwareAsset)
        {
            string query = @"INSERT INTO SoftwareAssets (id, userId, osName, osVersion, manufacturer) 
                     VALUES (@id, @userId, @osName, @osVersion, @manufacturer)";

            using (var connection = _databaseConHub.GetConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@userId", softwareAsset.userId);
                cmd.Parameters.AddWithValue("@id", softwareAsset.id);
                cmd.Parameters.AddWithValue("@osName", softwareAsset.osName);
                cmd.Parameters.AddWithValue("@osVersion", softwareAsset.osVersion);
                cmd.Parameters.AddWithValue("@manufacturer", softwareAsset.manufacturer);

                connection.Open();
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }

        public bool EditSoftwareAsset(SoftwareAsset softwareAsset)
        {
            string query = @"UPDATE SoftwareAssets 
                     SET osName = @osName, osVersion = @osVersion, manufacturer = @manufacturer 
                     WHERE id = @id";

            using (var connection = _databaseConHub.GetConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", softwareAsset.id);
                cmd.Parameters.AddWithValue("@osName", softwareAsset.osName);
                cmd.Parameters.AddWithValue("@osVersion", softwareAsset.osVersion);
                cmd.Parameters.AddWithValue("@manufacturer", softwareAsset.manufacturer);

                connection.Open();
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }

        public bool DeleteSoftwareAsset(int softwareAssetId, int userId)
        {
            string query = "DELETE FROM SoftwareAssets WHERE id = @id AND userId = @userId";

            using (var connection = _databaseConHub.GetConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", softwareAssetId);
                cmd.Parameters.AddWithValue("@userId", userId);

                connection.Open();
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }

    }
}
