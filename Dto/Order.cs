using System;
using System.Collections.Generic;

namespace customerapp.Dto
{
    public class Order
    {


		public String Id { get; set; }

		public Route Route { get; set; }

		public List<Mission> Missions { get; set; }



        public Position DeliveryPosition{ get; set;}
        public DateTime OrderTime { get; set; }
        public Product Product { get; set; }

		public List<Product> Products {
			get;
			set;
		}
           
    }
}

