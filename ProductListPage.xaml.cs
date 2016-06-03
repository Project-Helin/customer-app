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
            this.Title = "Available Products";
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
                var productsFromServer = 
                    await App.Rest.GetAllProductsByLocation (customerPosition);
                
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
				
            var canSentOrder = selectedProduct.Amount > 0;
            if (canSentOrder) {
				Button button = this.FindByName<Button> ("SendOrderButton");
				button.IsEnabled = true;
			}

			updateTotalAmount ();
        }

		private void updateTotalAmount(){
			var totalAmout = orderedProducts.Sum((a) => a.Amount * a.Price);
			setTotalAmout (totalAmout);
		}

		private void setTotalAmout(decimal totalAmout){
			Label totalAmountLabel = this.FindByName<Label> ("TotalAmount");
			totalAmountLabel.Text = "Total: " + string.Format("{0:0.00}", totalAmout)  + " CHF";
		}

        async void OnOrderButtonClick(object sender, EventArgs args)
        {
            var orderSum = orderedProducts.Sum((a) => a.Amount * a.Price);

            var message = "Are you sure to order for " + string.Format ("{0:0.00}", orderSum) + " CHF?";
            var orderConfirmed = await DisplayAlert("Buy products?", message, "Yes", "No");

            if (orderConfirmed)
            {
                var customerPosition = await PositionHelper.GetPositionOrNull (this);
                var response = await App.Rest.CreateOrder(orderedProducts, customerPosition);

                if(response != null ){
                    Debug.WriteLine ("Created order with id {0}", response.Id); 

                    await Navigation.PushModalAsync(new OrderConfirmPage(response));                    
                }
            }
        }
    }
}

