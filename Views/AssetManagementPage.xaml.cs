using ATS.DataAccess;
using ATS.Models;
using ATS.ViewModels;
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
    /// <summary>
    /// Class that represents the Asset Management Page.
    /// </summary>
    public sealed partial class AssetManagementPage : Page
    {
        /// <summary>
        /// Instance of the AssetInteractionModule class. 
        /// </summary>
        private readonly AssetInteractionModule _assetInteractionModule;
        private List<Asset> _allAssets;

        /// <summary>
        /// Constructor for the AssetManagementPage class.
        /// </summary>
        public AssetManagementPage()
        {
            this.InitializeComponent();
            _assetInteractionModule = new AssetInteractionModule(new DatabaseConHub());
            LoadAssets();
        }
        /// <summary>
        /// Method that loads the assets for the current user.
        /// </summary>
        private void LoadAssets()
        {
            int userId = SessionManager.Instance.UserId;
            _allAssets = _assetInteractionModule.GetAssetsForUser(userId, string.Empty);
            lstAssets.ItemsSource = _allAssets;
        }
        /// <summary>
        /// Method that handles the search text changed event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            FilterAssets();
        }
        /// <summary>
        /// Method that handles the search button click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            FilterAssets();
        }
        /// <summary>
        /// Method that filters the assets based on the search query and filter.
        /// </summary>
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
        /// <summary>
        /// Method that handles the delete button click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag == null || !int.TryParse(button.Tag.ToString(), out int assetId))
            {
                await new MessageDialog("Failed to delete: Asset ID not found.").ShowAsync();
                return;
            }

            var dialog = new MessageDialog("Are you sure you want to delete this asset?", "Confirm Delete");
            dialog.Commands.Add(new UICommand("Yes", command =>
            {
                _assetInteractionModule.DeleteAsset(assetId);
                LoadAssets();
            }));
            dialog.Commands.Add(new UICommand("No"));
            await dialog.ShowAsync();
        }
        /// <summary>
        /// Method that handles the edit button click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag == null || !int.TryParse(button.Tag.ToString(), out int assetId))
            {
                await new MessageDialog("Failed to edit: Asset ID not found.").ShowAsync();
                return;
            }

            var asset = _allAssets.FirstOrDefault(a => a.id == assetId);
            if (asset != null)
            {
                await ShowEditDialog(asset);
            }
        }
        /// <summary>
        /// Method that shows the edit dialog for the asset.
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        private async Task ShowEditDialog(Asset asset)
        {
            var dialog = new ContentDialog
            {
                Title = "Edit Asset Details",
                PrimaryButtonText = "Save",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary
            };

            var stackPanel = new StackPanel();

            // Purchase Date input
            var datePicker = new DatePicker
            {
                Date = asset.purchaseDate
            };
            stackPanel.Children.Add(new TextBlock { Text = "Purchase Date" });
            stackPanel.Children.Add(datePicker);

            // Text Notes input
            var notesBox = new TextBox
            {
                Text = asset.textNotes,
                AcceptsReturn = true
            };
            stackPanel.Children.Add(new TextBlock { Text = "Text Notes" });
            stackPanel.Children.Add(notesBox);

            dialog.Content = stackPanel;

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                _assetInteractionModule.UpdateAssetDetails(asset.id, datePicker.Date.DateTime, notesBox.Text);
                LoadAssets();
            }
        }
        /// <summary>
        /// Method that handles the back button click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
