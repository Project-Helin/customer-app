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

			var customerId = Settings.GetCustomerIdOrNull ();
			var loadCustomer =  customerId != null && App.Customer == null;
			if (loadCustomer) {
				App.Customer = await App.Rest.GetCustomerById (customerId);	
			} else {
				Debug.WriteLine ("Not loaded customer because hasCustomer={0} and isCustomerNull = {1}", 
					loadCustomer, App.Customer == null);
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

