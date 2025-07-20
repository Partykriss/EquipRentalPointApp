using EquipRentalPointApp.Models;
using System.Windows;
using System.Windows.Controls;

namespace EquipRentalPointApp.Views
{
    public partial class QueryAWindow : Window
    {
        private static QueryAWindow instance;

        public QueryAWindow()
        {
            InitializeComponent();
            CategoriesList.ItemsSource = new DataWorker().GetAllCategories();
            Closing += QueryAWindow_Closing;
        }

        private void QueryAWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        public static QueryAWindow GetInstance()
        {
            if (instance == null)
                instance = new QueryAWindow();
            return instance;
        }

        private void ShowAnswerQueryA_Click(object sender, RoutedEventArgs e)
        {
            AnswerQueryA.Items.Clear();
            AnswerQueryA.Items.Add(new DataWorker().AnswerQueryA((Category)CategoriesList.SelectedItem));
        }

        private void CategoriesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ButtonQueryA.IsEnabled = true;
            LabelQueryA.IsEnabled = true;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}
