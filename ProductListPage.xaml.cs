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
using Acr.UserDialogs;


namespace customerapp
{
    public partial class ProductListPage : ContentPage
    {
        ObservableCollection<Product> products = new ObservableCollection<Product>();
        ObservableCollection<Product> orderedProducts = new ObservableCollection<Product>();

        public ProductListPage()
        {
            InitializeComponent();
			ProductListView.ItemsSource = products;
			ProductListView.ItemTapped += ProductTapped;
        }

		protected async override void OnAppearing()
        {
			base.OnAppearing ();

            ResetElements ();
            await LoadProducts ();
		}

        private void ResetElements()
        {
            orderedProducts.Clear ();
            products.Clear ();

            Button button = this.FindByName<Button> ("SendOrderButton");
            button.IsEnabled = false;

            setTotalAmout (0);
        }

        private async Task LoadProducts()
        {
            var customerPosition = await PositionHelper.GetPositionOrNull (this);

            if (customerPosition != null) {
                UserDialogs.Instance.ShowLoading ("Loading available products");

                var productsFromServer = 
                    await App.Rest.GetAllProductsByLocation (customerPosition);
                
                UserDialogs.Instance.HideLoading ();

                foreach (Product each in productsFromServer) {
                    products.Add (each);
                }
            } 
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
				UserDialogs.Instance.ShowLoading ("Get drop position");
                Order response = await App.Rest.CreateOrder(orderedProducts, null);
				UserDialogs.Instance.HideLoading ();

				Debug.WriteLine ("Created order with id {0}", response.Id); 

				await Navigation.PushModalAsync(new OrderConfirmPage(response));
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

