using ProductManager.Component.ProductComponents.Data;

namespace ProductManager.Interface
{
	public interface IProductRepository
	{
		Product Add(Product product);
		bool Update(Product product);
		IEnumerable<Product> GetAll();
		Product? GetById(int id);
		bool Remove(int idToRemove);
		void ClearAll();
	}
}
