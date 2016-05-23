using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace customerapp
{
	public partial class OrderConfirmPage : ContentPage
	{
		private OrderApiOutput orderApiOutput; 
		RestService rest = new RestService ();

		public OrderConfirmPage (OrderApiOutput orderApiOutput)
		{
			InitializeComponent ();
			this.orderApiOutput = orderApiOutput;

			var map = initialiseMap (orderApiOutput);
		}

		MapWithRoute initialiseMap(OrderApiOutput orderApiOutput){

			var map = this.FindByName<MapWithRoute>("Map");
			map.MapType = MapType.Satellite;

			// add all waypoints for route
			foreach (var eachWayPoint in orderApiOutput.Route.WayPoints){
				map.RouteCoordinates.Add (eachWayPoint.Position);	
			}
				
			var pin = createPinForDeliveryPosition ();
			map.Pins.Add(pin);
			map.MoveToRegion(MapSpan.FromCenterAndRadius(pin.Position, Distance.FromMeters(100)));

			return map;
		}

		Pin createPinForDeliveryPosition(){
			var deliveryPosition = new Xamarin.Forms.Maps.Position(
				orderApiOutput.DeliveryPosition.Lat, 
				orderApiOutput.DeliveryPosition.Lon
			);
			var pin = new Pin {
				Type = PinType.Generic,
				Position = deliveryPosition,
				Label = "Drop position",
				Address = "Expected drop position for your delivery"
			};

			return pin;
		}
			
		async void OnButtonClicked(object sender, EventArgs e)
		{
			await rest.ConfirmOrder (orderApiOutput.orderId);
			// TODO change to another page 
			var stack = new StackLayout { Spacing = 0 };
			stack.Children.Add(initialiseMap(orderApiOutput));
			Content = stack;
		}

		async  void OnCancelButtonClicked(object sender, EventArgs e){
			await rest.CancelOrder (orderApiOutput.orderId);
			await Navigation.PopAsync();

		}
	}
}

