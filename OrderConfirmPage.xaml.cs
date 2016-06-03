using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Diagnostics;
using customerapp.Dto;

namespace customerapp
{
	public partial class OrderConfirmPage : ContentPage
	{
	
		private Order order; 
		IRestService rest = App.Rest;

		public OrderConfirmPage (Order order)
		{
			InitializeComponent ();
			this.order = order;

			initialiseMap (order);
		}

		void initialiseMap(Order order){
			var map = this.FindByName<MapWithRoute>("Map");



			var enumerator = order.Missions.GetEnumerator ();
			enumerator.MoveNext ();

			var position = enumerator.Current.Route.DropPosition();
			var pin = createPinForDeliveryPosition (position);

			map.Pins.Add(pin);
			map.MoveToRegion(
				MapSpan.FromCenterAndRadius(
					pin.Position, 
					Distance.FromMeters(100)
				)
			);

		}

		Pin createPinForDeliveryPosition(customerapp.Dto.Position position){
			var deliveryPosition = new Xamarin.Forms.Maps.Position(
				position.Lat, 
				position.Lon
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

			if(!App.IsLoggedIn){
				await Navigation.PushModalAsync (new AuthenticationPage ());		
			}

			if (App.IsLoggedIn) {



				await rest.ConfirmOrder (order.Id, App.Customer.Id);

				var x = order.Missions.GetEnumerator();
				x.MoveNext ();

				Debug.WriteLine ("First mission {0}", x.Current.Id);

				Debug.WriteLine ("Push misison page");

				Device.BeginInvokeOnMainThread( () => {
					Navigation.PopModalAsync ();
					Navigation.PushModalAsync(new MissionPage(x.Current));
				});


			} else {
				Debug.WriteLine ("Customer not logged in yet");
			}
		}

		async  void OnCancelButtonClicked(object sender, EventArgs e){
			await rest.DeleteOrder (order.Id);
			await Navigation.PopModalAsync();

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

