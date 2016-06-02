using System;
using Newtonsoft.Json;

namespace customerapp
{
	[JsonObject]
	public class Customer
	{
		public string Id { get; set; }

		public string Email { get; set; }

		[JsonProperty ("given_name")]
		public string GivenName { get; set; }

		[JsonProperty ("family_name")]
		public string FamilyName { get; set; }



	}
}
