using EquipRentalPointApp.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EquipRentalPointApp.Views
{
    public partial class AddNewEquipmentWindow : Window
    {
        private static AddNewEquipmentWindow instance;
        public AddNewEquipmentWindow()
        {
            InitializeComponent();
            CategoriesList.ItemsSource = new DataWorker().GetAllCategories();
            Closing += AddNewEquipmentWindow_Closing;
            AddNewCategoryWindow.GetInstance().CategoryAdded += AddNewCategoryWindow_CategoryAdded;
        }

        private void AddNewEquipmentWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        public static AddNewEquipmentWindow GetInstance()
        {
            if (instance == null)
                instance = new AddNewEquipmentWindow();
            return instance;
        }

        private void CategoriesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoriesList.SelectedItems.Count > 3)
            {
                CategoriesList.SelectedItems.RemoveAt(0);
            }
        }

        private void AddEquipment_Click(object sender, RoutedEventArgs e)
        {
            if(CategoriesList.SelectedItems.Count > 0 && decimal.TryParse(NewEquipmentPriceTextBox.Text, out decimal price))
            {
                Equipment newEquipment = new Equipment
                {
                    Title = NewEquipmentNameTextBox.Text.Trim(),
                    Price = price,
                    Categories = CategoriesList.SelectedItems.Cast<Category>().ToList()
                };
                var dataWorker = new DataWorker();
                dataWorker.AddEquipment(newEquipment);
                EquipmentAdded?.Invoke(this, EventArgs.Empty);
                CategoriesList.SelectedItems.Clear();
                NewEquipmentNameTextBox.Text = "";
                NewEquipmentPriceTextBox.Text = "";
                this.Hide();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        public event EventHandler EquipmentAdded;

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            var addNewCategoryWindow = AddNewCategoryWindow.GetInstance();
            addNewCategoryWindow.Owner = this;
            addNewCategoryWindow.Show();
        }

        private void AddNewCategoryWindow_CategoryAdded(object sender, EventArgs e)
        {
            CategoriesList.ItemsSource = new DataWorker().GetAllCategories();
        }
    }
}
