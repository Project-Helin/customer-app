using Newtonsoft.Json;
using System;
using System.Linq;
using TodoAWSSimpleDB;
using TodoAWSSimpleDB.iOS;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using customerapp;
using System.Diagnostics;
using Plugin.Settings;

[assembly: ExportRenderer(typeof(AuthenticationPage), typeof(AuthenticationPageRenderer))]

namespace TodoAWSSimpleDB.iOS
{
    // Use a custom page renderer to display the authentication UI on the AuthenticationPage
    public class AuthenticationPageRenderer : PageRenderer
    {
        bool isShown;
	IRestService rest = App.Rest;

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (!App.IsLoggedIn)
            {
                if (!isShown)
                {
                    isShown = true;

                    // Initialize the object that communicates with the OAuth service
                    var auth = new OAuth2Authenticator(
                        KeyConstants.ClientId,
                        KeyConstants.ClientSecret,
                        Constants.Scope,
                        new Uri(Constants.AuthorizeUrl),
                        new Uri(Constants.RedirectUrl), 
                        new Uri(Constants.AccessTokenUrl));

                    // Register an event handler for when the authentication process completes
                    auth.Completed += OnAuthenticationCompleted;

                    // Display the UI
                    PresentViewController(auth.GetUI(), true, null);
                }
            }
            else
            {
                if (!isShown)
                {
                    App.SuccessfulLoginAction.Invoke();					
                }
            }
        }

        async void OnAuthenticationCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
				App.Customer = await App.Rest.GetCustomerInfo (e.Account);
				App.Customer = await App.Rest.SaveCustomer (App.Customer);
				Settings.SetCustomerId (App.Customer.Id);
            }
			
            App.SuccessfulLoginAction.Invoke();
        }
    }
}
