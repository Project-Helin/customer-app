using System;

namespace customerapp
{
    public static class Constants
    {
        // OAuth
        // For Google login, configure at https://console.developers.google.com/
        public static string ClientId = "925780692220-s5ph6nk276u2sjvaarekib83hvj623nd.apps.googleusercontent.com";
        public static string ClientSecret = "zQa7y0KSf0TSPgJv7uDJ_PUT";

        // These values do not need changing
        public static string Scope = "https://www.googleapis.com/auth/userinfo.email";
        public static string AuthorizeUrl = "https://accounts.google.com/o/oauth2/auth";
        public static string AccessTokenUrl = "https://accounts.google.com/o/oauth2/token";
        public static string UserInfoUrl = "https://www.googleapis.com/oauth2/v2/userinfo";

        // Set this property to the location the user will be redirected too after successfully authenticating
        public static string RedirectUrl = "http://blank.org/";
    }
}