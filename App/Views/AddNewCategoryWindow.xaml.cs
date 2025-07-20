using EquipRentalPointApp.Models;
using System;
using System.Windows;

namespace EquipRentalPointApp.Views
{
    public partial class AddNewCategoryWindow : Window
    {
        private static AddNewCategoryWindow instance;
        
        public AddNewCategoryWindow()
        {
            InitializeComponent();
            Closing += AddNewCategoryWindow_Closing;
        }

        private void AddNewCategoryWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        public static AddNewCategoryWindow GetInstance()
        {
            if (instance == null)
                instance = new AddNewCategoryWindow();
            return instance;
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            Category newCategory = new Category
            {
                Title = (NewCategoryNameTextBox.Text.Trim())
            };
            var dataWorker = new DataWorker();
            dataWorker.AddCategory(newCategory);
            CategoryAdded?.Invoke(this, EventArgs.Empty);
            NewCategoryNameTextBox.Text = "";
            this.Hide();
        }

        public event EventHandler CategoryAdded;

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
