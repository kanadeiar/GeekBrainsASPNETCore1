using System;
using System.Net.Http;
using System.Windows;
using WebStore.WebAPI.Client.Values;

namespace WpfAppWebAPI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _webAPI = "http://localhost:5001";
        private ValuesClient _client;
        public MainWindow()
        {
            InitializeComponent();
            _client = new ValuesClient(new HttpClient {BaseAddress = new Uri(_webAPI)});
        }

        private void ButtonUpdate_OnClick(object sender, RoutedEventArgs e)
        {
            UpdateItemsOnForm();
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            _client.Add("Новая строка");
            UpdateItemsOnForm();
        }

        private void ButtonDeleta_OnClick(object sender, RoutedEventArgs e)
        {
            _client.Delete(0);
            UpdateItemsOnForm();
        }

        /// <summary> Обновить элементы на форме </summary>
        private void UpdateItemsOnForm()
        {
            var items = _client.GetAll();
            ListBoxItems.ItemsSource = items;
        }
    }
}
