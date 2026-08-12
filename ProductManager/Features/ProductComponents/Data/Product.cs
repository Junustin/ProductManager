using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.Features.ProductComponents.Data
{
	public class Product
	{
		private static int nextId = 0;
		public int Id { get; }
		public string Name { get; set; } = string.Empty;
		public decimal Price { get; set; } = decimal.Zero;
		public int Stock { get; set; } = 0;

		public Product(string name, decimal price, int stock)
		{
			Id = ++nextId; // Increment before assignment
			Name = name;
			Price = price;
			Stock = stock;
		}
	}
}
