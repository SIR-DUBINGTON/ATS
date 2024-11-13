using ATS.Models;
using ATS.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ATS.DataAccess;
using ATS.Helpers;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ATS.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SoftwareAssetRegistrationPage : Page
    {
        private readonly SoftwareAssetInteractionModule _softwareAssetInteractionModule;

        public SoftwareAssetRegistrationPage()
        {
            this.InitializeComponent();
            _softwareAssetInteractionModule = new SoftwareAssetInteractionModule(new DataAccess.DatabaseConHub());

            // Automatically capture OS information on page load
            LoadSoftwareAssetInfo();
        }

        private void LoadSoftwareAssetInfo()
        {
            // Capture OS Information
            string osName = Environment.OSVersion.Platform.ToString();       // Platform name
            string osVersion = Environment.OSVersion.Version.ToString();     // OS version
            string manufacturer = "Microsoft";  // Hardcoded, as this information might not be available in Environment

            // Display information on the UI
            OSNameText.Text = osName;
            OSVersionText.Text = osVersion;
            ManufacturerText.Text = manufacturer;
        }

        private async void RegisterSoftwareAssetButton_Click(object sender, RoutedEventArgs e)
        {
            // Capture OS information from the UI for registration
            string osName = OSNameText.Text;
            string osVersion = OSVersionText.Text;
            string manufacturer = ManufacturerText.Text;

            // Assuming user ID is retrieved from the session or current login
            int userId = SessionManager.Instance.UserId;
            SoftwareAsset softwareAsset = new SoftwareAsset(0, osName, osVersion, manufacturer);

            // Register software asset using SoftwareAssetInteractionModule
            bool isRegistered = _softwareAssetInteractionModule.RegisterSoftwareAsset(softwareAsset);

            if (isRegistered)
            {
                await new ContentDialog
                {
                    Title = "Success",
                    Content = "Software asset registered successfully.",
                    CloseButtonText = "OK"
                }.ShowAsync();
            }
            else
            {
                await new ContentDialog
                {
                    Title = "Error",
                    Content = "Failed to register the software asset. Please try again.",
                    CloseButtonText = "OK"
                }.ShowAsync();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
