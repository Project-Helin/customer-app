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


namespace customerapp
{
    public partial class ProductListPage : ContentPage
    {
        ObservableCollection<Product> products = new ObservableCollection<Product>();
        ObservableCollection<Product> orderedProducts = new ObservableCollection<Product>();

		RestService restService = new RestService();

        public ProductListPage()
        {
            InitializeComponent();
			ProductListView.ItemsSource = products;
			ProductListView.ItemTapped += ProductTapped;
        }

		protected async override void OnAppearing(){
			base.OnAppearing ();

			orderedProducts.Clear ();
			products.Clear ();

			Button button = this.FindByName<Button> ("SendOrderButton");
			button.IsEnabled = false;

			var productsFromServer = await restService.GetAllProducts();
			foreach (Product each in productsFromServer) {
				products.Add (each);
			}

			setTotalAmout (0);
		}

		void ProductTapped(object sender, ItemTappedEventArgs e)
        {
            Product selectedProduct = e.Item as Product;
			selectedProduct.Amount += 1;

			var isNotAlreadyOrdered = !orderedProducts.Contains (selectedProduct);
			if (isNotAlreadyOrdered)
            {
                orderedProducts.Add(selectedProduct);
            }
				
			if (selectedProduct.Amount > 0) {
				Button button = this.FindByName<Button> ("SendOrderButton");
				button.IsEnabled = true;
			}

			updateTotalAmount ();

			// TODO: disable all products with different organisation
        }

		void updateTotalAmount(){
			var totalAmout = orderedProducts.Sum((a) => a.Amount * a.Price);
			setTotalAmout (totalAmout);
		}

		void setTotalAmout(decimal totalAmout){
			Label totalAmountLabel = this.FindByName<Label> ("TotalAmount");
			totalAmountLabel.Text = "Total: " + string.Format("{0:0.00}", totalAmout)  + " CHF";
		}


        async void OnOrderButtonClick(object sender, EventArgs args)
        {
            var orderSum = orderedProducts.Sum((a) => a.Amount * a.Price);

            var orderConfirmed = await DisplayAlert("Buy products?", 
				"Are you sure to order for " + string.Format("{0:0.00}", orderSum) + " CHF?", "Yes", "No");

            if (orderConfirmed)
            {
				// get Location 
				var a = await Navigation.PushModalAsync(new AuthenticationPage());

				// send order
				if(App.IsLoggedIn){
					var response = await restService.CreateOrder(App.Customer, orderedProducts);
					var deliveryPosition = response.DeliveryPosition;
					Debug.WriteLine (deliveryPosition); 

					await Navigation.PushAsync(new OrderConfirmPage(response));
				}else{
					Debug.WriteLine("User not logged in ");

				}
            }
        }

		/* 
		 * TODO
		 */
		void payment(){
			// send the order
			Debug.WriteLine("Payment Successful");
			// let user pay the order ... 
			//                var result = await CrossPayPalManager.Current.Buy(new PayPalItem("Order 123", (Decimal) orderSum, "CHF"), new Decimal(0));
			//                if (result.Status == PayPalStatus.Cancelled)
			//                {
			//                    Debug.WriteLine("Cancelled");
			//                }
			//                else if (result.Status == PayPalStatus.Error)
			//                {
			//                    Debug.WriteLine(result.ErrorMessage);
			//                }
			//                else if (result.Status == PayPalStatus.Successful)
			//                {
			//                    Debug.WriteLine("Payment Successful");
			//                    Debug.WriteLine(result.ServerResponse.Response.Id);
			//                }

		}
		    
    }
}

