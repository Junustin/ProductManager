using ProductManager.Interface;
using ProductManager.Services;
using Spectre.Console;


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
				ConsoleLogger.LogInfo("No product in storage");
				return;
			}

			// Create table
			var table = new Table();
			table.Border(TableBorder.Rounded);
			table.Title("[bold white]Storage Inventroy[/]");

			// Add collumns
			table.AddColumn("[bold green]ID[/]");
			table.AddColumn("[bold blue]Product Name[/]");
			table.AddColumn("[bold gold1]Price[/]");
			table.AddColumn("[bold cyan]Stock[/]");

			// Right-align numeric columns for proper alignment
			table.Columns[0].RightAligned();
			table.Columns[2].RightAligned();
			table.Columns[3].RightAligned();

			// Build table row
			foreach (var product in products)
			{
				table.AddRow(
					product.Id.ToString(),
					Markup.Escape(product.Name),
					$"${product.Price:F2}",
					product.Stock.ToString()
				);
			}

			// Log the table
			AnsiConsole.Write(table);
		}
	}
}
