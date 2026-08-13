using ProductManager.Interface;
using ProductManager.Services;

namespace ProductManager.Command
{
	public class RemoveCommand : ICommand
	{
		public string Name => "Remove";

		public string Description => "Remove product entry from database";

		private readonly IProductRepository _repo;

		public RemoveCommand(IProductRepository repo)
		{
			_repo = repo;
		}

		public void Execute(string[] args)
		{
			if(args.Length != 1)
			{
				ConsoleLogger.LogError("Please enter correct arguments for Remove command Example: Remove <ProductID>");
				return;
			}

			if (int.TryParse(args[0], out int idToRemove))
			{
				if (_repo.Remove(idToRemove))
				{
					ConsoleLogger.LogSuccess($"Remove product {idToRemove} successful.");
					return;
				}
				else
				{
					ConsoleLogger.LogError($"There is no product in Id:{idToRemove} to remove.");
					return;
				}
			}
			else
			{
				ConsoleLogger.LogError("Please enter correct format for product Id");
			}
		}
	}
}
