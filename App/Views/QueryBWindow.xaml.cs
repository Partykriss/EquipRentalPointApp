using EquipRentalPointApp.Models;
using System.Windows;

namespace EquipRentalPointApp.Views
{
    public partial class QueryBWindow : Window
    {
        private static QueryBWindow instance;

        public QueryBWindow()
        {
            InitializeComponent();
            Closing += QueryBWindow_Closing;
            AnswerQueryB.ItemsSource = new DataWorker().AnswerQueryB();
        }

        private void QueryBWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        public static QueryBWindow GetInstance()
        {
            if (instance == null)
                instance = new QueryBWindow();
            return instance;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}
