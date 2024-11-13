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
        private readonly HardwareAssetInteractionModule _hardwareAssetInteractionModule;
        private readonly SoftwareAssetInteractionModule _softwareAssetInteractionModule;

        private List<HardwareAsset> _hardwareAssets;
        private List<SoftwareAsset> _softwareAssets;
        /// <summary>
        /// Constructor for the AssetManagementPage class.
        /// </summary>
        public AssetManagementPage()
        {
            this.InitializeComponent();
            _hardwareAssetInteractionModule = new HardwareAssetInteractionModule(new DatabaseConHub());
            _softwareAssetInteractionModule = new SoftwareAssetInteractionModule(new DatabaseConHub());

            LoadAssets();
        }
        /// <summary>
        /// Method that loads the assets for the current user.
        /// </summary>
        private void LoadAssets()
        {
            int userId = SessionManager.Instance.UserId;
            _hardwareAssets = _hardwareAssetInteractionModule.GetAssetsForUser(userId, string.Empty);
            _softwareAssets = _softwareAssetInteractionModule.GetSoftwareAssetsForUser(userId, string.Empty);
            FilterAssets();
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
        private void OnTypeFilterChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateFilterOptions();
            FilterAssets();
        }
        private void UpdateFilterOptions()
        {
            cmbFilter.Items.Clear();

            var selectedType = (cmbTypeFilter.SelectedItem as ComboBoxItem)?.Content?.ToString();
            if (selectedType == "Hardware")
            {
                cmbFilter.Items.Add(new ComboBoxItem { Content = "Name" });
                cmbFilter.Items.Add(new ComboBoxItem { Content = "Model" });
                cmbFilter.Items.Add(new ComboBoxItem { Content = "Manufacturer" });
                cmbFilter.Items.Add(new ComboBoxItem { Content = "Type" });
                cmbFilter.Items.Add(new ComboBoxItem { Content = "IP" });
                cmbFilter.Items.Add(new ComboBoxItem { Content = "PurchaseDate" });
                cmbFilter.Items.Add(new ComboBoxItem { Content = "TextNote" });
            }
            else if (selectedType == "Software")
            {
                cmbFilter.Items.Add(new ComboBoxItem { Content = "OS Name" });
                cmbFilter.Items.Add(new ComboBoxItem { Content = "OS Version" });
                cmbFilter.Items.Add(new ComboBoxItem { Content = "Manufacturer" });
            }
        }
        private void FilterAssets()
        {
            string searchQuery = txtSearch.Text.ToLower();
            string selectedFilter = (cmbFilter.SelectedItem as ComboBoxItem)?.Content?.ToString();
            string selectedType = (cmbTypeFilter.SelectedItem as ComboBoxItem)?.Content?.ToString();

            if (selectedType == "Hardware")
            {
                var filteredAssets = _hardwareAssets.Where(asset =>
                {
                    switch (selectedFilter)
                    {
                        case "Name": return asset.name.ToLower().Contains(searchQuery);
                        case "Model": return asset.model.ToLower().Contains(searchQuery);
                        case "Manufacturer": return asset.manufacturer.ToLower().Contains(searchQuery);
                        case "Type": return asset.type.ToLower().Contains(searchQuery);
                        case "IP": return asset.ip.ToLower().Contains(searchQuery);
                        case "PurchaseDate": return asset.purchaseDate.ToString().Contains(searchQuery);
                        case "TextNote": return asset.textNotes.ToLower().Contains(searchQuery);
                        default: return asset.name.ToLower().Contains(searchQuery);
                    }
                }).ToList();

                lstAssets.ItemsSource = filteredAssets;
            }
            else if (selectedType == "Software")
            {
                var filteredAssets = _softwareAssets.Where(asset =>
                {
                    switch (selectedFilter)
                    {
                        case "OS Name": return asset.osName.ToLower().Contains(searchQuery);
                        case "OS Version": return asset.osVersion.ToLower().Contains(searchQuery);
                        case "Manufacturer": return asset.manufacturer.ToLower().Contains(searchQuery);
                        default: return asset.osName.ToLower().Contains(searchQuery);
                    }
                }).ToList();
                lstAssets.ItemsSource = filteredAssets;
            }
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
                _hardwareAssetInteractionModule.DeleteAsset(assetId);
                LoadAssets();
            }));
            dialog.Commands.Add(new UICommand("No"));
            await dialog.ShowAsync();
        }
        private async void DeleteButton_ClickSoftware(object sender, RoutedEventArgs e)
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
                _softwareAssetInteractionModule.DeleteSoftwareAsset(assetId, SessionManager.Instance.UserId);
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

            var asset = _hardwareAssets.FirstOrDefault(a => a.id == assetId);
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
        private async Task ShowEditDialog(HardwareAsset asset)
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
                _hardwareAssetInteractionModule.UpdateAssetDetails(asset.id, datePicker.Date.DateTime, notesBox.Text);
                LoadAssets();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        /// <summary>
        /// Method that handles the back button click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void EditButton_ClickSoftware(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag == null || !int.TryParse(button.Tag.ToString(), out int assetId))
            {
                await new MessageDialog("Failed to edit: Asset ID not found.").ShowAsync();
                return;
            }

            var asset = _softwareAssets.FirstOrDefault(a => a.id == assetId);
            if (asset != null)
            {
                await ShowEditDialogSoftware(asset);
            }
        }

        private async Task ShowEditDialogSoftware(SoftwareAsset asset)
        {
            var dialog = new ContentDialog
            {
                Title = "Edit Software Asset Details",
                PrimaryButtonText = "Save",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary
            };

            var stackPanel = new StackPanel();

            // OS Name input
            var osNameBox = new TextBox
            {
                Text = asset.osName,
                AcceptsReturn = true
            };
            stackPanel.Children.Add(new TextBlock { Text = "OS Name" });
            stackPanel.Children.Add(osNameBox);

            // OS Version input
            var osVersionBox = new TextBox
            {
                Text = asset.osVersion,
                AcceptsReturn = true
            };
            stackPanel.Children.Add(new TextBlock { Text = "OS Version" });
            stackPanel.Children.Add(osVersionBox);

            // Manufacturer input
            var manufacturerBox = new TextBox
            {
                Text = asset.manufacturer,
                AcceptsReturn = true
            };
            stackPanel.Children.Add(new TextBlock { Text = "Manufacturer" });
            stackPanel.Children.Add(manufacturerBox);

            dialog.Content = stackPanel;

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                // Update the asset properties with the new values
                asset.osName = osNameBox.Text;
                asset.osVersion = osVersionBox.Text;
                asset.manufacturer = manufacturerBox.Text;

                _softwareAssetInteractionModule.EditSoftwareAsset(asset);
                LoadAssets();
            }
        }

    }
    /// <summary>
    /// DataTemplateSelector class to choose between displaying the HardwareAsset template or SoftwareAsset template.
    /// </summary>
    public class AssetTemplateSelector : DataTemplateSelector
    {
        public DataTemplate HardwareAssetTemplate { get; set; }
        public DataTemplate SoftwareAssetTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is HardwareAsset)
            {
                return HardwareAssetTemplate;
            }
            else if (item is SoftwareAsset)
            {
                return SoftwareAssetTemplate;
            }

            return base.SelectTemplateCore(item, container);
        }
    }
}