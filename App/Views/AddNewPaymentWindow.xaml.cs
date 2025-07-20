using EquipRentalPointApp.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace EquipRentalPointApp.Views
{
    public partial class AddNewPaymentWindow : Window
    {
        public AddNewPaymentWindow(Rental rental)
        {
            InitializeComponent();
            Rental.Items.Add(rental);
            Rental.DataContext = rental;
            ToFullPayLabel.Content = ((decimal)rental.Total - (decimal)rental.Payed).ToString("F2");
            ToFullPayLabel.MouseLeftButtonDown += ToFullPayLabel_MouseLeftButtonDown;
        }

        private void ToFullPayLabel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PaymentTextBox.Text = ToFullPayLabel.Content.ToString();
        }

        private void AddNewPaymentButton_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(PaymentTextBox.Text, out decimal price))
            {

                new DataWorker().AddPayment(new Payment()
                {
                    RentalId = (int)Rental.Items.Cast<Rental>().FirstOrDefault()?.Id,
                    PaymentDate = DateTime.Now,
                    Amount = price
                });
                RentalPayed?.Invoke(this, EventArgs.Empty);
                this.Close();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public event EventHandler RentalPayed;
    }
}
