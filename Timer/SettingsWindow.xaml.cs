using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
            string language = ConfigurationManager.AppSettings["Language"];
            if (language.Equals("Русский"))
            {
                settingsLanguage.SelectedIndex = 0;
            }
            else if (language.Equals("English"))
            {
                settingsLanguage.SelectedIndex = 1;
            }
        }

        private void SaveLanguage()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["Language"].Value = settingsLanguage.Text;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            MessageBox.Show(settingsLanguage.Text);
            MessageBox.Show(ConfigurationManager.AppSettings["Language"]);
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
            SaveLanguage();
        }
    }
}
