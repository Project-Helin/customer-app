using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using PayPal.Forms;
using PayPal.Forms.Abstractions;
using PayPal.Forms.Abstractions.Enum;
using Xamarin.Forms;

namespace customerapp.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            Xamarin.FormsMaps.Init();

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

            LoadApplication(new App());
            Websockets.Ios.WebsocketConnection.Link();


            return base.FinishedLaunching(app, options);
        }
    }
}

