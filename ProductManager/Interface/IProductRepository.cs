using ProductManager.Features.ProductComponents.Data;

namespace ProductManager.Interface
{
	public interface IProductRepository
	{
		void InitilizeDatabase();
		Product Add(Product product);
		bool Update(Product product);
		IEnumerable<Product> GetAll();
		Product? GetById(int id);
		bool Remove(int idToRemove);
		void ClearAll();
	}
}
