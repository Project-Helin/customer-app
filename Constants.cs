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
        public static string RedirectUrl = "https://blank.org/";

		public static string ApiUrlBase = "http://152.96.233.42:9000";
		// public static string ApiUrlBase = "http://152.96.236.122:9000";
		// public static string ApiUrlBase = "http://151.80.44.117:80";
		public static string ApiUrlListProducts = ApiUrlBase + "/api/products/find-by-location/{0}/{1}";
		public static string ApiUrlListProductsAll = ApiUrlBase + "/api/products/";
		public static string ApiUrlListOrder = ApiUrlBase + "/api/orders/";
		public static string ApiUrlConfirmOrder = ApiUrlBase + "/api/orders/{0}/confirm";
		public static string ApiUrlCancelOrder = ApiUrlBase + "/api/orders/{0}/cancel";
    }
}