using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Collections.ObjectModel;
using customerapp.Dto;

namespace customerapp
{
    public partial class OrderListPage : ContentPage
    {
        ObservableCollection<Order> orders = new ObservableCollection<Order>();

        public OrderListPage()
		{

			// TODO replace REST API: getOrderByCustomerId
			var product1 = new Product { Name = "Coca Cola 0.5 L", Price = 4.0m };
            var product2 = new Product { Name = "Fanta Orange 0.5 L", Price = 4.0m };
            var product3 = new Product { Name = "Rivella Rot 0.5 L", Price = 4.0m };

            orders.Add(new Order{ Product = product1, OrderTime = DateTime.Now.AddMonths(-4) });
            orders.Add(new Order{ Product = product2, OrderTime = DateTime.Now.AddMonths(-1) });
            orders.Add(new Order{ Product = product3, OrderTime = DateTime.Now.AddMonths(-2) });
            orders.Add(new Order{ Product = product1, OrderTime = DateTime.Now.AddMonths(-6) });

            InitializeComponent();

            OrderListView.ItemsSource = orders;

            OrderListView.ItemSelected += OrderSelected;
        }

        async void OrderSelected (object sender, SelectedItemChangedEventArgs e)
        {
            var selectedOrder = e.SelectedItem as Order;
            await Navigation.PushAsync(new OrderDetailPage(selectedOrder));
        }
    }
}

