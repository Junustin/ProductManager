using ProductManager.Interface;
using ProductManager.Services;
using ProductManager.Sqlite;


namespace ProductManager.Command
{
	public class ClearCommand : ICommand
	{
		public string Name => "Clear";

		public string Description => "For developer only! Clear the database";

		private readonly IProductRepository _repo;

		public ClearCommand(IProductRepository repo)
		{
			_repo = repo;
		}

		public void Execute(string[] args)
		{
			ConsoleLogger.LogWarning("This will DELETE ALL products and reset IDS. Are you sure Y/N: ", false);
			string? confirmation = Console.ReadLine();

			if (confirmation?.Trim().Equals("y", StringComparison.OrdinalIgnoreCase) == true)
			{
				_repo.ClearAll();
				ConsoleLogger.LogSuccess("Database cleared successfully. Next added product will start at ID 1.");
			}
			else
			{
				ConsoleLogger.LogError("Clear operation cancelled.");
			}
		}
	}
}
