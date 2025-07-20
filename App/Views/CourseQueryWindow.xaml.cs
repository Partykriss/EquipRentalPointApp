using System;
using System.Collections.Generic;
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
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;

namespace EquipRentalPointApp.Views
{
    /// <summary>
    /// Логика взаимодействия для CourseQuery.xaml
    /// </summary>
    public partial class CourseQueryWindow : Window
    {
        private static CourseQueryWindow instance;

        public CourseQueryWindow()
        {
            InitializeComponent();
            Closing += CourseQueryWindow_Closing;
        }

        private void CourseQueryWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        public static CourseQueryWindow GetInstance()
        {
            if (instance == null)
                instance = new CourseQueryWindow();
            return instance;
        }

        private void QueryA_Click(object sender, RoutedEventArgs e)
        {
            var queryAWindow = QueryAWindow.GetInstance();
            queryAWindow.Owner = this;
            queryAWindow.Show();
        }

        private void QueryB_Click(object sender, RoutedEventArgs e)
        {
            var queryBWindow = QueryBWindow.GetInstance();
            queryBWindow.Owner = this;
            queryBWindow.Show();
        }

        private void QueryC_Click(object sender, RoutedEventArgs e)
        {
            var queryCWindow = QueryCWindow.GetInstance();
            queryCWindow.Owner = this;
            queryCWindow.Show();
        }

        private void QueryD_Click(object sender, RoutedEventArgs e)
        {
            var queryDWindow = QueryDWindow.GetInstance();
            queryDWindow.Owner = this;
            queryDWindow.Show();
        }
    }
}
