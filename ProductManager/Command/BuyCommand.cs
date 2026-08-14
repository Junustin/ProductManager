using ProductManager.Component.OrderComponents;
using ProductManager.Interface;
using ProductManager.Services;
using Spectre.Console;

namespace ProductManager.Command
{
	public class BuyCommand : ICommand
	{
		public string Name => "Buy";

		public string Description => "Start transaction";

		private readonly IProductRepository _productRepository;
		private readonly IOrderRepository _orderRepository;

		public BuyCommand(IProductRepository productRepository, IOrderRepository orderRepository)
		{
			_productRepository = productRepository;
			_orderRepository = orderRepository;
		}

		public void Execute(string[] args)
		{
			// Ensure correct arguments
			if (args.Length != 2)
			{
				ConsoleLogger.LogWarning("Invalid command. Usage: buy <ProductId> <Quantity>");
				return;
			}

			// Parse Product ID
			if (!int.TryParse(args[0], out var id) || id <= 0)
			{
				ConsoleLogger.LogError("ID must be a positive integer.");
				return;
			}

			// Parse Quantity
			if (!int.TryParse(args[1], out var quantity) || quantity <= 0)
			{
				ConsoleLogger.LogError("Quantity must be a positive number");
				return;
			}

			// Fetch product data and check stock
			var product = _productRepository.GetById(id);
			if (product == null)
			{
				ConsoleLogger.LogError($"No product with ID: {id} found.");
				return;
			}

			if (product.Stock < quantity)
			{
				ConsoleLogger.LogError($"Not enough stock to satisfy this order. Avaliable: {product.Stock} Requested: {quantity}");
				return;
			}

			var orderItem = new OrderItem
			{
				ProductId = product.Id,
				Quantity = quantity,
				UnitPrice = product.Price,
			};
			var order = new Order
			{
				CreatedAt = DateTime.UtcNow,
				TotalAmount = product.Price * quantity,
				Items = new List<OrderItem> { orderItem }

			};

			bool isSuccess = _orderRepository.CreateOrder(order);

			if (!isSuccess)
			{
				// Log is handle by OrderRepository
				return;
			}

			var grid = new Grid()
				.AddColumn()
				.AddColumn();

			grid.AddRow("[bold gray]Order ID:[/]", $"[yellow]#{order.Id}[/]");
			grid.AddRow("[bold gray]Product:[/]", $"[green]{product.Name}[/]");
			grid.AddRow("[bold gray]Unit Price:[/]", $"[green]${product.Price:F2}[/]");
			grid.AddRow("[bold gray]Quantity Paid:[/]", $"[cyan]{quantity}[/]");
			grid.AddRow("[bold gray]Total Amount:[/]", $"[bold green]${order.TotalAmount:F2}[/]");

			AnsiConsole.Write(new Panel(grid)
				.Header("[bold green] Purchase Successful![/]")
				.BorderColor(Color.Green)
				.Padding(1, 1)
				);
				
		}
	}
}
