using ProductManager.Features.ProductComponents.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.Interface
{
	public interface IProductRepository
	{
		void InitilizeDatabase();
		Product Add(Product product);
		IEnumerable<Product> GetAll();
	}
}
