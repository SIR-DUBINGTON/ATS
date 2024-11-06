using ATS.DataAccess;
using ATS.Models;
using MySqlConnector;
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
using ATS.ViewModels;
using ATS.Helpers;


namespace ATS.Views
{
    /// <summary>
    /// Class for the RegistrationPage
    /// </summary>
    public sealed partial class RegistrationPage : Page
    {
        /// <summary>
        /// Instance of the DepartmentInteractionModule class
        /// </summary>
        private readonly DepartmentInteractionModule _departmentInteractionModule;
        private DatabaseConHub dbConHub = new DatabaseConHub();
        /// <summary>
        /// Constructor for the RegistrationPage class
        /// </summary>
        public RegistrationPage()
        {
            InitializeComponent();
            _departmentInteractionModule = new DepartmentInteractionModule(new DatabaseConHub());
            this.Loaded += Page_Loaded;
        }
        /// <summary>
        /// Method to load departments into the combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var departments = _departmentInteractionModule.GetAllDepartments();

            foreach (var department in departments)
            {
                cmbDepartment.Items.Add(department.GetDepartmentNameString());
            }
        }
        /// <summary>
        /// Method to register a user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedDepartment = cmbDepartment.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedDepartment))
            {
                var errorDialog = new MessageDialog("Please select a department.");
                await errorDialog.ShowAsync();
                return;
            }

            int departmentId = _departmentInteractionModule.GetDepartmentID(selectedDepartment);

            if (departmentId <= 0)
            {
                var errorDialog = new MessageDialog("Selected department does not exist.");
                await errorDialog.ShowAsync();
                return;
            }

            string hashedPassword = PasswordHelper.HashPassword(pwdPassword.Password);

            int loggedInUserId = await RegisterUserAsync(
                txtUsername.Text,
                hashedPassword,  
                txtFirstName.Text,
                txtLastName.Text,
                txtEmailAddress.Text,
                selectedDepartment,
                txtRole.Text
            );

            string loggedInUsername = txtUsername.Text;
            SessionManager.Initialize(loggedInUserId, loggedInUsername);

            Frame.Navigate(typeof(ATSHubPage));
        }
        /// <summary>
        /// Method to register a user asynchronously
        /// </summary>
        /// <param name="username"></param>
        /// <param name="passwordHash"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <param name="department"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        private async Task<int> RegisterUserAsync(string username, string passwordHash, string firstName, string lastName, string email, string department, string role)
        {
            int departmentID = _departmentInteractionModule.GetDepartmentID(department);
            string query = "INSERT INTO Users (username, passwordHash, firstName, lastName, emailAddress, DepartmentID, urole) " +
                           "VALUES (@username, @passwordHash, @firstName, @lastName, @Email, @departmentID, @urole)";

            using (MySqlConnection connection = dbConHub.GetConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@passwordHash", passwordHash);  
                cmd.Parameters.AddWithValue("@firstName", firstName);
                cmd.Parameters.AddWithValue("@lastName", lastName);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@departmentID", departmentID);
                cmd.Parameters.AddWithValue("@urole", role);

                try
                {
                    await connection.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    Console.WriteLine("User registered successfully.");

                    return (int)cmd.LastInsertedId;
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                    return -1;
                }
            }
        }
        /// <summary>
        /// Method to retrieve the user ID asynchronously
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private async Task<int> RetrieveUserIdAsync(string username)
        {
            string query = "SELECT id FROM Users WHERE username = @username";
            using (MySqlConnection connection = dbConHub.GetConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@username", username);

                try
                {
                    await connection.OpenAsync();
                    var result = await cmd.ExecuteScalarAsync();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                    return -1;
                }
            }
        }
        /// <summary>
        /// Method to navigate back to the previous page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}