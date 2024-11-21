using ATS.DataAccess;
using ATS.Models;
using ATS.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Profile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Networking.Connectivity;
using Windows.System;
using Windows.Security.ExchangeActiveSyncProvisioning;


namespace ATS.Views
{
    /// <summary>
    /// Class that represents the Asset Registration Page.
    /// </summary>
    public sealed partial class HardwareAssetRegistrationPage : Page
    {
        /// <summary>
        /// Instance of the AssetInteractionModule class.
        /// </summary>
        private readonly HardwareAssetInteractionModule _assetInteractionModule;
        private DatabaseConHub _databaseConHub = new DatabaseConHub();
        /// <summary>
        /// Constructor for the AssetRegistrationPage class.
        /// </summary>
        public HardwareAssetRegistrationPage()
        {
            this.InitializeComponent();
            _assetInteractionModule = new HardwareAssetInteractionModule(new DatabaseConHub());
        }
        /// <summary>
        /// Method that navigates back to the previous page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
        /// <summary>
        /// Method that shows the notes text box when the checkbox is checked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChkAddNotes_Checked(object sender, RoutedEventArgs e)
        {
            txtNotes.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// Method that hides the notes text box when the checkbox is unchecked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChkAddNotes_Unchecked(object sender, RoutedEventArgs e)
        {
            txtNotes.Visibility = Visibility.Collapsed;
        }
        /// <summary>
        /// Method that shows the purchase date date picker when the checkbox is checked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChkAddPurchaseDate_Checked(object sender, RoutedEventArgs e)
        {
            dpPurchaseDate.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// Method that hides the purchase date date picker when the checkbox is unchecked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChkAddPurchaseDate_Unchecked(object sender, RoutedEventArgs e)
        {
            dpPurchaseDate.Visibility = Visibility.Collapsed;
        }
        /// <summary>
        /// Method that registers an asset in the system.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RegisterAssetButton_Click(object sender, RoutedEventArgs e)
        {
            string deviceName = GetDeviceName();
            string deviceModel = GetDeviceModel();
            string manufacturer = GetManufacturer();
            string type = GetDeviceType();
            string ipAddress = GetIPAddress();
            string notes = chkAddNotes.IsChecked == true ? txtNotes.Text : string.Empty;
            DateTime purchaseDate = chkAddPurchaseDate.IsChecked == true && dpPurchaseDate.SelectedDate.HasValue
                                    ? dpPurchaseDate.SelectedDate.Value.DateTime
                                    : DateTime.Now;

            var asset = new HardwareAsset(0, SessionManager.Instance.UserId, deviceName, deviceModel, manufacturer, type, ipAddress, purchaseDate, notes)
            {
                name = deviceName,
                model = deviceModel,
                manufacturer = manufacturer,
                type = type,
                ip = ipAddress,
                purchaseDate = purchaseDate,
                textNotes = notes
            };

            _assetInteractionModule.RegisterAsset(asset);
            Frame.Navigate(typeof(ATSHubPage));
        }
        /// <summary>
        /// Method that gets the logged in user's id.
        /// </summary>
        /// <returns>return SessionManager.Instance.UserId</returns>
        private int GetLoggedInUserId()
        {
            return SessionManager.Instance.UserId;
        }
        /// <summary>
        /// Method that generates the device name.
        /// </summary>
        /// <returns>systemId.FriendlyName</returns>
        private string GetDeviceName()
        {
            var systemId = new EasClientDeviceInformation();
            return systemId.FriendlyName;
        }
        /// <summary>
        /// Method that generates the device model.
        /// </summary>
        /// <returns>deviceFamily and deviceName</returns>
        private string GetDeviceModel()
        {
            var deviceFamily = AnalyticsInfo.VersionInfo.DeviceFamily;
            var deviceName = Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
            return $"{deviceFamily} {deviceName}";
        }
        /// <summary>
        /// Method that generates the manufacturer of the device.
        /// </summary>
        /// <returns>deviceInfo.SystemManufacturer</returns>
        private string GetManufacturer()
        {
            var deviceInfo = new EasClientDeviceInformation();
            return deviceInfo.SystemManufacturer;
        }
        /// <summary>
        /// Method that generates the type of the device.
        /// </summary>
        /// <returns>deviceType</returns>
        private string GetDeviceType()
        {
            var deviceType = Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily;
            return deviceType;
        }
        /// <summary>
        /// Method that generates the IP address of the device.
        /// </summary>
        /// <returns>hostName.CanonicalName</returns>
        private string GetIPAddress()
        {
            var hostNames = Windows.Networking.Connectivity.NetworkInformation.GetHostNames();
            foreach (var hostName in hostNames)
            {
                if (hostName.IPInformation != null)
                {
                    return hostName.CanonicalName;
                }
            }
            return "IP Address not found";
        }
    }
}
