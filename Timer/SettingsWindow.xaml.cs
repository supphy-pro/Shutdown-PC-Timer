using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;

namespace Timer
{
    public partial class SettingsWindow : MetroWindow
    {
        public SettingsWindow()
        {
            InitializeComponent();
            LoadLanguage();
        }

        private void LoadLanguage()
        {
            string currentLanguage = ConfigurationManager.AppSettings["Language"];
            var matchingItem = settingsLanguage.Items
                .Cast<ComboBoxItem>()
                .FirstOrDefault(item => (string)item.Content == currentLanguage);
            if (matchingItem != null)
                settingsLanguage.SelectedItem = matchingItem;
        }

        private void SaveLanguage(string _language)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["Language"].Value = _language;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void CheckboxLastTimerClick(object sender, RoutedEventArgs e)
        {

        }

        private void CheckboxLastTypeClick(object sender, RoutedEventArgs e)
        {

        }

        private void CheckboxDeleteTimerClick(object sender, RoutedEventArgs e)
        {

        }

        private void CheckboxAutorunClick(object sender, RoutedEventArgs e)
        {

        }

        private void CheckboxBackgroundClick(object sender, RoutedEventArgs e)
        {

        }

        private void CheckboxTopmostClick(object sender, RoutedEventArgs e)
        {

        }

        private void ComboboxLanguageChange(object sender, SelectionChangedEventArgs e)
        {
            if (settingsLanguage.SelectedItem is ComboBoxItem selectedItem)
                SaveLanguage(selectedItem.Content.ToString());
        }
    }
}
