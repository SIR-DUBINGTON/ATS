using ATS.DataAccess;
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


namespace ATS.Views
{
    public sealed partial class AssetManagementPage : Page
    {
        private readonly AssetInteractionModule _assetInteractionModule;
        private List<Asset> _allAssets;
        public AssetManagementPage()
        {
            this.InitializeComponent();
            _assetInteractionModule = new AssetInteractionModule(new DatabaseConHub());
            LoadAssets();
        }

        private void LoadAssets()
        {
            int userId = SessionManager.Instance.UserId;
            _allAssets = _assetInteractionModule.GetAssetsForUser(userId, string.Empty);
            lstAssets.ItemsSource = _allAssets;
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            FilterAssets();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            FilterAssets();
        }

        private void FilterAssets()
        {
            string searchQuery = txtSearch.Text.ToLower();
            string selectedFilter = (cmbFilter.SelectedItem as ComboBoxItem)?.Content?.ToString();

            var filteredAssets = _allAssets.Where(asset =>
            {
                switch (selectedFilter)
                {
                    case "Name":
                        return asset.name.ToLower().Contains(searchQuery);
                    case "Model":
                        return asset.model.ToLower().Contains(searchQuery);
                    case "Manufacturer":
                        return asset.manufacturer.ToLower().Contains(searchQuery);
                    case "Type":
                        return asset.type.ToLower().Contains(searchQuery);
                    case "IP":
                        return asset.ip.ToLower().Contains(searchQuery);
                    case "PurchaseDate":
                        return asset.purchaseDate.ToString().Contains(searchQuery);
                    case "TextNote":
                        return asset.textNotes.ToLower().Contains(searchQuery);
                    default:
                        return asset.name.ToLower().Contains(searchQuery);
                }
            }).ToList();

            lstAssets.ItemsSource = filteredAssets;
        }

    }
}
