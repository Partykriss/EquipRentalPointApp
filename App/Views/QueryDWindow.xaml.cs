using EquipRentalPointApp.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace EquipRentalPointApp.Views
{
    public partial class QueryDWindow : Window
    {
        private static QueryDWindow instance;
        public QueryDWindow()
        {
            InitializeComponent();
            Closing += QueryDWindow_Closing;
        }

        private void QueryDWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        public static QueryDWindow GetInstance()
        {
            if (instance == null)
                instance = new QueryDWindow();
            return instance;
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            CalendarButton.IsEnabled = true;
        }

        private void CalendarButton_Click(object sender, RoutedEventArgs e)
        {
            AnswerQueryD.ItemsSource = null;
            DateTime dateBegin = Calendar.SelectedDate.GetValueOrDefault(DateTime.Now);
            DateTime dateEnd = new DateTime();
            if (Calendar.SelectedDates.Count == 1)
                dateEnd = Calendar.SelectedDate.GetValueOrDefault(DateTime.Now).AddMonths(1);
            else
                dateEnd = Calendar.SelectedDate.GetValueOrDefault(DateTime.Now).AddDays(Calendar.SelectedDates.Count - 1);
            AnswerQueryD.ItemsSource = new DataWorker().AnswerQueryD(dateBegin, dateEnd);
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}
