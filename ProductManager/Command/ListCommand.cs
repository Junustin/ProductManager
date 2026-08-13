using ProductManager.Features.ProductComponents.Data;
using ProductManager.Features.Storage;
using ProductManager.Interface;
using ProductManager.Sqlite;
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

		private readonly IProductRepository _repo;

		// Inject product storage on construct command
		public ListCommand(IProductRepository repo)
		{ 
			_repo = repo;
		}

		public void Execute(string[] args)
		{
			var products = _repo.GetAll();
			if (!products.Any())
			{
				Console.WriteLine("No product in storage");
				return;
			}

			Console.WriteLine("\nStorage list:");
			foreach (var product in products)
			{
				Console.WriteLine($"[ID: {product.Id}] {product.Name} ${product.Price} Stock: {product.Stock}");
			}
		}
	}
}
