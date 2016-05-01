using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace customerapp
{
    public partial class MainPage : ContentPage
    {
        public MainPage () 
        {
            InitializeComponent();
        }

        void OnShowOrdersClick(object sender, EventArgs args) {
        }

        async void OnShowProductButtonClick(object sender, EventArgs args) {
            await Navigation.PushAsync(new ProductListPage());
        }



 
    }
}

