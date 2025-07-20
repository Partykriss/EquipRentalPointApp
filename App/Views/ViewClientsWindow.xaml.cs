using EquipRentalPointApp.Models;
using System;
using System.Windows;

namespace EquipRentalPointApp.Views
{
    public partial class ViewClientsWindow : Window
    {
        private static ViewClientsWindow instance;

        public ViewClientsWindow()
        {
            InitializeComponent();
            ClientsVeiw.ItemsSource = new DataWorker().GetAllClients();
            Closing += ViewClientsWindow_Closing;
            AddNewClientWindow.GetInstance().ClientAdded += ViewClientsWindow_ClientAdded;
        }

        private void ViewClientsWindow_ClientAdded(object? sender, EventArgs e)
        {
            ClientsVeiw.ItemsSource = new DataWorker().GetAllClients();
        }

        public static ViewClientsWindow GetInstance()
        {
            if (instance == null)
                instance = new ViewClientsWindow();
            return instance;
        }

        private void ViewClientsWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void AddNewClient_Click(object sender, RoutedEventArgs e)
        {
            var addNewClientWindow = AddNewClientWindow.GetInstance();
            addNewClientWindow.Owner = this;
            addNewClientWindow.Show();
        }
    }
}
