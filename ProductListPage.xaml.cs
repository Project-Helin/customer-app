using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Diagnostics;
using customerapp.Dto;
using System.Linq;
using PayPal.Forms;
using PayPal.Forms.Abstractions;
using PayPal.Forms.Abstractions.Enum;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace customerapp
{
    public partial class ProductListPage : ContentPage
    {
        ObservableCollection<Product> products = new ObservableCollection<Product>();
        ObservableCollection<Product> orderedProducts = new ObservableCollection<Product>();

        public ProductListPage()
        {
            InitializeComponent();
        }

		protected async override void OnAppearing(){
			base.OnAppearing ();
			var products2 = await FetchProduct();
			foreach (Product b in products2) {
				products.Add (b);
			}
				
			ProductListView.ItemsSource = products;
			ProductListView.ItemTapped += ProductTapped;
		}

		public async Task<List<Product>> FetchProduct()
		{
			var client = new HttpClient ();
			using (client) {

				Debug.WriteLine ("Fetch order ");
				// TODO we need a common place for this
				var uri = new Uri ("http://192.168.222.1:9000/api/products/");
				var response = await client.GetAsync (uri);
				List<Product> items = new List<Product>();

				if (response.IsSuccessStatusCode) {
					var content = await response.Content.ReadAsStringAsync ();
					items = Newtonsoft.Json.JsonConvert.DeserializeObject <List<Product>> (content);
				} else {
					Debug.WriteLine ("Failed to get all order with status code " + response.StatusCode);
				}
			
				return items;
			}
		}


		void ProductTapped(object sender, ItemTappedEventArgs e)
        {
            Product selectedProduct = e.Item as Product;

            if (!orderedProducts.Contains(selectedProduct))
            {
                orderedProducts.Add(selectedProduct);
            }

            selectedProduct.Amount += 1;
        }

        async void OnOrderButtonClick(object sender, EventArgs args)
        {
            var orderSum = orderedProducts.Sum((a) => a.Amount * a.Price);
            var orderConfirmed = await DisplayAlert("Produkte Kaufen?", "Möchten Sie die Bestellung im Wert von" + orderSum + " CHF abschicken?", "Ja", "Nein");

            if (orderConfirmed)
            {
        
                var result = await CrossPayPalManager.Current.Buy(new PayPalItem("Order 123", (Decimal) orderSum, "CHF"), new Decimal(0));
                if (result.Status == PayPalStatus.Cancelled)
                {
                    Debug.WriteLine("Cancelled");
                }
                else if (result.Status == PayPalStatus.Error)
                {
                    Debug.WriteLine(result.ErrorMessage);
                }
                else if (result.Status == PayPalStatus.Successful)
                {
                    Debug.WriteLine("Payment Successful");
                    Debug.WriteLine(result.ServerResponse.Response.Id);
                }
            }
          
        }
		    
    }
}

