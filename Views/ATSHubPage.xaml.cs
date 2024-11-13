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


namespace ATS.Views
{
    /// <summary>
    /// Class for the ATSHubPage
    /// </summary>
    public sealed partial class ATSHubPage : Page
    {
        /// <summary>
        /// Initializes the ATSHubPage
        /// </summary>
        public ATSHubPage()
        {
            this.InitializeComponent();
        }
        /// <summary>
        /// Method for the AssetRegistrationButton click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HardwareAssetRegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(HardwareAssetRegistrationPage));
        }
        /// <summary>
        /// Method for the AssetManagementButton click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AssetManagementButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AssetManagementPage));
        }
        /// <summary>
        /// Method for the ExitButton click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        private void SoftwareAssetRegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SoftwareAssetRegistrationPage));
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }
    }
}
