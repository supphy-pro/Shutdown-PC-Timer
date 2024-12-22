using System;
using System.Configuration;
using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using MahApps.Metro.Controls;

namespace Timer
{
    public partial class MainWindow : MetroWindow
    {
        SettingsWindow settingsWindow;
        private System.Timers.Timer countdownTimer;
        private DateTime? endTime;
        public MainWindow()
        {
            InitializeComponent();
            LoadTimerData();
            StartCountdown();
            LoadSettings();
        }

        private void SaveTimerData()
        {
            if (endTime.HasValue || endTime == null)
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["EndTime"].Value = endTime != null ? endTime.Value.ToString("o") : null;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }

        private void LoadTimerData()
        {
            string savedTime = ConfigurationManager.AppSettings["EndTime"];
            if (DateTime.TryParse(savedTime, out DateTime parsedTime))
            {
                if (parsedTime > DateTime.Now)
                {
                    endTime = parsedTime;
                }
            }
        }

        private void SaveSettings()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            string hours = timerHours.Value < 10 ? $"0{(int)timerHours.Value}" : ((int)timerHours.Value).ToString();
            string minutes = timerMinutes.Value < 10 ? $"0{(int)timerMinutes.Value}" : ((int)timerMinutes.Value).ToString();
            config.AppSettings.Settings["SavedTimer"].Value = $"{hours}:{minutes}";
            config.Save(ConfigurationSaveMode.Modified);
            config.AppSettings.Settings["SavedAction"].Value = timerAction.Text;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void LoadSettings()
        {
            string savedTimer = ConfigurationManager.AppSettings["SavedTimer"];
            string savedAction = ConfigurationManager.AppSettings["SavedAction"];
            if (savedTimer != "" && savedTimer != null)
            {
                timerHours.Value = double.Parse(savedTimer.Split(':')[0]);
                timerMinutes.Value = double.Parse(savedTimer.Split(':')[1]);
            }
            if (savedAction != "" && savedTimer != null)
            {
                timerAction.Text = savedTimer;
            }
        }

        private void StartCountdown()
        {
            if (endTime == null) return;

            countdownTimer = new System.Timers.Timer(1000);
            countdownTimer.Elapsed += (s, e) =>
            {
                var remainingTime = endTime - DateTime.Now;

                if (remainingTime <= TimeSpan.Zero)
                {
                    countdownTimer.Stop();
                    Dispatcher.Invoke(() => timerTime.Text = "00:00:00");
                }
                else
                {
                    Dispatcher.Invoke(() =>
                    {
                        timerTime.Text = $"{remainingTime:hh\\:mm\\:ss}";
                    });
                }
            };
            countdownTimer.Start();
        }

        private int GetMinutesFromInput()
        {
            int minutes = 0;
            if (timerHours.Value > 0)
                minutes = (int)timerHours.Value * 60;
            if (timerMinutes.Value > 0)
                minutes += (int)timerMinutes.Value;
            if (minutes > 0)
                return minutes;
            MessageBox.Show("Введите корректное положительное число минут.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            throw new InvalidOperationException("Некорректное время.");
        }

        private void SettingsOpenClick(object sender, RoutedEventArgs e)
        {
            settingsWindow = new SettingsWindow(this);
            settingsWindow.ShowDialog();
        }

        private void StartTimerClick(object sender, RoutedEventArgs e)
        {
            if (!timerAction.Text.Equals("") && (timerHours.Value > 0 || timerMinutes.Value > 0))
            {
                int minutes = GetMinutesFromInput();
                if (timerAction.Text.Equals("Завершение работы"))
                {
                    ScheduleTask("ShutdownTask", "shutdown /s /t 0", minutes);
                }
                else if (timerAction.Text.Equals("Перезагрузка"))
                {
                    ScheduleTask("RestartTask", "shutdown /r /t 0", minutes);
                }
                else
                {
                    ScheduleTask("SleepTask", "rundll32.exe powrprof.dll,SetSuspendState Sleep", minutes);
                }
                timerStartButton.Visibility = Visibility.Collapsed;
                timerCancelButton.Visibility = Visibility.Visible;
                timerHours.IsEnabled = false;
                timerMinutes.IsEnabled = false;
                timerAction.IsEnabled = false;
                endTime = DateTime.Now.AddMinutes(minutes);
                SaveTimerData();
                StartCountdown();
                SaveSettings();
            }
            else
            {
                if (timerAction.Text.Equals(""))
                    timerAction.Focus();
                else
                    timerHours.Focus();
            }
        }

        private void CancelTimerClick(object sender, RoutedEventArgs e)
        {
            try
            {
                CancelTask("ShutdownTask");
                CancelTask("RestartTask");
                CancelTask("SleepTask");
                timerStartButton.Visibility = Visibility.Visible;
                timerCancelButton.Visibility = Visibility.Collapsed;
                endTime = null;
                SaveTimerData();
                string hours = "00";
                string minutes = "00";
                if (timerHours.Value > 0)
                {
                    hours = timerHours.Value < 10 ? "0" + timerHours.Value.ToString() : timerHours.Value.ToString();
                }
                if (timerMinutes.Value > 0)
                {
                    minutes = timerMinutes.Value < 10 ? "0" + timerMinutes.Value.ToString() : timerMinutes.Value.ToString();
                }
                timerTime.Text = $"{hours}:{minutes}:00";
                countdownTimer?.Stop();
                timerHours.IsEnabled = true;
                timerMinutes.IsEnabled = true;
                timerAction.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при отмене задач: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelTask(string taskName)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "schtasks",
                    Arguments = $"/Delete /TN \"{taskName}\" /F",
                    CreateNoWindow = true,
                    UseShellExecute = false
                }
            };

            process.Start();
            process.WaitForExit();
        }

        private void ScheduleTask(string taskName, string command, int minutes)
        {
            try
            {
                var startTime = DateTime.Now.AddMinutes(minutes);
                string arguments = $"/Create /TN \"{taskName}\" /TR \"{command}\" /SC ONCE /ST {startTime:HH:mm} /F";

                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "schtasks",
                        Arguments = arguments,
                        CreateNoWindow = true,
                        UseShellExecute = false
                    }
                };

                process.Start();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при планировании задачи: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ChangeHoursValue(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (timerHours.IsInitialized)
            {
                string hours = timerHours.Value < 10 ? $"0{timerHours.Value}" : timerHours.Value.ToString();
                if (hours.Equals(""))
                    hours = "00";
                timerTime.Text = $"{hours}:{timerTime.Text.Split(':')[1]}:00";
            }
        }

        private void ChangeMinutesValue(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            
            if (timerMinutes.IsInitialized)
            {
                string minutes = timerMinutes.Value < 10 ? $"0{timerMinutes.Value}" : timerMinutes.Value.ToString();
                if (minutes.Equals(""))
                    minutes = "00";
                timerTime.Text = $"{timerTime.Text.Split(':')[0]}:{minutes}:00";
            }
        }
    }
}
