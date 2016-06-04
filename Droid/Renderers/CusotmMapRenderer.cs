using System;
using Xamarin.Forms;
using MapOverlay.Droid;
using Xamarin.Forms.Maps.Android;
using customerapp;
using System.Collections.Generic;
using customerapp.Dto;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Xamarin.Forms.Maps;
using System.ComponentModel;

[assembly:ExportRenderer (typeof(MapWithRoute), typeof(CustomMapRenderer))]
namespace MapOverlay.Droid
{
	public class CustomMapRenderer : MapRenderer, IOnMapReadyCallback
	{
		
		GoogleMap map;
		MapWithRoute mapR;

		List<customerapp.Dto.Position> calculatedRoute;
		List<customerapp.Dto.Position> flownRoute;

		protected override void OnElementChanged (Xamarin.Forms.Platform.Android.ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged (e);

			if (e.OldElement != null) {
				// Unsubscribe
			}

			if (e.NewElement != null) {
				var formsMap = (MapWithRoute)e.NewElement;
				mapR = formsMap;
				calculatedRoute = formsMap.CalculatedRoute;
				flownRoute = formsMap.FlownRoute;

				formsMap.PropertyChanged += OnChange;


				((MapView)Control).GetMapAsync (this);
			}
		
		}

		public void OnChange(object o,PropertyChangedEventArgs ar){

            Device.BeginInvokeOnMainThread( () => {
    			var polylineOptions = new CircleOptions ();
                polylineOptions.InvokeFillColor (0x00E5FF);
    			polylineOptions.InvokeRadius (3);
                Console.WriteLine(mapR.CurrentPosition.Lat + " " + mapR.CurrentPosition.Lon );
    			polylineOptions.InvokeCenter (
    				new LatLng(
    					mapR.CurrentPosition.Lat,
    					mapR.CurrentPosition.Lon
    				)
    			);
    			map.AddCircle (polylineOptions);

    			var a = new Xamarin.Forms.Maps.Position (
                    mapR.CurrentPosition.Lat,
                    mapR.CurrentPosition.Lon
    			);
    		
    			mapR.MoveToRegion(MapSpan.FromCenterAndRadius(a, Distance.FromMeters(5)));
            });
		}

		public void OnMapReady (GoogleMap googleMap)
		{
			map = googleMap;

			var polylineOptions = new PolylineOptions ();
			polylineOptions.InvokeColor (0x66FF0000);

			foreach (var position in calculatedRoute) {
				polylineOptions.Add (new LatLng (position.Lat, position.Lon));
			}
			map.AddPolyline (polylineOptions);


			polylineOptions = new PolylineOptions ();
			polylineOptions.InvokeColor (0x00E5FF00);

			foreach (var position in flownRoute) {
				polylineOptions.Add (new LatLng (position.Lat, position.Lon));
			}
			map.AddPolyline (polylineOptions);
		}
	}
}