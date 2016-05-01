using System;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using customerapp.Dto;

namespace customerapp
{
    public class OrderDetailPage : ContentPage
    {
        public OrderDetailPage(Order order)
        {
            var map = new Map(
                          MapSpan.FromCenterAndRadius(
                              new Xamarin.Forms.Maps.Position(37, -122), Distance.FromMiles(0.3)))
            {
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(map);
            Content = stack;
        }
    }
}


