using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.Features.ProductComponents.Data
{
	public class Product
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public decimal Price { get; set; } = decimal.Zero;
		public int Stock { get; set; } = 0;

		public Product() { }

		public Product(int id,string name, decimal price, int stock)
		{
			Id = id;
			Name = name;
			Price = price;
			Stock = stock;
		}
	}
}
