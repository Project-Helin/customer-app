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
			
			var uri = new Uri (Constants.ApiUrlListProducts);
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
			customerapp.Dto.Position p = new customerapp.Dto.Position ();

			var locator = CrossGeolocator.Current;


			var t = await locator.GetPositionAsync (timeoutMilliseconds: 10000);
			p.Lat= t.Latitude;
			p.Lon = t.Latitude;				
			return p;
		}

		public async Task<OrderApiOutput> CreateOrder (ICollection<Product> orderProducts)
		{
			var uri = new Uri (Constants.ApiUrlListOrder);


			var pos = await GetPosition();


			var v = new {
				displayName = "Batman",
				email = "batman@wayneenterprise.com",
				customerPosition = pos,
				projectId = "c2d05aad-864a-43ed-9fa1-87f9deb90642",
				orderProducts = orderProducts
			};

			var jsonToSend = JsonConvert.SerializeObject (v, jsonSetting);
			var contentToSend = new StringContent (jsonToSend, Encoding.UTF8, "application/json");

			HttpResponseMessage response = await client.PostAsync (uri, contentToSend);

			if (response.IsSuccessStatusCode) {
				var content = await response.Content.ReadAsStringAsync();

				OrderApiOutput output = Newtonsoft.Json.JsonConvert.DeserializeObject <OrderApiOutput> (content); 
				return output;
			} else {
				Debug.WriteLine ("Failed to create order with status code " + response.StatusCode);
				return null;
			}

		}

	}

}

