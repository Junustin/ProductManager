using ProductManager.Features.Storage;
using ProductManager.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.Command
{
	public class ListCommand : ICommand
	{
		public string Name => "List";

		public string Description => "Display all product in database.";

		private readonly ProductStorage _storage;

		// Inject product storage on construct command
		public ListCommand(ProductStorage storage)
		{ 
			_storage = storage;
		}

		public void Execute(string[] args)
		{
			if (_storage.GetAllProducts().Count == 0)
			{
				Console.WriteLine("No product in storage");
				return;
			}

			Console.WriteLine("\nStorage list:");

			var products = _storage.GetAllProducts();
			foreach (var product in products)
			{
				Console.WriteLine($"[ID: {product.Id}] {product.Name} ${product.Price} Stock: {product.Stock}");
			}
		}
	}
}
