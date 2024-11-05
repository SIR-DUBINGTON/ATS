using ATS.DataAccess;
using ATS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace ATS.ViewModels
{
    public class AssetInteractionModule
    {
        private readonly DatabaseConHub _databaseConHub;

        public AssetInteractionModule(DatabaseConHub databaseConHub)
        {
            _databaseConHub = databaseConHub;
        }

        public List<Asset> GetAssetsForUser(int userId, string searchTerm = "")
        {
            List<Asset> userAssets = new List<Asset>();

            string query = "SELECT * FROM assets WHERE userId = @userId AND (aname LIKE @searchTerm OR model LIKE @searchTerm)";

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
                        string name = reader.GetString("aname");
                        string model = reader.GetString("model");
                        string manufacturer = reader.GetString("manufacturer");
                        string type = reader.GetString("atype");
                        string ip = reader.GetString("ip");
                        DateTime purchaseDate = reader.GetDateTime("purchaseDate");
                        string textNotes = reader.GetString("textNote");

                        Asset asset = new Asset(id, name, model, manufacturer, type, ip, purchaseDate, textNotes);
                        userAssets.Add(asset);
                    }
                }
            }
            return userAssets;
        }

        public void RegisterAsset(Asset asset)
        {
            using (var connection = _databaseConHub.GetConnection())
            {
                connection.Open();
                string query = @"INSERT INTO assets (userId, aname, model, manufacturer, atype, ip, purchaseDate, textNote) 
                         VALUES (@userId, @aname, @model, @manufacturer, @atype, @ip, @purchaseDate, @textNote)";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", asset.userId);
                    command.Parameters.AddWithValue("@aname", asset.name);
                    command.Parameters.AddWithValue("@model", asset.model);
                    command.Parameters.AddWithValue("@manufacturer", asset.manufacturer);
                    command.Parameters.AddWithValue("@atype", asset.type);
                    command.Parameters.AddWithValue("@ip", asset.ip);
                    command.Parameters.AddWithValue("@purchaseDate", asset.purchaseDate);
                    command.Parameters.AddWithValue("@textNote", asset.textNotes);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
