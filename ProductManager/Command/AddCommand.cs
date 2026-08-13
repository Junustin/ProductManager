using ProductManager.Component.ProductComponents.Data;
using ProductManager.Interface;
using ProductManager.Services;

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
			// If invalid amount of arguments
			if (args.Length != 3)
			{
				ConsoleLogger.LogWarning("Please enter correct arguments for Add command Example: Add <ProductName> <Price> <Stock>");
				return;
			}

			// Check for correct arguments type
			string productName = args[0];

			if (decimal.TryParse(args[1], out decimal price))
			{
				if (price <= 0)
				{
					ConsoleLogger.LogError("Price must positive number.");
					return;
				}
			}
			
			if (!int.TryParse(args[2], out int stock))
			{
				ConsoleLogger.LogError("Stock must be non-negative whole number.");
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
			ConsoleLogger.LogSuccess($"Successfully added {productName} ID: {createdProduct.Id}");
		}
	}
}
