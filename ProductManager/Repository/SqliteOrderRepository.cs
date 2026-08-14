using Dapper;
using Microsoft.Data.Sqlite;
using ProductManager.Component.OrderComponents;
using ProductManager.Interface;
using ProductManager.Services;

namespace ProductManager.Repository
{
	public class SqliteOrderRepository : IOrderRepository
	{
		private readonly string _connectionString = "Data Source=supermarket.db;Foreign Keys=True;";

		public bool CreateOrder(Order order)
		{
			// Open connection
			using var connection = new SqliteConnection(_connectionString);
			connection.Open();

			// Create transaction
			using var transaction = connection.BeginTransaction();

			try
			{
				string sqlInsertOrder = @"
                INSERT INTO Orders (TotalAmount) 
                VALUES (@TotalAmount);
                SELECT last_insert_rowid();";

				int orderId = connection.QuerySingle<int>(
					sqlInsertOrder,
					new { order.TotalAmount },
					transaction: transaction
				);

				order.Id = orderId;

				string sqlInsertItem = @"
                INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice, LineTotal)
                VALUES (@OrderId, @ProductId, @Quantity, @UnitPrice, @LineTotal);";

				string sqlUpdateStock = @"
                UPDATE Products 
                SET Stock = Stock - @Quantity 
                WHERE Id = @ProductId;";

				foreach (var item in order.Items)
				{
					item.OrderId = orderId;

					connection.Execute(sqlInsertItem, item, transaction: transaction);

					connection.Execute(sqlUpdateStock,
						new { item.Quantity, item.ProductId },
						transaction: transaction);
				}

				transaction.Commit();
				return true;
			}
			catch (Exception ex)
			{
				transaction.Rollback();
				ConsoleLogger.LogError($"Transaction failed: {ex.Message}");
				return false;
			}
		}

		public IEnumerable<Order> GetAll()
		{
			using var connection = new SqliteConnection(_connectionString);

			string sqlOreder = @"SELECT * FROM Orders ORDER BY CreatedAt DESC;";
			var orders = connection.Query<Order>(sqlOreder).ToList();

			if (!orders.Any()) return orders;

			string sqlItem = "SELECT * FROM OrderItems;";
			var allItems = connection.Query<OrderItem>(sqlItem).ToList();

			var itemsGroupByOrder = allItems.GroupBy(i => i.OrderId).ToDictionary(g => g.Key,g => g.ToList());

			foreach (var order in orders)
			{
				if(itemsGroupByOrder.TryGetValue(order.Id, out var item))
				{
					order.Items = item;
				}
			}

			return orders;
		}

		public Order? GetById(int id)
		{
			using var connection = new SqliteConnection(_connectionString);

			string sqlOrder = @"SELECT * FROM Orders WHERE Id = @Id;";
			var order = connection.QuerySingleOrDefault(sqlOrder, new { Id = id });

			if(order == null) return null;

			string sqlItems = "SELECT * FROM OrderItems WHERE OrderId = @OrderId;";
			var item = connection.QuerySingleOrDefault(sqlItems, new { OrderId = id });

			order.Items = item;
			return order;
		}

		public void ClearAll()
		{
			using var connection = new SqliteConnection(_connectionString);

			string deleteOrders = @"
				PRAGMA foreign_keys = OFF;
				DELETE FROM Orders;
				DELETE FROM sqlite_sequence WHERE name='Orders';
				PRAGMA foreign_keys = ON;";

			string deleteOrderItems = @"
				PRAGMA foreign_keys = OFF;
				DELETE FROM OrderItems;
				DELETE FROM sqlite_sequence WHERE name='OrderItems';
				PRAGMA foreign_keys = ON;";

			connection.Execute(deleteOrderItems);
			connection.Execute(deleteOrders);
		}
	}
}
