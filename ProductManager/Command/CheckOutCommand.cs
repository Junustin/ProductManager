using ProductManager.Component.CartComponents;
using ProductManager.Component.OrderComponents;
using ProductManager.Interface;
using ProductManager.Services;
using Spectre.Console;

namespace ProductManager.Command
{
	public class CheckOutCommand : ICommand
	{
		public string Name => "Checkout";

		public string Description => "Check out Items in shopping cart";

		private readonly IOrderRepository _orderRepository;
		private readonly IProductRepository _productRepository;

		private readonly ShoppingCart _cart;

		public CheckOutCommand(IProductRepository productRepository, IOrderRepository orderRepository, ShoppingCart cart)
		{
			_productRepository = productRepository;
			_orderRepository = orderRepository;
			_cart = cart;
		}

		public void Execute(string[] args)
		{
			// Check empty cart
			if(!_cart.Items.Any())
			{
				ConsoleLogger.LogError($"Cart are empty");
				return;
			}

			// Recheck all item against DB stock
			foreach(var cartItem in _cart.Items)
			{
				var product = _productRepository.GetById(cartItem.ProductId);
				if(product == null)
				{
					ConsoleLogger.LogError($"No product with ID: {cartItem.ProductId} found.");
					return;
				}

				if(product.Stock < cartItem.Quantity)
				{
					ConsoleLogger.LogError($"Not enough stock for {product.Name} Currently in stock: {product.Stock} Request: {cartItem.Quantity}");
					return;
				}
			}

			// Turn cart item list into order item list
			var orderItems = _cart.Items.Select(ci => new OrderItem
			{
				ProductId = ci.ProductId,
				Quantity = ci.Quantity,
				UnitPrice = ci.UnitPrice,
			}).ToList();

			// Calculate order total product price
			decimal totalPrice = orderItems.Sum(i => i.LineTotal);

			// Create order
			Order order = new Order
			{
				CreatedAt = DateTime.UtcNow,
				TotalAmount = totalPrice,
				Items = orderItems	
			};

			// Run atomic transaction
			bool isSuccess = _orderRepository.CreateOrder(order);

			if (!isSuccess)
			{
				// Error log handle by OrderRepository
				return;
			}

			// Transaction complete
			int itemCountTotal = _cart.Items.Sum(i => i.Quantity);
			_cart.Clear();

			// Draw receipt
			var grid = new Grid().AddColumn().AddColumn();
			grid.AddRow("[bold gray]Order ID:[/]", $"[yellow]#{order.Id}[/]");
			grid.AddRow("[bold white]Item ID[/]", $"[bold white]Quantity [/]");
			foreach( var item in orderItems)
			{
				var product = _productRepository.GetById(item.ProductId);
				if(product != null )
					grid.AddRow($"{product.Name}", $"{item.Quantity}");
				else
					grid.AddRow($"{item.ProductId}", $"{item.Quantity}");
			}
			grid.AddRow("[bold gray]Total Items Purchased:[/]", $"[cyan]{itemCountTotal}[/]");
			grid.AddRow("[bold gray]Grand Total Paid:[/]", $"[bold green]${order.TotalAmount:F2}[/]");

			AnsiConsole.Write(
				new Panel(grid)
					.Header("[bold green] Order Checkout Successful [/]")
					.BorderColor(Color.Green)
					.Padding(1, 1)
			);
		}
	}
}
