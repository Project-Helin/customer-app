using System;
using customerapp.Dto;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xamarin.Auth;

namespace customerapp
{
	public interface IRestService
	{
        Task<List<Product>> GetAllProductsByLocation (Position customerPosition);

		Task<List<Order>> GetAllOrders (String customerId);

		Task<List<Mission>>  GetAllMissions (String orderId);

		Task ConfirmOrder(String orderId, String customerId);

        Task<Order> CreateOrder (ICollection<Product> orderProducts, Position customerPosition);

		Task DeleteOrder (string orderId);

		Task<Customer> GetCustomerInfo (Account account);

		Task<Customer> SaveCustomer (Customer c);

		Task<Customer> GetCustomerById (String customerId);
			
	}
}

