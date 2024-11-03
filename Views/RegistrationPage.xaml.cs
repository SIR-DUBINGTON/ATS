using ATS.DataAccess;
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


namespace ATS.Views
{
    public sealed partial class RegistrationPage : Page
    {
        private readonly DepartmentInteractionModule _departmentInteractionModule;
        private DatabaseConHub dbConHub = new DatabaseConHub();

        public RegistrationPage()
        {
            InitializeComponent();
            _departmentInteractionModule = new DepartmentInteractionModule(new DatabaseConHub());
            this.Loaded += Page_Loaded;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var departments = _departmentInteractionModule.GetAllDepartments();

            foreach (var department in departments)
            {
                cmbDepartment.Items.Add(department.GetDepartmentNameString());
            }
        }


        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedDepartment = cmbDepartment.SelectedItem.ToString();
            int departmentId = _departmentInteractionModule.GetDepartmentID(selectedDepartment);

            if (departmentId <= 0)
            {
                var dialog = new MessageDialog("Selected department does not exist.");
                await dialog.ShowAsync();
                return;
            }

            await RegisterUserAsync(
                txtUsername.Text,
                pwdPassword.Password,
                txtFirstName.Text,
                txtLastName.Text,
                txtEmailAddress.Text,
                selectedDepartment,
                txtRole.Text
            );
        }

        private async Task RegisterUserAsync(string username, string passwordHash, string firstName, string lastName, string email, string department, string role)
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
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }
        }
    }
}