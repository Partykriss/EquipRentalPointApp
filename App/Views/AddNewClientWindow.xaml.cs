using EquipRentalPointApp.Models;
using System;
using System.Windows;

namespace EquipRentalPointApp.Views
{
    public partial class AddNewClientWindow : Window
    {
        private static AddNewClientWindow instance;

        public AddNewClientWindow()
        {
            InitializeComponent();
            Closing += AddNewClientWindow_Closing;
        }

        public static AddNewClientWindow GetInstance()
        {
            if (instance == null)
                instance = new AddNewClientWindow();
            return instance;
        }

        private void AddNewClientWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Client newClient = new Client
                {
                    FullName = NewClientFullNameTextBox.Text.Trim(),
                    Phone = NewClientPhoneTextBox.Text.Trim()
                };
                var dataWorker = new DataWorker();
                dataWorker.AddClient(newClient);
                ClientAdded?.Invoke(this, EventArgs.Empty);
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            NewClientFullNameTextBox.Text = "";
            NewClientPhoneTextBox.Text = "";
        }

        public event EventHandler ClientAdded;

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
