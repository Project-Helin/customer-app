using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Diagnostics;

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
			
			var map = this.FindByName<MapWithRoute>("Map");
			map.MapType = MapType.Satellite;

			// add all waypoints for route
			foreach (var eachWayPoint in mission.Route.WayPoints){
				map.RouteCoordinates.Add (eachWayPoint.Position);	
			}
//
//			var pin = createPinForDeliveryPosition ();
//			map.Pins.Add(pin);
//			map.MoveToRegion(MapSpan.FromCenterAndRadius(pin.Position, Distance.FromMeters(100)));

			return map;
		}

//		Pin createPinForDeliveryPosition(){
//			var deliveryPosition = new Xamarin.Forms.Maps.Position(
//				orderApiOutput.DeliveryPosition.Lat, 
//				orderApiOutput.DeliveryPosition.Lon
//			);
//			var pin = new Pin {
//				Type = PinType.Generic,
//				Position = deliveryPosition,
//				Label = "Drop position",
//				Address = "Expected drop position for your delivery"
//			};
//
//			return pin;
//		}
	}
}

