using ProductManager.Interface;
using ProductManager.Services;


namespace ProductManager.Command
{
	public class ClearAllCommand : ICommand
	{
		public string Name => "ClearAll";

		public string Description => "Clear all tables in Supermarket.db";

		private readonly IProductRepository _productRepo;
		private readonly IOrderRepository _orderRepo;

		public ClearAllCommand(IProductRepository productRepo, IOrderRepository orderRepo)
		{
			_productRepo = productRepo;
			_orderRepo = orderRepo;
		}

		public void Execute(string[] args)
		{
			ConsoleLogger.LogWarning("This will DELETE ALL(Ignore foreign key) tables in Supermarket.db Are you sure Y/N: ", false);
			string? confirmation = Console.ReadLine();

			if (confirmation?.Trim().Equals("y", StringComparison.OrdinalIgnoreCase) == true)
			{
				_productRepo.ClearAll();
				_orderRepo.ClearAll();
				ConsoleLogger.LogSuccess("Database cleared successfully.");
			}
			else
			{
				ConsoleLogger.LogError("Clear operation cancelled.");
			}
		}
	}
}
