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
    public partial class AutorizWindow : Window
    {
        public AutorizWindow()
        {
            InitializeComponent();
        }
        private void LoginManager_Click(object sender, RoutedEventArgs e)
        {
            if (PinBox.Password == "1234")
            {
                MainWindow mainWindow = new MainWindow(true);
                mainWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Неверный пин-код", "Ошибка авторизации", MessageBoxButton.OK);
            }
        }

        private void LoginVisitor_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(false);
            mainWindow.Show();
            Close();
        }
    }
}
