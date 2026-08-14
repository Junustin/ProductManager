using ProductManager.Component.OrderComponents;

namespace ProductManager.Interface
{
	public interface IOrderRepository
	{
		/// <summary>
		/// Executes an atomic multi-table transaction:
		/// 1. Inserts the Order header record.
		/// 2. Inserts all OrderItems with price snapshots.
		/// 3. Deducts stock for each purchased item in the Products table.
		/// Returns true if all operations succeed; false if any step fails.
		/// </summary>
		bool CreateOrder(Order order);

		/// <summary>
		/// Retrieves a single order by its ID, including its associated line items.
		/// </summary>
		Order? GetById(int id);

		/// <summary>
		/// Retrieves all orders in the system for history or reporting.
		/// </summary>
		IEnumerable<Order> GetAll();
		void ClearAll();
	}
}
