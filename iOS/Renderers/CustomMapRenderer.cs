using System;
using Xamarin.Forms;
using customerapp;
using MapOverlay.iOS;
using Xamarin.Forms.Maps.iOS;
using MapKit;
using Xamarin.Forms.Platform.iOS;
using CoreLocation;
using UIKit;
using System.ComponentModel;

[assembly:ExportRenderer (typeof(MapWithRoute), typeof(CustomMapRenderer))]
namespace MapOverlay.iOS
{
	public class CustomMapRenderer : MapRenderer
	{
		MKPolylineRenderer polylineRenderer;
        MKCircle circleOverlay;
        MKCircleRenderer circleRenderer;
        MKMapView nativeMap;
        MapWithRoute mapWithRoute {
            get;
            set;
        }

		protected override void OnElementChanged (ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged (e);

			if (e.OldElement != null) {
				var nativeMap = Control as MKMapView;
				nativeMap.OverlayRenderer = null;
			}

			if (e.NewElement != null) {
				var formsMap = (MapWithRoute)e.NewElement;
				nativeMap = Control as MKMapView;
                mapWithRoute = formsMap;

                nativeMap.OverlayRenderer = GetOverlayRenderer;
                nativeMap.OverlayRenderer = (m, o) => {
                    if (circleRenderer == null) {
                        var circle = o as MKCircle;
                        if(circle != null){
                            circleRenderer = new MKCircleRenderer (circle);
                            circleRenderer.FillColor = UIColor.Blue;
                            circleRenderer.Alpha = 0.5f;    
                        }
                    }
                    return circleRenderer;
                };

                formsMap.PropertyChanged += OnChange;

                addOverLayForCalulatedRoute (formsMap, nativeMap);
                addOverLayForFlownRoute (formsMap, nativeMap);
			}
		}


        public void OnChange(object o, PropertyChangedEventArgs ar){
            Device.BeginInvokeOnMainThread( () => {
                var pos = mapWithRoute.CurrentPosition;
                var cords = new CLLocationCoordinate2D(pos.Lat, pos.Lon);
                circleOverlay = MKCircle.Circle(cords, 20);
                nativeMap.AddOverlay(circleOverlay);
            });
        }


        private void addOverLayForCalulatedRoute(MapWithRoute mapWithRoute, MKMapView nativeMap){

            CLLocationCoordinate2D[] coords = new CLLocationCoordinate2D[mapWithRoute.CalculatedRoute.Count];

            int index = 0;
            foreach (var position in mapWithRoute.CalculatedRoute) {
                coords [index] = new CLLocationCoordinate2D (position.Lat, position.Lon);
                index++;
            }

            var routeOverlay = MKPolyline.FromCoordinates (coords);

            nativeMap.AddOverlay (routeOverlay);
        }

        private void addOverLayForFlownRoute(MapWithRoute mapWithRoute, MKMapView nativeMap){

            CLLocationCoordinate2D[] coords = new CLLocationCoordinate2D[mapWithRoute.FlownRoute.Count];

            int index = 0;
            foreach (var position in mapWithRoute.FlownRoute) {
                coords [index] = new CLLocationCoordinate2D (position.Lat, position.Lon);
                index++;
            }

            var routeOverlay = MKPolyline.FromCoordinates (coords);

            nativeMap.AddOverlay (routeOverlay);
        }

		MKOverlayRenderer GetOverlayRenderer (MKMapView mapView, IMKOverlay overlay)
		{
            if (polylineRenderer == null) {
                var poly = overlay as MKPolygon;
                if (poly != null) {
                    polylineRenderer = new MKPolylineRenderer (overlay as MKPolyline);
                    polylineRenderer.FillColor = UIColor.Blue;
                    polylineRenderer.StrokeColor = UIColor.Red;
                    polylineRenderer.LineWidth = 3;
                    polylineRenderer.Alpha = 0.4f;
                }
			}
			return polylineRenderer;
		}
		}
}
