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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PM1_020_ilichev.Pages
{
    /// <summary>
    /// Логика взаимодействия для Events.xaml
    /// </summary>
    public partial class Events : Page
    {
        public Events()
        {
            InitializeComponent();
            _listView.ItemsSource = ilichevEntities.GetContext().Events.ToList();
        }

        private void NameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = NameTextBox.Text.ToLower();
            var list = ilichevEntities.GetContext().Events.Where(ev => ev.Title.ToLower().Contains(text)).ToList();
            _listView.ItemsSource = list;
        }

        private void DateTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = DateTextBox.Text.ToLower();
            var list = ilichevEntities.GetContext().Events.Where(ev => ev.Date.ToString().Contains(text)).ToList();
            _listView.ItemsSource = list;
        }

        private void _regButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Registr());
        }

        private void _enterButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Auto());
        }
    }
}
