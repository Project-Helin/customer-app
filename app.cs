using System;

using Xamarin.Forms;

namespace customerapp
{
    public class App : Application
    {
        static NavigationPage NavPage;

        public static bool IsLoggedIn { 
           get { 
//                if (User != null)
//                    return !string.IsNullOrWhiteSpace (User.Email);
//                else
//                    return false;
                return false;
            } 
        }

        public static Action SuccessfulLoginAction {
            get {
                return new Action (() => { 
//                    NavPage.Navigation.PopModalAsync ();                    
//
//                    if (IsLoggedIn) {
//                        NavPage.Navigation.PushAsync(new MainPage());
//                    }
                });
            }
        }

        public App()
        {
            // The root page of your application
            MainPage = NavPage = new NavigationPage (new AuthenticationPage());
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

