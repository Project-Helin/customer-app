using System;
using System.Collections.Generic;

namespace customerapp.Dto
{
    public class Order
    {
        public Position DeliveryPosition{ get; set;}
        public DateTime OrderTime { get; set; }
        public Product Product { get; set; }

		public List<Product> Products {
			get;
			set;
		}
           
    }
}

