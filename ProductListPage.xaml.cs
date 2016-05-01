using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Diagnostics;
using customerapp.Dto;

namespace customerapp
{
    public partial class ProductListPage : ContentPage
    {
        ObservableCollection<Product> products = new ObservableCollection<Product>();

        public ProductListPage()
        {
            InitializeComponent();

            products.Add(new Product { Name = "Coca Cola 0.5 L", Price = 4.0 });
            products.Add(new Product { Name = "Fanta Orange 0.5 L", Price = 4.0 });
            products.Add(new Product { Name = "Rivella Rot 0.5 L", Price = 4.0 });
            products.Add(new Product { Name = "Drone Selfie", Price = 2.0 });
            products.Add(new Product { Name = "Snickers", Price = 1.50 });
            
            ProductListView.ItemsSource = products;

            ProductListView.ItemSelected += ProductSelected;
        }

        async void ProductSelected (object sender, SelectedItemChangedEventArgs e)
        {
            Product selectedProduct = e.SelectedItem as Product;

            var answer = await DisplayAlert ("Produkt Bestellen?", "Möchten Sie wirklich ein " + selectedProduct.Name + " bestellen?", "Ja", "Nein");
            Debug.WriteLine ("Answer: " + answer);
        }
    }
}

