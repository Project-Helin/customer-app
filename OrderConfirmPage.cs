using System;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using customerapp.Dto;

namespace customerapp
{
    public class OrderConfirmPage : ContentPage
    {
		public OrderConfirmPage(OrderApiOutput orderApiOutput)
        {
            var map = new Map(
        	    MapSpan.FromCenterAndRadius(
					new Xamarin.Forms.Maps.Position(
						orderApiOutput.DeliveryPosition.Lat, 
						orderApiOutput.DeliveryPosition.Lon
					), 
					Distance.FromMiles(0.3)
				)
			)
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


