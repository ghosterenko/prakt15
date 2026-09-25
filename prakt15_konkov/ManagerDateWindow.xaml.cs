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
    public partial class ManagerDateWindow : Window
    {
        private StoreElectronicDbContext db;

        public ManagerDateWindow(StoreElectronicDbContext context)
        {
            InitializeComponent();
            db = context;
            LoadAll();
        }

        private void LoadAll()
        {
            CategoriesList.ItemsSource = db.Categories.ToList();
            BrandsList.ItemsSource = db.Brands.ToList();
            TagsList.ItemsSource = db.Tags.ToList();
        }

        private void Category_Selected(object sender, SelectionChangedEventArgs e)
        {
            Category c = CategoriesList.SelectedItem as Category;
            if (c != null) CategoryBox.Text = c.Name;
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CategoryBox.Text)) return;
            db.Categories.Add(new Category { Name = CategoryBox.Text.Trim() });
            db.SaveChanges();
            CategoryBox.Text = "";
            LoadAll();
        }

        private void EditCategory_Click(object sender, RoutedEventArgs e)
        {
            Category c = CategoriesList.SelectedItem as Category;
            if (c == null) return;
            if (string.IsNullOrWhiteSpace(CategoryBox.Text)) return;
            c.Name = CategoryBox.Text.Trim();
            db.SaveChanges();
            LoadAll();
        }

        private void DeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            Category c = CategoriesList.SelectedItem as Category;
            if (c == null) return;

            if (db.Products.Any(p => p.CategoryId == c.Id))
            {
                MessageBox.Show("Категория используется товарами!");
                return;
            }
            if (MessageBox.Show($"Удалить \"{c.Name}\"?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes) return;

            db.Categories.Remove(c);
            db.SaveChanges();
            CategoryBox.Text = "";
            LoadAll();
        }

        private void Brand_Selected(object sender, SelectionChangedEventArgs e)
        {
            Brand b = BrandsList.SelectedItem as Brand;
            if (b != null) BrandBox.Text = b.Name;
        }

        private void AddBrand_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(BrandBox.Text)) return;
            db.Brands.Add(new Brand { Name = BrandBox.Text.Trim() });
            db.SaveChanges();
            BrandBox.Text = "";
            LoadAll();
        }

        private void EditBrand_Click(object sender, RoutedEventArgs e)
        {
            Brand b = BrandsList.SelectedItem as Brand;
            if (b == null) return;
            if (string.IsNullOrWhiteSpace(BrandBox.Text)) return;
            b.Name = BrandBox.Text.Trim();
            db.SaveChanges();
            LoadAll();
        }

        private void DeleteBrand_Click(object sender, RoutedEventArgs e)
        {
            Brand b = BrandsList.SelectedItem as Brand;
            if (b == null) return;

            if (db.Products.Any(p => p.BrandId == b.Id))
            {
                MessageBox.Show("Бренд используется товарами!");
                return;
            }
            if (MessageBox.Show($"Удалить \"{b.Name}\"?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes) return;

            db.Brands.Remove(b);
            db.SaveChanges();
            BrandBox.Text = "";
            LoadAll();
        }

        private void Tag_Selected(object sender, SelectionChangedEventArgs e)
        {
            Tag t = TagsList.SelectedItem as Tag;
            if (t != null) TagBox.Text = t.Name;
        }

        private void AddTag_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TagBox.Text)) return;
            db.Tags.Add(new Tag { Name = TagBox.Text.Trim() });
            db.SaveChanges();
            TagBox.Text = "";
            LoadAll();
        }

        private void EditTag_Click(object sender, RoutedEventArgs e)
        {
            Tag t = TagsList.SelectedItem as Tag;
            if (t == null) return;
            if (string.IsNullOrWhiteSpace(TagBox.Text)) return;
            t.Name = TagBox.Text.Trim();
            db.SaveChanges();
            LoadAll();
        }

        private void DeleteTag_Click(object sender, RoutedEventArgs e)
        {
            Tag t = TagsList.SelectedItem as Tag;
            if (t == null) return;

            if (MessageBox.Show($"Удалить \"{t.Name}\"?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes) return;

            db.Tags.Remove(t);
            db.SaveChanges();
            TagBox.Text = "";
            LoadAll();
        }
    }
}

