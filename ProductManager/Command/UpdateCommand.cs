using ProductManager.Features.ProductComponents.Data;
using ProductManager.Interface;
using ProductManager.Services;

namespace ProductManager.Command
{
	public class UpdateCommand : ICommand
	{
		public string Name => "update";

		public string Description => "Update entry in database";
		private readonly IProductRepository _repo;
		public UpdateCommand(IProductRepository repo) 
		{ 
			_repo = repo;
		}

		public void Execute(string[] args)
		{
			if(args.Length == 4)
			{
				FullUpdate(args);
			}
			else if(args.Length == 3)
			{
				if (!int.TryParse(args[0], out int id) || id <= 0)
				{
					ConsoleLogger.LogError("ID must be a positive integer.");
					return;
				}

				// Fetch product from database by Id
				var product = _repo.GetById(id);
				if (product == null)
				{
					ConsoleLogger.LogError($"No product with ID: {id} found.");
					return;
				}

				string valueToUpdate = args[1].ToLower();
				switch(valueToUpdate)
				{
					case "name":
						product.Name = args[2];
						break;
					case "price":
						if (decimal.TryParse(args[2], out decimal price) || price <= 0)
						{
							product.Price = price;
						}
						else
						{
							ConsoleLogger.LogError("Price must be a positive number.");
							return;
						}
						break;
					case "stock":
						if (int.TryParse(args[3], out int stock) || stock < 0)
						{
							product.Stock = stock;
						}
						else
						{
							ConsoleLogger.LogError("Stock must be non-negative whole number");
							return;
						}
						break;
					default:
						ConsoleLogger.LogError("Unknown field. Use name, price, or stock.");
						break;
				}

				bool sucess = _repo.Update(product);

				if (sucess)
				{
					ConsoleLogger.LogSuccess($"Updated product ID: {id} to '{product.Name}' '${product.Price:F2}' '{product.Stock}.'");
				}
				else
					ConsoleLogger.LogError($"No product with ID: {id} found.");

			}
			else
			{
				ConsoleLogger.LogWarning("Usage: update <ID> <Name> <Price> <Stock>");
				return;
			}
		}

		private void FullUpdate(string[] args)
		{
			if (!int.TryParse(args[0], out int id) || id <= 0)
			{
				ConsoleLogger.LogError("ID must be a positive integer.");
				return;
			}

			string name = args[1];

			if (!decimal.TryParse(args[2], out decimal price) || price <= 0)
			{
				ConsoleLogger.LogError("Price must be a positive number.");
				return;
			}

			if (!int.TryParse(args[3], out int stock) || stock < 0)
			{
				ConsoleLogger.LogError("Stock must be non-negative whole number");
				return;
			}

			var updatedProduct = new Product
			{
				Id = id,
				Name = name,
				Price = price,
				Stock = stock
			};

			bool sucess = _repo.Update(updatedProduct);

			if (sucess)
			{
				ConsoleLogger.LogSuccess($"Updated product ID: {id} to '{name}' '${price:F2}' '{stock}.'");
			}
			else
				ConsoleLogger.LogError($"No product with ID: {id} found.");
		}
	}
}
