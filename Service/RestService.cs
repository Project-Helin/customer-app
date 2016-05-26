using System;
using System.Net.Http;
using System.Collections.Generic;
using customerapp.Dto;
using Newtonsoft.Json;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;

using Xamarin.Forms;

using Plugin.Geolocator;
using Xamarin.Auth;

namespace customerapp
{
	public class RestService : IRestService
	{
		HttpClient client;

		/**
		 * We need this custom setting, so that JSON is created with 
		 * correct propertyName. Without this setting, json Name would be start 
		 * with capital letter as in .Net Property. With this setting
		 * all property names in json start with small letter.
		 */
		JsonSerializerSettings jsonSetting = new JsonSerializerSettings { 
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			NullValueHandling = NullValueHandling.Ignore // ignore null values
		};
				
		public RestService ()
		{
			client = new HttpClient ();
		}

		public async Task<List<Product>> GetAllProducts ()
		{
			Debug.WriteLine ("Fetch order ");

			var position = await GetPosition ();

			string x = position.Lat.ToString();
			string y = position.Lon.ToString();

			// replace is needed to be sure, that double is written xx.zz not xx,zz
			var uri = new Uri (String.Format(Constants.ApiUrlListProducts, x.Replace(',', '.'), y.Replace(',', '.')));
			Debug.WriteLine ("URI " + uri);

			var response = await client.GetAsync (uri);
			List<Product> items = new List<Product>();
			
			if (response.IsSuccessStatusCode) {
				var content = await response.Content.ReadAsStringAsync ();
				items = Newtonsoft.Json.JsonConvert.DeserializeObject <List<Product>> (content);
			} else {
				Debug.WriteLine ("Failed to get all order with status code " + response.StatusCode);
			}

			return items;
		}

		public async Task<List<Order>> GetAllOrders (String customerId)
		{
			Debug.WriteLine ("Fetch order ");

			var position = await GetPosition ();

			string x = position.Lat.ToString();
			string y = position.Lon.ToString();

			// replace is needed to be sure, that double is written xx.zz not xx,zz
			var uri = new Uri (String.Format(Constants.ApiUrlListProducts, x.Replace(',', '.'), y.Replace(',', '.')));
			Debug.WriteLine ("URI " + uri);

			var response = await client.GetAsync (uri);
			List<Order> items = new List<Order>();

			if (response.IsSuccessStatusCode) {
				var content = await response.Content.ReadAsStringAsync ();
				items = Newtonsoft.Json.JsonConvert.DeserializeObject <List<Order>> (content);
			} else {
				Debug.WriteLine ("Failed to get all order with status code " + response.StatusCode);
			}

			return items;
		}



		public async Task<List<Mission>> GetAllMissions (String customerId)
		{
			Debug.WriteLine ("Fetch order ");

			var position = await GetPosition ();

			string x = position.Lat.ToString();
			string y = position.Lon.ToString();

			// replace is needed to be sure, that double is written xx.zz not xx,zz
			var uri = new Uri (String.Format(Constants.ApiUrlListProducts, x.Replace(',', '.'), y.Replace(',', '.')));
			Debug.WriteLine ("URI " + uri);

			var response = await client.GetAsync (uri);
			List<Mission> items = new List<Mission>();

			if (response.IsSuccessStatusCode) {
				var content = await response.Content.ReadAsStringAsync ();
				items = Newtonsoft.Json.JsonConvert.DeserializeObject <List<Mission>> (content);
			} else {
				Debug.WriteLine ("Failed to get all order with status code " + response.StatusCode);
			}

			return items;
		}


		async Task<customerapp.Dto.Position> GetPosition ()
		{
			// TODO handle failure if position is not switched on 

			Debug.WriteLine ("Try to get current position");
			var locator = CrossGeolocator.Current;
			var pos = await locator.GetPositionAsync (timeoutMilliseconds: 100000);

			customerapp.Dto.Position p = new customerapp.Dto.Position ();
			p.Lat= pos.Latitude;
			p.Lon = pos.Longitude;				

			Debug.WriteLine ("Got positoin");
			return p;
		}

		public async Task ConfirmOrder(String orderId, String customerId){
			var uri = new Uri (string.Format(Constants.ApiUrlConfirmOrder, orderId, customerId));
			Debug.WriteLine ("URI " + uri);
			var content = new StringContent ("", Encoding.UTF8);
			HttpResponseMessage response = await client.PostAsync (uri, content);

			if (!response.IsSuccessStatusCode) {
				Debug.WriteLine (
					"Failed to confirm order {0} for customer {1} with status code {2}", 
					response.StatusCode, 
					orderId, 
					customerId
				);
			}
		
		}

		public async Task<Order> CreateOrder (ICollection<Product> orderProducts)
		{
			var uri = new Uri (Constants.ApiUrlListOrder);
			Debug.WriteLine ("URI " + uri);

			var position = await GetPosition();
			var projectId = getProjectId (orderProducts);

			var request = new {
				customerPosition = position,
				projectId = projectId,
				orderProducts = orderProducts
			};

			var jsonToSend = JsonConvert.SerializeObject (request, jsonSetting);
			var contentToSend = new StringContent (jsonToSend, Encoding.UTF8, "application/json");

			HttpResponseMessage response = await client.PostAsync (uri, contentToSend);

			if (response.IsSuccessStatusCode) {
				var content = await response.Content.ReadAsStringAsync();

				Debug.WriteLine ("Create order response {0}", content);
				Order output = Newtonsoft.Json.JsonConvert.DeserializeObject <Order> (content, jsonSetting); 
				return output;
			} else {
				Debug.WriteLine ("Failed to create order with status code " + response.StatusCode);
				return null;
			}

		}

		public async Task DeleteOrder (string orderId)
		{
			var uri = new Uri (string.Format(Constants.ApiUrlDeleteOrder, orderId));
			Debug.WriteLine ("URI " + uri);

			var content = new StringContent ("", Encoding.UTF8);
			HttpResponseMessage response = await client.PostAsync (uri, content);
			Debug.WriteLine ("Delete order {0} successfully", orderId);

			if (!response.IsSuccessStatusCode) {
				Debug.WriteLine ("Failed to delete order {0} with status code {1} ", orderId, response.StatusCode);
			}
		}

		String getProjectId(ICollection<Product> orderProducts){
			/**
			 * Since all products should be from the same project
			 * we can get the project id from the first product
			 */
			var enumerator = orderProducts.GetEnumerator ();
			enumerator.MoveNext ();
			return enumerator.Current.ProjectId;
		}

		public async Task<Customer> GetCustomerInfo (Account account)
		{
			// If the user is authenticated, request their basic user data from Google
			var request = new OAuth2Request("GET", new Uri(Constants.UserInfoUrl), null, account);

			var response = await request.GetResponseAsync();
			if (response != null)
			{
				// Deserialize the data and store it in the account store
				string userJson = response.GetResponseText();
				return JsonConvert.DeserializeObject<Customer>(userJson, jsonSetting);
			}

			return null;
		}

		public async Task<Customer> SaveCustomer (Customer customer)
		{
			var uri = new Uri (Constants.ApiUrlSaveCustomer);
			Debug.WriteLine ("URI " + uri);

			var jsonToSend = JsonConvert.SerializeObject (customer, jsonSetting);
			var contentToSend = new StringContent (jsonToSend, Encoding.UTF8, "application/json");

			HttpResponseMessage response = await client.PostAsync (uri, contentToSend);

			if (response.IsSuccessStatusCode) {
				var content = await response.Content.ReadAsStringAsync();
				Customer output = Newtonsoft.Json.JsonConvert.DeserializeObject <Customer> (content, jsonSetting); 
				return output;
			} else {
				Debug.WriteLine ("Failed to save customer with status code " + response.StatusCode);
				return null;
			}
		}


		public async Task<Customer> GetCustomerById (String customerId)
		{
			Debug.WriteLine ("Fetch customer ");

			var uri = new Uri (String.Format(Constants.ApiUrlCustomerFind, customerId));
			Debug.WriteLine ("URI " + uri);

			var response = await client.GetAsync (uri);
			if (response.IsSuccessStatusCode) {
				var content = await response.Content.ReadAsStringAsync ();
				return Newtonsoft.Json.JsonConvert.DeserializeObject <Customer> (content);
			} else {
				Debug.WriteLine ("Failed to get customer " + customerId + " with status code " + response.StatusCode);
			}

			return null;
		}


	}

}

