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
    
        public List<Asset> GetAssets()
        {
            List<Asset> assets = new List<Asset>();

            using (var connection = _databaseConHub.GetConnection())
            {
                connection.Open();
                string query = "SELECT id, name, model, manufacturer, type, ip, purchaseDate, textNotes FROM assets";
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32("id");
                            string name = reader.GetString("name");
                            string model = reader.GetString("model");
                            string manufacturer = reader.GetString("manufacturer");
                            string type = reader.GetString("type");
                            string ip = reader.GetString("ip");
                            DateTime purchaseDate = reader.GetDateTime("purchaseDate");
                            string textNotes = reader.GetString("textNotes");

                            Asset asset = new Asset(id, name, model, manufacturer, type, ip, purchaseDate, textNotes);
                            assets.Add(asset);
                        }
                    }
                }
            }
            return assets;

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
