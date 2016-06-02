using System;
using System.Collections.Generic;

namespace customerapp.Dto
{
    public class Order
    {
		
		public String Id { get; set; }

//		public DateTime CreatedAt { set; get; }

		public List<Mission> Missions { get; set; }

		public List<OrderProduct> OrderProducts { get; set; }
		   
		public String State{ get; set; }
    }
}

