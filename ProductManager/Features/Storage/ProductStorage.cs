using ProductManager.Features.ProductComponents.Data;

namespace ProductManager.Features.Storage
{
	// Temporary class for interal memmory product storage (Will replace with database later)
	public class ProductStorage
	{
		private readonly List<Product> storage = new();

		public Product AddNewProduct(Product product)
		{
			storage.Add(product);
			return product;
		}
		public IReadOnlyList<Product> GetAllProducts()
		{
			return storage;
		}
	}
}
