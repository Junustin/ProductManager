using Microsoft.Data.Sqlite;
using Dapper;

namespace ProductManager.Services
{
	public static class DatabaseInitializer
	{
		private static readonly string ConnectionString = "Data Source=supermarket.db;Foreign Keys=True;";
        public static void Initialize()
        {

			using var connection = new SqliteConnection(ConnectionString);
			connection.Open();

			// 1. Enable Foreign Key Enforcement
			string enableFkSql = "PRAGMA foreign_keys = ON;";
			connection.Execute(enableFkSql);

			// 2. Create Products Table
			string createProductsTable = @"
            CREATE TABLE IF NOT EXISTS Products (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Price REAL NOT NULL,
                Stock INTEGER NOT NULL
            );";

			// 3. Create Orders Table
			string createOrdersTable = @"
            CREATE TABLE IF NOT EXISTS Orders (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                CreatedAt TEXT NOT NULL DEFAULT (datetime('now', 'localtime')),
                TotalAmount REAL NOT NULL
            );";

			// 4. Create OrderItems Table (Depends on Products and Orders)
			string createOrderItemsTable = @"
            CREATE TABLE IF NOT EXISTS OrderItems (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                OrderId INTEGER NOT NULL,
                ProductId INTEGER NOT NULL,
                Quantity INTEGER NOT NULL,
                UnitPrice REAL NOT NULL,
                LineTotal REAL NOT NULL,
                FOREIGN KEY (OrderId) REFERENCES Orders(Id) ON DELETE CASCADE,
                FOREIGN KEY (ProductId) REFERENCES Products(Id)
            );";

			connection.Execute(createProductsTable);
			connection.Execute(createOrdersTable);
			connection.Execute(createOrderItemsTable);
		}
		
	}
}
