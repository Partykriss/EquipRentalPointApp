using EquipRentalPointApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EquipRentalPointApp.Views
{
    public partial class MakePaymentWindow : Window
    {
        private static List<Rental> rentals;
        public MakePaymentWindow()
        {
            InitializeComponent();
            rentals = new DataWorker().GetAllRentalsNotPayed();
            RentalsList.ItemsSource = rentals;
        }

        private void OnRentalPayed(object sender, EventArgs e)
        {
            rentals = new DataWorker().GetAllRentalsNotPayed();
            RentalsList.ItemsSource = rentals;
        }

        private void Filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (rentals != null)
            {
                var filteredList = rentals.Where(rental =>
                    rental.Client.FullName.Contains(NameFilter.Text, StringComparison.OrdinalIgnoreCase) &&
                    rental.Client.Phone.Contains(PhoneFilter.Text, StringComparison.OrdinalIgnoreCase)).ToList();
                RentalsList.ItemsSource = filteredList;
            }
        }

        private void RentalsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AddPaymentButton.IsEnabled = true;
        }

        private void AddPaymentButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewPaymentWindow addNewPaymentWindow = new AddNewPaymentWindow((Rental)RentalsList.SelectedItem);
            addNewPaymentWindow.Owner = this;
            addNewPaymentWindow.Show();
            addNewPaymentWindow.RentalPayed += AddNewPaymentWindow_PaymentAdded;
        }
        private void AddNewPaymentWindow_PaymentAdded(object sender, EventArgs e)
        {
            rentals = new DataWorker().GetAllRentalsNotPayed();
            RentalsList.ItemsSource = rentals;
        }
    }
}
