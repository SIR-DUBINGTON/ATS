using ATS.DataAccess;
using ATS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MySqlConnector;
using ATS.Helpers;

namespace ATS.Views
{
    /// <summary>
    /// Class for the login page of the application
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        /// <summary>
        /// Instance of the DatabaseConHub class for establishing a connection to the database
        /// </summary>
        private readonly DatabaseConHub _databaseConHub = new DatabaseConHub();
        /// <summary>
        /// Initializes a new instance of the LoginPage class
        /// </summary>
        public LoginPage()
        {
            this.InitializeComponent();
        }
        /// <summary>
        /// Event handler for the login button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ErrorTextBlock.Text = "Please enter both username and password.";
                return;
            }

            bool isValidUser = await AuthenticateUser(username, password);
            if (isValidUser)
            {
                Frame.Navigate(typeof(ATSHubPage));
            }
            else
            {
                ErrorTextBlock.Text = "Invalid username or password.";
            }
        }
        /// <summary>
        /// Method that authenticates the user by checking the username and password against the database
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>Boolean</returns>
        private async Task<bool> AuthenticateUser(string username, string password)
        {
            using (var connection = _databaseConHub.GetConnection())
            {
                connection.Open();
                string query = "SELECT id, passwordHash FROM Users WHERE username = @username";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int userId = reader.GetInt32("id");
                            string storedHash = reader.GetString("passwordHash");

                            if (PasswordHelper.VerifyPassword(password, storedHash))
                            {
                                SessionManager.Initialize(userId, username); 
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }
        /// <summary>
        /// Method that navigates back to the previous page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
