using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Auth;
using System.Diagnostics;
using System.Threading.Tasks;

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

            await loadCustomerIfAvailable ();
		}

        private async Task loadCustomerIfAvailable(){
            var customerId = Settings.GetCustomerIdOrNull ();

            var noCustomerAvailable =  !App.IsLoggedIn;

            if (App.IsLoggedIn) {
                App.Customer = await App.Rest.GetCustomerById (customerId); 
            } else {
                Debug.WriteLine ("No customer loaded because hasCustomer={0} and isCustomerNull = {1}", 
                    noCustomerAvailable, App.Customer == null);
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

