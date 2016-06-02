using System;
using System.Diagnostics;
using Plugin.Geolocator;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

namespace customerapp
{
    public class PositionHelper
    {
        public static async Task<customerapp.Dto.Position> GetPositionOrNull (Page parentPage)
        {
            try
            {
                var status = await AskForPermissionToGetGps(parentPage);

                var userCanReadGps = (status == PermissionStatus.Granted);
                if (userCanReadGps)
                {
                    return await ReadGpsPosition();
                }
                else if(status != PermissionStatus.Unknown)
                {
                    await parentPage.DisplayAlert("Location Denied", "Can not continue, try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.ShowError (ex.Message);
                return null;
            }
            return null;
        }

        public static async Task<PermissionStatus> AskForPermissionToGetGps(Page parentPage){
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);

            var hasNoPermission = (status != PermissionStatus.Granted);
            if (hasNoPermission)
            {
                if(await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                {
                    await parentPage.DisplayAlert("Need location", "Gunna need that location", "OK");
                }

                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] {Permission.Location});
                status = results[Permission.Location];
            }

            return status;
        }

        public static async Task<customerapp.Dto.Position> ReadGpsPosition(){
            UserDialogs.Instance.ShowLoading ("Waiting for GPS position");

            var locator = CrossGeolocator.Current;

            Debug.WriteLine ("Try to get current position");
            Plugin.Geolocator.Abstractions.Position pos = await locator.GetPositionAsync (timeoutMilliseconds: 100000); ;

            customerapp.Dto.Position p = new customerapp.Dto.Position ();
            p.Lat= pos.Latitude;
            p.Lon = pos.Longitude;              

            Debug.WriteLine ("Got position");

            UserDialogs.Instance.HideLoading ();

            return p;
        }

    }
}

