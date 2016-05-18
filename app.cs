using System;

using Xamarin.Forms;

namespace customerapp
{
    public class App : Application
    {
        static NavigationPage NavPage;

        public static Customer Customer { get; set; }

        public static bool IsLoggedIn
        { 
            get
            { 
				if (Customer != null) 
				{
					return !string.IsNullOrWhiteSpace (Customer.Email);
				} else 
				{
					return false;
				}
            } 
        }

        public static Action SuccessfulLoginAction
        {
            get
            {
                return new Action(() =>
                    { 
                        if (IsLoggedIn)
                        {
                            NavPage.Navigation.PopModalAsync();    
                        }
                    });
            }
        }

        public App()
        {
            // The root page of your application
            MainPage = NavPage = new NavigationPage(new MainPage());
            //User shouldnt to have to login in order to see Products
            //NavPage.Navigation.PushModalAsync(new AuthenticationPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

