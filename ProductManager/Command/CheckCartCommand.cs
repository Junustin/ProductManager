using ProductManager.Component.CartComponents;
using ProductManager.Component.OrderComponents;
using ProductManager.Interface;
using ProductManager.Services;
using Spectre.Console;

namespace ProductManager.Command
{
	public class CheckCartCommand : ICommand
	{
		public string Name => "Checkcart";

		public string Description => "Check item list in cart";
		private readonly IProductRepository _productRepository;
		private readonly ShoppingCart _cart;

		public CheckCartCommand(IProductRepository productRepository, ShoppingCart cart)
		{
			_productRepository = productRepository;
			_cart = cart;
		}

		public void Execute(string[] args)
		{
			// Check empty cart
			if (!_cart.Items.Any())
			{
				ConsoleLogger.LogError($"Cart are empty");
				return;
			}
			
			var grid = new Grid().AddColumn().AddColumn();
			grid.AddRow("[bold white]Product name[/]", "[bold white]Quantity[/]");
			
			foreach (var item in _cart.Items)
			{
				var product = _productRepository.GetById(item.ProductId);
				if (product != null)
					grid.AddRow($"{product.Name}", $"{item.Quantity}");
				else
					grid.AddRow($"{item.ProductId}", $"{item.Quantity}");
			}

			AnsiConsole.Write(
					new Panel(grid)
						.Header("[bold green] Cart item list [/]")
						.BorderColor(Color.Green)
						.Padding(1, 1)
				);
		}
	}
}
