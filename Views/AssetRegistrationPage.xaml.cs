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

    public sealed partial class AssetRegistrationPage : Page
    {
        private readonly AssetInteractionModule _assetInteractionModule;
        private DatabaseConHub dbConHub = new DatabaseConHub();

        public AssetRegistrationPage()
        {
            this.InitializeComponent();
            _assetInteractionModule = new AssetInteractionModule(new DatabaseConHub());
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void ChkAddNotes_Checked(object sender, RoutedEventArgs e)
        {
            txtNotes.Visibility = Visibility.Visible;
        }

        private void ChkAddNotes_Unchecked(object sender, RoutedEventArgs e)
        {
            txtNotes.Visibility = Visibility.Collapsed;
        }

        private void ChkAddPurchaseDate_Checked(object sender, RoutedEventArgs e)
        {
            dpPurchaseDate.Visibility = Visibility.Visible;
        }

        private void ChkAddPurchaseDate_Unchecked(object sender, RoutedEventArgs e)
        {
            dpPurchaseDate.Visibility = Visibility.Collapsed;
        }

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

            var asset = new Asset(SessionManager.Instance.UserId, deviceName, deviceModel, manufacturer, type, ipAddress, purchaseDate, notes)
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

        private int GetLoggedInUserId()
        {
            return SessionManager.Instance.UserId;
        }

        private string GetDeviceName()
        {
            var systemId = Windows.System.Profile.SystemIdentification.GetSystemIdForPublisher();
            if (systemId != null && systemId.Id != null)
            {
                return BitConverter.ToString(systemId.Id.ToArray());
            }
            return "Unknown Device";
        }

        private string GetDeviceModel()
        {
            var deviceFamily = AnalyticsInfo.VersionInfo.DeviceFamily;
            var deviceName = Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
            return $"{deviceFamily} {deviceName}";
        }

        private string GetManufacturer()
        {
            var deviceInfo = new EasClientDeviceInformation();
            return deviceInfo.SystemManufacturer;
        }

        private string GetDeviceType()
        {
            var deviceType = Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily;
            return deviceType;
        }

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
