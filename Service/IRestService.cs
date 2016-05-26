using System;
using customerapp.Dto;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xamarin.Auth;

namespace customerapp
{
	public interface IRestService
	{
		Task<List<Product>> GetAllProducts ();

		Task ConfirmOrder(String orderId, String customerId);

		Task<Order> CreateOrder (ICollection<Product> orderProducts);

		Task DeleteOrder (string orderId);

		Task<Customer> GetCustomerInfo (Account account);

		Task<Customer> SaveCustomer (Customer c);

		Task<Customer> GetCustomerById (String customerId);
			
	}
}

