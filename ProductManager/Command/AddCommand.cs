using ProductManager.Features.ProductComponents.Data;
using ProductManager.Interface;
using ProductManager.Sqlite;

namespace ProductManager.Command
{
	public class AddCommand : ICommand
	{
		public string Name => "Add";

		public string Description => "Add prodcut to the storage";
		private readonly IProductRepository _repo;

		public AddCommand(IProductRepository repo)
		{
			_repo = repo;
		}

		public void Execute(string[] args)
		{
			// Create new product from argument or pass to factory to create it
			// Should validate the product data e.g. the same product name already exist in database if that is the case maybe return error 
			// or ask user if they want to update the data of that product instead.
			// Tell storage or database to add this new product to storage

			// If invalid amount of arguments
			if (args.Length != 3)
			{
				Console.WriteLine("Please enter correct arguments for Add command Example: Add <ProductName> <Price> <Stock>");
				return;
			}

			// Check for correct arguments type
			string productName = args[0];

			if (decimal.TryParse(args[1], out decimal price))
			{
				if (price <= 0)
				{
					Console.WriteLine("Price must positive number.");
					return;
				}
			}
			else
			{
				Console.WriteLine("Price must be positive number.");
				return;
			}

			if (!int.TryParse(args[2], out int stock))
			{
				Console.WriteLine("Error: Stock must be non-negative whole number.");
				return;
			}

			var productToCreate = new Product
			{
				Name = productName,
				Price = price,
				Stock = stock
			};

			// Add product to list
			Product createdProduct = _repo.Add(productToCreate); // Add new product ot list
			Console.WriteLine($"Successfully added {productName} [ID: {createdProduct.Id}]");
		}
	}
}
