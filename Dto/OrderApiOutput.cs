using System;
using customerapp.Dto;
using System.Collections.Generic;

namespace customerapp
{
	public class OrderApiOutput
	{
		public Position DeliveryPosition{ get; set;}

		public String orderId { get; set; }

		public Route Route { get; set; }

		public List<Mission> Missions { get; set; }


	}
}

