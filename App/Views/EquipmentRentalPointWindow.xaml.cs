using EquipRentalPointApp.Models;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EquipRentalPointApp.Views
{
    public partial class EquipmentRentalPointWindow : Window
    {
        private static EquipmentRentalPointWindow instance;
        public EquipmentRentalPointWindow()
        {
            InitializeComponent();
            EquipmentsList.ItemsSource = new DataWorker().GetAllEquipments();
            Closing += EquipmentRentalPointWindow_Closing;
            AddNewEquipmentWindow.GetInstance().EquipmentAdded += AddNewEquipmentWindow_EquipmentAdded;
        }

        private void EquipmentRentalPointWindow_Closing(object? sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        public static EquipmentRentalPointWindow GetInstance()
        {
            if (instance == null)
                instance = new EquipmentRentalPointWindow();
            return instance;
        }


        private void AddNewEquipment_Click(object sender, RoutedEventArgs e)
        {
            var addNewEquipmentWindow = AddNewEquipmentWindow.GetInstance();
            addNewEquipmentWindow.Owner = this;
            addNewEquipmentWindow.Show();
        }

        private void AddNewEquipmentWindow_EquipmentAdded(object sender, EventArgs e)
        {
            EquipmentsList.ItemsSource = new DataWorker().GetAllEquipments();
        }

        private void AddNewRental_Click(object sender, RoutedEventArgs e)
        {
            if (EquipmentsList.SelectedItems.Count > 0)
            {
                var selectedEquipments = EquipmentsList.SelectedItems.Cast<Equipment>().ToList();
                AddNewRentalWindow addNewRentalWindow = new AddNewRentalWindow(selectedEquipments);
                addNewRentalWindow.Owner = this;
                addNewRentalWindow.Show();
            }
        }

        private void Clients_Click(object sender, RoutedEventArgs e)
        {
            var clientsWindow = ViewClientsWindow.GetInstance();
            clientsWindow.Owner = this;
            clientsWindow.Show();
        }

        private void AddNewPayment_Click(object sender, RoutedEventArgs e)
        {
            MakePaymentWindow makePaymentWindow = new MakePaymentWindow();
            makePaymentWindow.Owner = this;
            makePaymentWindow.Show();
        }

        private void EquipmentsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EquipmentsList.SelectedItems.Count > 5)
            {
                EquipmentsList.SelectedItems.RemoveAt(0);
            }
        }
    }
}
