using System;
using Newtonsoft.Json;

namespace customerapp
{
	[JsonObject]
	public class Customer
	{
		public string Id { get; set; }

		public string Email { get; set; }

		public string GivenName { get; set; }

		public string FamilyName { get; set; }

	}
}
