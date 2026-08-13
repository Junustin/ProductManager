using ProductManager.Command;
using ProductManager.Features.ProductComponents.Data;
using ProductManager.Features.Storage;
using ProductManager.Interface;
using ProductManager.Sqlite;
using System.Security.Cryptography.X509Certificates;

namespace ProductManager
{
    internal class Program
    {
		static void Main(string[] args)
        {
			Console.WriteLine("Welcome to product manager");
			bool isRunning = true;

			// Initilize all components
			CommandDispatcher dispatcher = new CommandDispatcher();
			IProductRepository repo = new SqliteProductRepository();
			repo.InitilizeDatabase();

			// Register/Init all commands
			dispatcher.RegisterCommand(new AddCommand(repo));
			dispatcher.RegisterCommand(new ListCommand(repo));
			dispatcher.RegisterCommand(new RemoveCommand(repo));
			dispatcher.RegisterCommand(new ClearCommand(repo));
			dispatcher.RegisterCommand(new HelpCommand());
			dispatcher.RegisterCommand(new ExitCommand(() => isRunning = false));

			// Program loop
            while (isRunning)
            {
				Console.WriteLine("Please enter command");
                Console.Write("> ");
				string? rawInput = Console.ReadLine();

				// handole blank or empty input
                if(string.IsNullOrWhiteSpace(rawInput))
                {
					Console.WriteLine("Please enter valid command\nType help for all command");
					continue;
                }

				// Split raw input
				string[] tokens = rawInput.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
				// Extract command
				string command = tokens[0];
				// Extract arguments
				string[] arguments = tokens[1..];

				// Dispatch command
				dispatcher.Dispatch(command, arguments);
			}
		}
    }
}
