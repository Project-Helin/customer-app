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
        public static string AuthorizeUrl = "https://accounts.google.com/o/oauth2/auth";
        public static string AccessTokenUrl = "https://accounts.google.com/o/oauth2/token";

		/**
		 * See https://developers.google.com/+/web/api/rest/oauth#profile
		 * for more information what the meaning of the scope is.
		 */
		public static string Scope = "profile email";
		/** 
		 * See https://developers.google.com/+/web/api/rest/openidconnect/getOpenIdConnect 
		 * for more information
		 */
		public static string UserInfoUrl = "https://www.googleapis.com/plus/v1/people/me/openIdConnect";

        /**
         * Set this property to the location the user will be redirected too after successfully authenticating
         * Be aware, that this need to be registred in google Developer Console.
         */
        public static string RedirectUrl =  "http://blank.org";

        // public static string Server = "192.168.188.26:9000";
		public static string Server = "151.80.44.117:80"; // ifnotelse.com

		public static string ApiUrlBase = "http://" + Server;
        public static string ApiUrlListProducts = ApiUrlBase + "/api/products/?lat={0}&lon={1}";
		public static string ApiUrlListProductsAll = ApiUrlBase + "/api/products/";
		public static string ApiUrlListOrder = ApiUrlBase + "/api/orders/";

		public static string ApiUrlConfirmOrder = ApiUrlBase + "/api/orders/{0}/confirm/{1}";
		public static string ApiUrlDeleteOrder = ApiUrlBase + "/api/orders/{0}/delete";

		public static string ApiUrlSaveCustomer = ApiUrlBase + "/api/customers/save";
		public static string ApiUrlCustomerFind = ApiUrlBase + "/api/customers/{0}";

		public static string ApiUrlOrdersByCustomer = ApiUrlBase + "/api/orders/find-by-customer/{0}";
		public static string ApiUrlMissionsByOrder = ApiUrlBase + "/api/missions/find-by-order/{0}";

		public static string WsForDroneInfos = Server + "/api/missions/{0}/ws";
    }
}
