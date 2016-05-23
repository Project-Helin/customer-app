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

namespace customerapp
{
	public class RestService
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

		public async Task ConfirmOrder(String orderId){
			var uri = new Uri (string.Format(Constants.ApiUrlConfirmtOrder, orderId));
			Debug.WriteLine ("URI " + uri);
			var content = new StringContent ("", Encoding.UTF8);
			HttpResponseMessage response = await client.PostAsync (uri, content);

			if (!response.IsSuccessStatusCode) {
				Debug.WriteLine ("Failed to create order with status code " + response.StatusCode);
			}
		
		}

		public async Task<OrderApiOutput> CreateOrder (ICollection<Product> orderProducts)
		{
			var uri = new Uri (Constants.ApiUrlListOrder);
			Debug.WriteLine ("URI " + uri);

			var pos = await GetPosition();

			var aa = orderProducts.GetEnumerator ();
			aa.MoveNext ();
			var first = aa.Current.ProjectId;

			var v = new {
				displayName = "Batman",
				email = "batman@wayneenterprise.com",
				customerPosition = pos,
				projectId = first,
				orderProducts = orderProducts
			};

			var jsonToSend = JsonConvert.SerializeObject (v, jsonSetting);
			var contentToSend = new StringContent (jsonToSend, Encoding.UTF8, "application/json");

			HttpResponseMessage response = await client.PostAsync (uri, contentToSend);

			if (response.IsSuccessStatusCode) {
				var content = await response.Content.ReadAsStringAsync();

				Debug.WriteLine (content);
				OrderApiOutput output = Newtonsoft.Json.JsonConvert.DeserializeObject <OrderApiOutput> (content, jsonSetting); 
				return output;
			} else {
				Debug.WriteLine ("Failed to create order with status code " + response.StatusCode);
				return null;
			}

		}

	}

}

