using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Diagnostics;
using customerapp.Dto;


namespace customerapp
{
	public partial class MissionPage : ContentPage
	{

		private Mission mission;
		Websockets.IWebSocketConnection connection;

		public MissionPage (Mission mission)
		{
			InitializeComponent ();
            this.Title = "Mission";
			Debug.WriteLine ("Init MissionPage");

			this.mission = mission;
			initialiseMap (mission);
		}
            
		private MapWithRoute initialiseMap(Mission mission){
			
			var map = Map;

			// add all waypoints for route
			foreach (var eachWayPoint in mission.Route.WayPoints){
				map.CalculatedRoute.Add (eachWayPoint.Position);	
			}

            if (mission.DroneInfos != null) {
                // add all waypoints for route
                foreach (var info in mission.DroneInfos){
                    map.FlownRoute.Add (info.PhonePosition);    
                }
            }
			
            Waypoint dropPoint = mission.Route.WayPoints.Find (o => o.Action.Equals ("DROP"));

			var pin = createPinForDeliveryPosition(dropPoint.Position);
			map.Pins.Add(pin);
			map.MoveToRegion(MapSpan.FromCenterAndRadius(pin.Position, Distance.FromMeters(100)));

			return map;
		}


		protected override void OnAppearing(){
			base.OnAppearing ();
			connection = Websockets.WebSocketFactory.Create();
			connection.OnOpened += Connection_OnOpened;
			connection.OnMessage += Connection_OnMessage;

			connection.Open(String.Format(Constants.WsForDroneInfos, mission.Id));
		}

		private void Connection_OnOpened()
		{
			Debug.WriteLine("Opened !");
		}

		private void Connection_OnMessage(string obj)
		{
            var droneInfo = Newtonsoft.Json.JsonConvert.DeserializeObject <DroneInfoMessage> (obj);
			Map.CurrentPosition = droneInfo.DroneInfo.PhonePosition;
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
	}
}

