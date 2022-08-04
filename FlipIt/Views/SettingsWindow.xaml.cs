using FlipIt.Controls;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FlipIt.Views
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            cultureTextBox.Text = GlobalData.Culture.ToString() == "" ? "Default" : GlobalData.Culture.ToString();
            timeFormatComboBox.SelectedIndex = (int)GlobalData.TimeFormat;
            dateTimeFormatTextBox.Text = GlobalData.DateTimeFormat;
            visibleSecondsChb.IsChecked = GlobalData.VisibleSeconds;
            visibleDateChb.IsChecked = GlobalData.VisibleDate;
            visibleDayOfWeekChb.IsChecked = GlobalData.VisibleDayOfWeek;
            visibleMonthChb.IsChecked = GlobalData.VisibleMonth;
        }

        private void TimeFormatComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GlobalData.TimeFormat = (TimeFormat)((ComboBox)sender).SelectedIndex;
        }

        private void VisibleSeconds_Click(object sender, RoutedEventArgs e)
        {
            GlobalData.VisibleSeconds = ((CheckBox)sender).IsChecked.GetValueOrDefault();
        }

        private void VisibleDate_Click(object sender, RoutedEventArgs e)
        {
            GlobalData.VisibleDate = ((CheckBox)sender).IsChecked.GetValueOrDefault();
        }

        private void VisibleDayOfWeek_Click(object sender, RoutedEventArgs e)
        {
            GlobalData.VisibleDayOfWeek = ((CheckBox)sender).IsChecked.GetValueOrDefault();
        }

        private void VisibleMonth_Click(object sender, RoutedEventArgs e)
        {
            GlobalData.VisibleMonth = ((CheckBox)sender).IsChecked.GetValueOrDefault();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cultureTextBox.Text != "Default") GlobalData.Culture = new CultureInfo(cultureTextBox.Text);
                GlobalData.DateTimeFormat = dateTimeFormatTextBox.Text;
                GlobalData.Save();
                Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            GlobalData.Reset();
            Init();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Accept_Click(sender, e);
            else if (e.Key == Key.Escape) Close();
        }
    }
}