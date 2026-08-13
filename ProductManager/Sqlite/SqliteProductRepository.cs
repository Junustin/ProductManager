using Dapper;
using Microsoft.Data.Sqlite;
using ProductManager.Features.ProductComponents.Data;
using ProductManager.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.Sqlite
{
	public class SqliteProductRepository : IProductRepository
	{
		private readonly string _connectionString;

		public SqliteProductRepository(string connectionString = "Data Source=supermarket.db")
		{
			_connectionString = connectionString;
		}

		public void InitilizeDatabase()
		{
			using var connection = new SqliteConnection(_connectionString);

			string sql = @"
				CREATE TABLE IF NOT EXISTS Products (
				Id INTEGER PRIMARY KEY AUTOINCREMENT,
				Name TEXT NOT NULL,
				Price DECIMAL NOT NULL,
				Stock INTEGER NOT NULL);";

			// Dapper's execute
			connection.Execute(sql);
		}

		public Product Add(Product product)
		{
			using var connection = new SqliteConnection(_connectionString);

			string sql = @"
				INSERT INTO Products (Name, Price, Stock)
				VALUES (@Name, @Price, @Stock);
				SELECT last_insert_rowid();";

			int newId = connection.QuerySingle<int>(sql, product);

			return new Product(newId, product.Name, product.Price, product.Stock);
		}

		public IEnumerable<Product> GetAll()
		{
			using var connection = new SqliteConnection(_connectionString);

			string sql = @"SELECT Id, Name, Price, Stock FROM Products";

			return connection.Query<Product>(sql);
		}

		public bool Remove(int id)
		{
			using var connection = new SqliteConnection(_connectionString);

			string sql = @"DELETE FROM Products WHERE Id = @Id";

			int rowAffected = connection.Execute(sql, new { Id = id });

			return rowAffected > 0;	
		}

		public void ClearAll()
		{
			using var connection = new SqliteConnection(_connectionString);

			string sql = @"
				DELETE FROM Products;
				DELETE FROM sqlite_sequence WHERE name='Products';";

			connection.Execute(sql);
		}
	}
}
