using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Collections.ObjectModel;
using customerapp.Dto;
using System.Diagnostics;

namespace customerapp
{
    public partial class OrderListPage : ContentPage
    {
        ObservableCollection<Order> orders = new ObservableCollection<Order>();


        public OrderListPage()
		{
            InitializeComponent();

            OrderListView.ItemsSource = orders;

            OrderListView.ItemSelected += OrderSelected;
        }

		protected async override void OnAppearing(){
			base.OnAppearing ();

			orders.Clear ();

			if (App.IsLoggedIn) {
				var ordersFromServer = await App.Rest.GetAllOrders (Settings.GetCustomerIdOrNull());
				foreach (Order each in ordersFromServer) {
					orders.Add (each);
				}
			} else {
				Debug.WriteLine ("Dont fetch order because not logged in.");
			}

		}

        async void OrderSelected (object sender, SelectedItemChangedEventArgs e)
        {
            var selectedOrder = e.SelectedItem as Order;
			await Navigation.PushAsync(new MissionListPage(selectedOrder));
        }
    }
}

