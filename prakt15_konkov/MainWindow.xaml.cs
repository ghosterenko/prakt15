using Microsoft.EntityFrameworkCore;
using prakt15_konkov.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace prakt15_konkov
{
    public partial class MainWindow : Window
    {
        private StoreElectronicDbContext db = new StoreElectronicDbContext();
        public ObservableCollection<Product> products { get; set; } = new ObservableCollection<Product>();
        private List<Product> allProducts = new List<Product>();

        public MainWindow(bool isManager)
        {
            InitializeComponent();

            if (isManager) ManagerPanel.Visibility = Visibility.Visible;

            LoadData();
            ProductsList.ItemsSource = products;
            UpdateCounters();
        }

        private void LoadData()
        {
            try
            {
                allProducts = db.Products.Include(p => p.Category)
                    .Include(p => p.Brand)
                    .ToList();

                ApplyFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка");
            }
        }

        private void ApplyFilter()
        {
            var result = allProducts.ToList();

            if (!string.IsNullOrWhiteSpace(SearchBox.Text))
                result = result.Where(p => p.Name.Contains(SearchBox.Text, StringComparison.OrdinalIgnoreCase)).ToList();

            if (decimal.TryParse(PriceFrom.Text, out decimal min))
                result = result.Where(p => p.Price >= min).ToList();

            if (decimal.TryParse(PriceTo.Text, out decimal max))
                result = result.Where(p => p.Price <= max).ToList();

            if (SortBox.SelectedItem is ComboBoxItem item && item.Tag != null)
            {
                switch (item.Tag.ToString())
                {
                    case "Name":
                        result = result.OrderBy(p => p.Name).ToList();
                        break;
                    case "PriceAsc":
                        result = result.OrderBy(p => p.Price).ToList();
                        break;
                    case "PriceDesc":
                        result = result.OrderByDescending(p => p.Price).ToList();
                        break;
                    case "StockAsc":
                        result = result.OrderBy(p => p.Stock).ToList();
                        break;
                    case "StockDesc":
                        result = result.OrderByDescending(p => p.Stock).ToList();
                        break;
                }
            }

            products.Clear();
            foreach (var p in result) products.Add(p);

            UpdateCounters();
        }

        private void FilterChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilter();
        }
        private void SortChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilter();
        }
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = string.Empty;
            PriceFrom.Text = string.Empty;
            PriceTo.Text = string.Empty;
            SortBox.SelectedItem = null;

            ApplyFilter();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var window = new ProductWindow(db);
            window.Owner = this;
            if (window.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsList.SelectedItem is Product selectedProduct)
            {
                var window = new ProductWindow(db, selectedProduct);
                window.Owner = this;
                if (window.ShowDialog() == true)
                {
                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Выберите товар из списка для редактирования!", "Предупреждение");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsList.SelectedItem is Product selectedProduct)
            {

                if (MessageBox.Show($"Вы действительно хотите удалить товар \"{selectedProduct.Name}\"?", "Подтверждение удаления", MessageBoxButton.YesNo)
                    == MessageBoxResult.Yes)
                {
                    db.Products.Remove(selectedProduct);
                    db.SaveChanges();
                    LoadData();
                }
            }
            else MessageBox.Show("Выберите товар из списка для удаления.", "Предупреждение", MessageBoxButton.OK);
        }

        private void References_Click(object sender, RoutedEventArgs e)
        {
            var window = new ManagerDateWindow(db);
            window.Owner = this;
            window.ShowDialog();
            LoadData();
        }

        private void UpdateCounters()
        {
            TotalCountText.Text = $"Всего в базе: {allProducts.Count}";
            FilteredCountText.Text = $"Отображено: {products.Count}";
        }

    }
}