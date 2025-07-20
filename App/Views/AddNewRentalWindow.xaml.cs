using EquipRentalPointApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EquipRentalPointApp.Views
{
    public partial class AddNewRentalWindow : Window
    {
        public AddNewRentalWindow(List<Equipment> equipments)
        {
            InitializeComponent();
            EquipmentsList.ItemsSource = equipments;
            ClientsList.ItemsSource = new DataWorker().GetAllClients();
            decimal total = 0;
            foreach (Equipment equip in equipments)
                total += equip.Price;
            PriceForDay.Content = total.ToString("F2");
            FirstPayment.Text = decimal.Parse(PriceForDay.Content.ToString()).ToString();
            TotalPrice.MouseLeftButtonUp += TotalPrice_MouseLeftButtonUp;
            PriceForDay.MouseLeftButtonUp += PriceForDay_MouseLeftButtonUp;
        }

        private void PriceForDay_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FirstPayment.Text = PriceForDay.Content.ToString();
        }

        private void TotalPrice_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FirstPayment.Text = TotalPrice.Content.ToString();
        }

        private void EquipmentsList_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in EquipmentsList.Items)
            {
                var listBoxItem = (ListBoxItem)EquipmentsList.ItemContainerGenerator.ContainerFromItem(item);
                listBoxItem.AddHandler(ListBoxItem.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(ListBoxItem_PreviewMouseLeftButtonDown), true);
            }
        }

        private void ListBoxItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            int dateRange = 1;
            if (Calendar.SelectedDates.Count - 1 > 0)
                dateRange = Calendar.SelectedDates.Count - 1;
            TotalPrice.Content = (dateRange * decimal.Parse(PriceForDay.Content.ToString())).ToString();
            AddRental.IsEnabled = true;
        }

        private void FirstPayment_GotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).Text = "";
        }

        private void ClientsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Calendar.IsEnabled = true;
        }

        private void AddRental_Click(object sender, RoutedEventArgs e)
        {
            if (ClientPhoneTextBox.Text.Length > 0)
            {
                try
                {
                    Rental newRental = new Rental
                    {
                        Client = (Client)ClientsList.SelectedItem,
                        Equipments = EquipmentsList.ItemsSource.Cast<Equipment>().ToList(),
                        DateBegin = Calendar.SelectedDate.GetValueOrDefault(DateTime.Now),
                        DateEnd = Calendar.SelectedDate.GetValueOrDefault(DateTime.Now).AddDays(Calendar.SelectedDates.Count() - 1),
                        Payed = decimal.Parse(FirstPayment.Text.ToString())
                    };
                    var dataWorker = new DataWorker();
                    DateTime dateBegin = Calendar.SelectedDate ?? DateTime.Now.Date;
                    dataWorker.AddRental(newRental);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                this.Close();
            }
        }
    }
}
