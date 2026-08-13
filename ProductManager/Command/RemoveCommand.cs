using ProductManager.Interface;
using ProductManager.Sqlite;

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
				Console.WriteLine("Please enter correct arguments for Remove command Example: Remove <ProductID>");
				return;
			}

			if (int.TryParse(args[0], out int idToRemove))
			{
				if (_repo.Remove(idToRemove))
				{
					Console.WriteLine($"Remove product {idToRemove} successful.");
					return;
				}
				else
				{
					Console.WriteLine($"There is no product in Id:{idToRemove} to remove.");
					return;
				}
			}
			else
			{
				Console.WriteLine("Please enter correct format for product Id");
			}
		}
	}
}
