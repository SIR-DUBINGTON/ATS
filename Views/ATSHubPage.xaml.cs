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

    public sealed partial class ATSHubPage : Page
    {
        public ATSHubPage()
        {
            this.InitializeComponent();
        }

        private void AssetRegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AssetRegistrationPage));
        }

        private void AssetManagementButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AssetManagementPage));
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }
    }
}
