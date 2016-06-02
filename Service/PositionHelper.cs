using System;
using System.Diagnostics;
using Plugin.Geolocator;
using System.Threading.Tasks;
using Acr.UserDialogs;

namespace customerapp
{
    public class PositionHelper
    {
        public static async Task<customerapp.Dto.Position> GetPosition ()
        {


            UserDialogs.Instance.ShowLoading ("Waiting for GPS position");

            Plugin.Geolocator.Abstractions.Position pos = null;
            Debug.WriteLine ("Try to get current position");
            try{
                var locator = CrossGeolocator.Current;
                pos = await locator.GetPositionAsync (timeoutMilliseconds: 100000); 
            }catch(Exception e){
                Debug.WriteLine ("Got position", e);
                return null;
            }


            customerapp.Dto.Position p = new customerapp.Dto.Position ();
            p.Lat= pos.Latitude;
            p.Lon = pos.Longitude;              

            Debug.WriteLine ("Got position");

            UserDialogs.Instance.HideLoading ();
            return p;
        }
    }
}

