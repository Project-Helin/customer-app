using System;
using customerapp.Dto;

namespace customerapp
{
	public class OrderApiOutput
	{
		public Position DeliveryPosition{ get; set;}

		public String orderId { get; set; }

		public Route Route { get; set; }

	}
}

