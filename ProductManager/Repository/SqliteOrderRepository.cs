using Microsoft.Data.Sqlite;
using ProductManager.Interface;

namespace ProductManager.Repository
{
	internal class SqliteOrderRepository : IOrderRepository
	{
		private readonly string _connectionString = "Data Source";
	}
}
