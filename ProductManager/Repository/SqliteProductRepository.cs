using Dapper;
using Microsoft.Data.Sqlite;
using ProductManager.Component.ProductComponents.Data;
using ProductManager.Interface;

namespace ProductManager.Repository
{
	public class SqliteProductRepository : IProductRepository
	{
		private readonly string _connectionString = "Data Source=supermarket.db;Foreign Keys=True;";

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

		public bool Update(Product product)
		{
			using var connection = new SqliteConnection(_connectionString);

			string sql = @"
				UPDATE Products
				SET Name = @Name, Price = @Price, Stock = @Stock
				WHERE Id = @Id;";

			int rowsAffected = connection.Execute(sql, product);
			return rowsAffected > 0;
		}

		public IEnumerable<Product> GetAll()
		{
			using var connection = new SqliteConnection(_connectionString);

			string sql = @"SELECT Id, Name, Price, Stock FROM Products";

			return connection.Query<Product>(sql);
		}

		public Product? GetById(int id)
		{
			using var connection = new SqliteConnection(_connectionString);

			string sql = @"SELECT Id, Name, Price, Stock FROM Products WHERE Id = @Id";

			return connection.QuerySingleOrDefault<Product>(sql, new {Id = id});
		}

		public bool Delete(int id)
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
				PRAGMA foreign_keys = OFF;
				DELETE FROM Products;
				DELETE FROM sqlite_sequence WHERE name='Products';
				PRAGMA foreign_keys = ON;";

			connection.Execute(sql);
		}
	}
}
