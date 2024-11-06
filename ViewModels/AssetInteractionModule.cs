using ATS.DataAccess;
using ATS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;


namespace ATS.ViewModels
{
    /// <summary>
    /// Class that handles the assets interaction in the system.
    /// </summary>
    public class AssetInteractionModule
    {
        /// <summary>
        /// Instance of the DatabaseConHub class.
        /// </summary>
        private readonly DatabaseConHub _databaseConHub;
        /// <summary>
        /// Initializes a new instance of the AssetInteractionModule class.
        /// </summary>
        /// <param name="databaseConHub"></param>
        public AssetInteractionModule(DatabaseConHub databaseConHub)
        {
            _databaseConHub = databaseConHub;
        }
        /// <summary>
        /// Method that gets the assets for a specific user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="searchTerm"></param>
        /// <returns>userAssets</returns>
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

                        Asset asset = new Asset(id, userId, name, model, manufacturer, type, ip, purchaseDate, textNotes);
                        userAssets.Add(asset);
                    }
                }
            }
            return userAssets;
        }
        /// <summary>
        /// Method that registers an asset in the system.
        /// </summary>
        /// <param name="asset"></param>
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
        /// <summary>
        /// Method that deletes an asset from the system.
        /// </summary>
        /// <param name="assetId"></param>
        public void DeleteAsset(int assetId)
        {
            using (var connection = _databaseConHub.GetConnection())
            {
                connection.Open();
                string query = "DELETE FROM Assets WHERE id = @assetId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@assetId", assetId);
                    command.ExecuteNonQuery();
                }
            }
        }
        /// <summary>
        /// Method that updates the details of an asset in the system.
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="purchaseDate"></param>
        /// <param name="textNotes"></param>
        public void UpdateAssetDetails(int assetId, DateTime purchaseDate, string textNotes)
        {
            using (var connection = _databaseConHub.GetConnection())
            {
                connection.Open();
                string query = "UPDATE assets SET purchaseDate = @purchaseDate, textNote = @textNote WHERE id = @assetId";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@purchaseDate", purchaseDate);
                    command.Parameters.AddWithValue("@textNote", textNotes);
                    command.Parameters.AddWithValue("@assetId", assetId);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
