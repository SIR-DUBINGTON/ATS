using ATS.DataAccess;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace ATS.Views
{
    public sealed partial class RegistrationPage : Page
    {
        private readonly DatabaseConHub _dbConHub = new DatabaseConHub();

        public RegistrationPage()
        {
            this.InitializeComponent();
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string passwordHash = pwdPassword.Password; 
            string firstName = txtFirstName.Text;
            string lastName = txtLastName.Text;
            string email = txtEmailAddress.Text;
            string department = cmbDepartment.SelectedItem.ToString();
            string role = txtRole.Text;

            await RegisterUserAsync(username, passwordHash, firstName, lastName, email, department, role);
        }
        private async Task RegisterUserAsync(string username, string passwordHash, string firstName, string lastName, string email, string department, string role)
        {
            string query = $"INSERT INTO Users (username, passwordHash, firstName, lastName, emailAddress, department, urole) " +
                           $"VALUES ('{username}', '{passwordHash}', '{firstName}', '{lastName}', '{email}', '{department}', '{role}');";

            try
            {
                var success = await _dbConHub.ExecuteQueryAsync(query);
                if (success)
                {
                    await new MessageDialog("Registration successful!").ShowAsync();
                }
                else
                {
                    await new MessageDialog("Registration failed. Please try again.").ShowAsync();
                }
            }
            catch (Exception ex)
            {
                await new MessageDialog($"Error: {ex.Message}").ShowAsync();
            }
        }
    }
}
