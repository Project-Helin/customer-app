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

namespace customerapp
{
    public partial class ProductListPage : ContentPage
    {
        ObservableCollection<Product> products = new ObservableCollection<Product>();
        ObservableCollection<Product> orderedProducts = new ObservableCollection<Product>();

        public ProductListPage()
        {
            InitializeComponent();

            products.Add(new Product { Name = "Coca Cola 0.5 L", Price = 4.0m });
            products.Add(new Product { Name = "Fanta Orange 0.5 L", Price = 4.0m });
            products.Add(new Product { Name = "Rivella Rot 0.5 L", Price = 4.0m });
            products.Add(new Product { Name = "Drone Selfie", Price = 2.0m });
            products.Add(new Product { Name = "Snickers", Price = 1.50m });
            
            ProductListView.ItemsSource = products;

            ProductListView.ItemTapped += ProductTapped;
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
        
                var result = await CrossPayPalManager.Current.Buy(new PayPalItem("Order 123", new Decimal(12.55), "CHF"), new Decimal(0));
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

