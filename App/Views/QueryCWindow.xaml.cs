using EquipRentalPointApp.Models;
using System.Windows;

namespace EquipRentalPointApp.Views
{
    public partial class QueryCWindow : Window
    {
        public static QueryCWindow instance;

        public QueryCWindow()
        {
            InitializeComponent();
            Closing += QueryCWindow_Closing;
            AnswerQueryC.ItemsSource = new DataWorker().AnswerQueryC();
        }

        public static QueryCWindow GetInstance()
        {
            if (instance == null)
                instance = new QueryCWindow();
            return instance;
        }

        private void QueryCWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}
