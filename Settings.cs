using System;
using Plugin.Settings;

namespace customerapp
{
	public class Settings
	{
		public static void SetCustomerId (string id)
		{
			CrossSettings.Current.AddOrUpdateValue<string> ("cusotmerId", id);
		}

		public static string GetCustomerIdOrNull ()
		{
			return CrossSettings.Current.GetValueOrDefault<string> ("cusotmerId", null);
		}

	}
}

