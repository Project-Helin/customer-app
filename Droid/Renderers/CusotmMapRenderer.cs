using System;
using Xamarin.Forms;
using MapOverlay.Droid;
using Xamarin.Forms.Maps.Android;
using customerapp;
using System.Collections.Generic;
using customerapp.Dto;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;

[assembly:ExportRenderer (typeof(MapWithRoute), typeof(CustomMapRenderer))]
namespace MapOverlay.Droid
{
	public class CustomMapRenderer : MapRenderer, IOnMapReadyCallback
	{
		
		GoogleMap map;

		List<Position> routeCoordinates;

		protected override void OnElementChanged (Xamarin.Forms.Platform.Android.ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged (e);

			if (e.OldElement != null) {
				// Unsubscribe
			}

			if (e.NewElement != null) {
				var formsMap = (MapWithRoute)e.NewElement;
				routeCoordinates = formsMap.RouteCoordinates;

				((MapView)Control).GetMapAsync (this);
			}
		}

		public void OnMapReady (GoogleMap googleMap)
		{
			map = googleMap;

			var polylineOptions = new PolylineOptions ();
			polylineOptions.InvokeColor (0x66FF0000);

			foreach (var position in routeCoordinates) {
				polylineOptions.Add (new LatLng (position.Lat, position.Lon));
			}

			map.AddPolyline (polylineOptions);
		}

	}
}