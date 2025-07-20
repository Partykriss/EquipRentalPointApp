using System;
using System.Data.SqlClient;
using System.Windows;

namespace EquipRentalPointApp.Views
{
    public partial class MainWindow : Window
    {
        SqlConnection connection = DatabaseConnection.Instance.GetConnection();

        public MainWindow()
        {
            InitializeComponent();
            //Topmost = true;
            using (connection)
            {
                try
                {
                    connection.Open();
                    MessageBox.Show("Connection opened to " + connection.Database);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                connection.Close();
            }
            Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (var window in App.OpenedWindows)
            {
                window.Close();
            }
        }

        private void EquipmentRentalPoint_Click(object sender, RoutedEventArgs e)
        {
            var equipmentRentalPointWindow = EquipmentRentalPointWindow.GetInstance();
            equipmentRentalPointWindow.Owner = this;
            equipmentRentalPointWindow.Show();
        }

        private void CourseQueries_Click(object sender, RoutedEventArgs e)
        {
            var courseQueryWindow = CourseQueryWindow.GetInstance();
            courseQueryWindow.Owner = this;
            courseQueryWindow.Show();
        }
    }
}
