using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace customerapp.Dto
{
    public class Order
    {
		
		public String Id { get; set; }

		public List<Mission> Missions { get; set; }

		public List<OrderProduct> OrderProducts { get; set; }
		   
		public String State{ get; set; }

        public String CreatedAt{ get; set; }
    }
}

