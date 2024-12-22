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

namespace Timer
{
    public partial class SettingsWindow : MetroWindow
    {
        private MainWindow _mainWindow;

        public SettingsWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            LoadLanguage();
        }

        private void LoadLanguage()
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
            {
                SetLanguage(selectedItem.Tag.ToString());
            }
        }
    }
}
