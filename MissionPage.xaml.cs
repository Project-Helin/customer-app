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

		public MissionPage (Mission mission)
		{
			InitializeComponent ();

			Debug.WriteLine ("Init MissionPage");

			this.mission = mission;
			initialiseMap (mission);
		}


		MapWithRoute initialiseMap(Mission mission){
			


			var map = Map;

			// add all waypoints for route
			foreach (var eachWayPoint in mission.Route.WayPoints){
				map.CalculatedRoute.Add (eachWayPoint.Position);	
			}

			// add all waypoints for route
			foreach (var info in mission.DroneInfos){
				map.FlownRoute.Add (info.PhonePosition);	
			}

			Waypoint droppoint = mission.Route.WayPoints.Find (o => o.Action.Equals ("DROP"));

			var pin = createPinForDeliveryPosition(droppoint.Position);
			map.Pins.Add(pin);
			map.MoveToRegion(MapSpan.FromCenterAndRadius(pin.Position, Distance.FromMeters(100)));

			return map;
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
	}
}

