using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using Microsoft.Win32;

namespace Timer
{
    public partial class SettingsWindow : MetroWindow
    {
        private MainWindow _mainWindow;

        public SettingsWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            LoadSettings();
        }

        private void LoadSettings()
        {
            string savedLanguage = Properties.Settings.Default.AppLanguage;
            SetLanguage(savedLanguage);
            foreach (ComboBoxItem item in settingsLanguage.Items)
            {
                if (item.Tag.ToString() == savedLanguage)
                {
                    settingsLanguage.SelectedItem = item;
                    break;
                }
            }

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings["Topmost"].Value == "1")
                settingsTopmost.IsChecked = true;
            if (config.AppSettings.Settings["SavedAction"].Value != null && config.AppSettings.Settings["SavedAction"].Value != "")
                settingsLastType.IsChecked = true;
            if (config.AppSettings.Settings["SavedTimer"].Value != null && config.AppSettings.Settings["SavedTimer"].Value != "")
                settingsLastTimer.IsChecked = true;
            if (config.AppSettings.Settings["Autorun"].Value != null && config.AppSettings.Settings["Autorun"].Value != "")
                settingsAutorun.IsChecked = true;
            if (config.AppSettings.Settings["BackgroundWork"].Value != null && config.AppSettings.Settings["BackgroundWork"].Value != "")
                settingsBackground.IsChecked = true;
        }

        private void SetLanguage(string languageCode)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageCode);
            settingsAutorun.Content = Properties.Resources.SettingsAutorun;
            settingsBackground.Content = Properties.Resources.SettingsBackground;
            settingsDeleteTimer.Content = Properties.Resources.SettingsDeleteTimer;
            settingsLanguageText.Text = Properties.Resources.SettingsLanguageText;
            settingsLastTimer.Content = Properties.Resources.SettingsLastTimer;
            settingsLastType.Content = Properties.Resources.SettingsLastType;
            settingsTopmost.Content = Properties.Resources.SettingsTopmost;
            _mainWindow.mainTitle.Text = Properties.Resources.MainTitle;
            _mainWindow.timerStartButton.Content = Properties.Resources.MainStart;
            _mainWindow.timerCancelButton.Content = Properties.Resources.MainCancel;
            _mainWindow.mainHoursText.Text = Properties.Resources.MainHours;
            _mainWindow.mainMinutesText.Text = Properties.Resources.MainMinutes;
            _mainWindow.mainShutdown.Content = Properties.Resources.MainShutdown;
            _mainWindow.mainRestart.Content = Properties.Resources.MainRestart;
            _mainWindow.mainSleep.Content = Properties.Resources.MainSleep;
            Properties.Settings.Default.AppLanguage = languageCode;
            Properties.Settings.Default.Save();
        }

        private void CheckboxLastTimerClick(object sender, RoutedEventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (settingsLastTimer.IsChecked == true) config.AppSettings.Settings["SavedTimer"].Value = "00:00";
            else config.AppSettings.Settings["SavedTimer"].Value = null;
            config.Save(ConfigurationSaveMode.Modified);
        }

        private void CheckboxLastTypeClick(object sender, RoutedEventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (settingsLastType.IsChecked == true) config.AppSettings.Settings["SavedAction"].Value = "2";
            else config.AppSettings.Settings["SavedAction"].Value = null;
            config.Save(ConfigurationSaveMode.Modified);
        }

        private void CheckboxDeleteTimerClick(object sender, RoutedEventArgs e)
        {

        }

        private void CheckboxAutorunClick(object sender, RoutedEventArgs e)
        {
            string appName = "Power Timer";
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (settingsAutorun.IsChecked == true)
            {
                config.AppSettings.Settings["Autorun"].Value = "1";
                
                string appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                try
                {
                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
                    {
                        key?.SetValue(appName, appPath);
                    }
                }
                catch { }
            }
            else
            {
                config.AppSettings.Settings["Autorun"].Value = null;
                try
                {
                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
                    {
                        if (key != null && key.GetValue(appName) != null) key.DeleteValue(appName);
                    }
                }
                catch { }
            }
            config.Save(ConfigurationSaveMode.Modified);
        }

        private void CheckboxBackgroundClick(object sender, RoutedEventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (settingsBackground.IsChecked == true)
            {
                config.AppSettings.Settings["BackgroundWork"].Value = "1";
            }
            else
            {
                config.AppSettings.Settings["BackgroundWork"].Value = null;
            }
            config.Save(ConfigurationSaveMode.Modified);
        }

        private void CheckboxTopmostClick(object sender, RoutedEventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (settingsTopmost.IsChecked == true)
            {
                config.AppSettings.Settings["Topmost"].Value = "1";
                _mainWindow.Topmost = true;
            }
            else
            {
                config.AppSettings.Settings["Topmost"].Value = "0";
                _mainWindow.Topmost = false;
            }
            config.Save(ConfigurationSaveMode.Modified);
        }

        private void ComboboxLanguageChange(object sender, SelectionChangedEventArgs e)
        {
            if (settingsLanguage.SelectedItem is ComboBoxItem selectedItem)
            {
                SetLanguage(selectedItem.Tag.ToString());
            }
        }
    }
}
