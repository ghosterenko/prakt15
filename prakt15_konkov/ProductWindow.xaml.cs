using prakt15_konkov.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace prakt15_konkov
{

    public partial class ProductWindow : Window
    {
        public StoreElectronicDbContext db;
        public Product product;

        public ProductWindow(StoreElectronicDbContext db, Product product = null)
        {
            InitializeComponent();
            this.db = db;
            this.product = product;

            CategoryBox.ItemsSource = db.Categories.ToList();
            BrandBox.ItemsSource = db.Brands.ToList();

            if (product != null)
            {
                TitleText.Text = "Редактирование товара";
                NameBox.Text = product.Name;
                PriceBox.Text = product.Price.ToString();
                QuantityBox.Text = product.Stock.ToString();
                RatingBox.Text = product.Rating.ToString();
                DescriptionBox.Text = product.Description;
                CategoryBox.SelectedItem = db.Categories.FirstOrDefault(c => c.Id == product.CategoryId);
                BrandBox.SelectedItem = db.Brands.FirstOrDefault(b => b.Id == product.BrandId);
            }
            else
            {
                product = new Product();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text) ||
                !decimal.TryParse(PriceBox.Text, out decimal price) ||
                !int.TryParse(QuantityBox.Text, out int quantity) ||
                !double.TryParse(RatingBox.Text, out double rating) ||
                CategoryBox.SelectedItem == null ||
                BrandBox.SelectedItem == null)
            {
                MessageBox.Show("Заполните корректно все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            product.Name = NameBox.Text.Trim();
            product.Price = price;
            product.Stock = quantity;
            product.Rating = rating;
            product.Description = DescriptionBox.Text.Trim();
            product.CategoryId = ((Category)CategoryBox.SelectedItem).Id;
            product.BrandId = ((Brand)BrandBox.SelectedItem).Id;

            if (product.Id == 0)
            {
                db.Products.Add(product);
            }

            db.SaveChanges();
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
