using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Diagnostics;
using customerapp.Dto;
using System.Threading.Tasks;
using PayPal.Forms;
using PayPal.Forms.Abstractions;
using PayPal.Forms.Abstractions.Enum;
using Acr.UserDialogs;

namespace customerapp
{
	public partial class OrderConfirmPage : ContentPage
	{
		private Order order; 

		public OrderConfirmPage (Order order)
		{
			InitializeComponent ();
			this.order = order;

			initialiseMap (order);
		}

		private void initialiseMap(Order order){
			var map = this.FindByName<MapWithRoute>("Map");

            // Show only the first mission - because drop position is same
			var enumerator = order.Missions.GetEnumerator ();
			enumerator.MoveNext ();

            var dropPosition = enumerator.Current.Route.DropPosition();
			var pin = createPinForDeliveryPosition (dropPosition);

			map.Pins.Add(pin);
			map.MoveToRegion(
				MapSpan.FromCenterAndRadius(
					pin.Position, 
					Distance.FromMeters(100)
				)
			);
		}

		private Pin createPinForDeliveryPosition(customerapp.Dto.Position position){
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
			
        async void OnConfirmButtonClicked(object sender, EventArgs e)
		{
            var userNeedToLoginFirst = !App.IsLoggedIn;
            if(userNeedToLoginFirst)
            {
				await Navigation.PushModalAsync (new AuthenticationPage ());		
			}

			if (App.IsLoggedIn) 
            {
                var isPaymentSuccess = await doPayment ();
                if(isPaymentSuccess)
                {
                    await ConfirmOrder ();    
                }
			} 
            else 
            {
				Debug.WriteLine ("Customer not logged in yet");
			}
		}

        private async Task ConfirmOrder(){
            await App.Rest.ConfirmOrder (order.Id, App.Customer.Id);

            var showMissionList = order.Missions.Count > 1;
            if (showMissionList) {
                Device.BeginInvokeOnMainThread (() => {
                    Navigation.PopModalAsync ();
                    Navigation.PushModalAsync (new MissionListPage (order));
                });
            } 
            else 
            {
                Device.BeginInvokeOnMainThread( () => {
                    var missionEnum = order.Missions.GetEnumerator();
                    missionEnum.MoveNext ();

                    Navigation.PopModalAsync ();
                    Navigation.PushModalAsync(new MissionPage(missionEnum.Current));
                });

            }
        }

		async  void OnCancelButtonClicked(object sender, EventArgs e){
            await App.Rest.DeleteOrder (order.Id);
			await Navigation.PopModalAsync();
		}


        private async Task<bool> doPayment(){

            decimal sum = 0;
            foreach(OrderProduct each in order.OrderProducts){
                sum += each.Amount;
            }

            Debug.WriteLine("Payment Successful");
            var result = await CrossPayPalManager.Current.Buy(new PayPalItem("Order ", (Decimal) sum, "CHF"), new Decimal(0));

            if (result.Status == PayPalStatus.Cancelled)
            {
                UserDialogs.Instance.ShowError ("Cancelled");
                return false;
            }
            else if (result.Status == PayPalStatus.Error)
            {
                Debug.WriteLine (result.ErrorMessage);
                UserDialogs.Instance.ShowError ("Failed tp do payment");
                return false;
            }
            else if (result.Status == PayPalStatus.Successful)
            {
                Debug.WriteLine("Payment Successful");
                Debug.WriteLine(result.ServerResponse.Response.Id);
                UserDialogs.Instance.ShowSuccess ("Payment accepted");

                return true;
            }

            return false;
        }
	}
}

