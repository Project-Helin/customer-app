using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Auth;
using System.Diagnostics;

namespace customerapp
{
    public partial class MainPage : ContentPage
    {
        public MainPage () 
        {
            InitializeComponent();

        }

		protected async override void OnAppearing(){
			base.OnAppearing ();
			var hasCustomer = Application.Current.Properties.ContainsKey("customerId");

			if (hasCustomer && App.Customer != null) {
				RestService rest = new RestService ();

				var customerId = Application.Current.Properties ["customerId"] as string;
				App.Customer = await rest.GetCustomerById (customerId);	
			}

		}


        async void OnShowOrdersClick(object sender, EventArgs args) {
            await Navigation.PushAsync(new OrderListPage());
        }

        async void OnShowProductButtonClick(object sender, EventArgs args) {
            await Navigation.PushAsync(new ProductListPage());
        }

    }
}

