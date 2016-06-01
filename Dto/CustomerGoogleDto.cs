using System;
using System.Json;
using Newtonsoft.Json;

namespace customerapp
{
	/*
	 * This is how the customer is returend 
	 * from the Google REST call
	 */ 
	[JsonObject]
	public class CustomerGoogleDto
	{
		public string Email { get; set; }

		[JsonProperty ("given_name")]
		public string GivenName { get; set; }

		[JsonProperty ("family_name")]
		public string FamilyName { get; set; }

	}
}

