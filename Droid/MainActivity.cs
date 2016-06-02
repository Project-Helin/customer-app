using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using PayPal.Forms;
using PayPal.Forms.Abstractions;
using PayPal.Forms.Abstractions.Enum;
using Xamarin.Forms;
using Acr.UserDialogs;

namespace customerapp.Droid
{
    [Activity(Label = "customer-app.Droid", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            Xamarin.FormsMaps.Init(this, bundle);

            CrossPayPalManager.Init(new PayPalConfiguration(
                    PayPalEnvironment.NoNetwork,
                    "APP-80W284485P519543T"
                )
                {
                    AcceptCreditCards = true,
                    //Your business name
                    MerchantName = "Project Helin Store",
                    //Your privacy policy Url
                    MerchantPrivacyPolicyUri = "https://www.example.com/privacy",
                    //Your user agreement Url
                    MerchantUserAgreementUri = "https://www.example.com/legal"
                }
            );

			Websockets.Droid.WebsocketConnection.Link();

			// for Toast messages
			// see https://github.com/aritchie/userdialogs
			UserDialogs.Init(this);


            LoadApplication(new App());
        }


        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            PayPalManagerImplementation.Manager.OnActivityResult(requestCode, resultCode, data);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            PayPalManagerImplementation.Manager.Destroy();
        }
    }
}

