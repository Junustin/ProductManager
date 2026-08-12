using ProductManager.Command;
using ProductManager.Features.ProductComponents.Data;
using ProductManager.Features.Storage;

namespace ProductManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
			Console.WriteLine("Welcome to product manager");

			// Initilize all components
			ProductStorage storage = new ProductStorage();
			CommandDispatcher dispatcher = new CommandDispatcher();

			// Register all commands
			dispatcher.RegisterCommand(new AddCommand(storage));
			dispatcher.RegisterCommand(new ListCommand(storage));
			dispatcher.RegisterCommand(new HelpCommand());
			dispatcher.RegisterCommand(new ExitCommand());

			// Program loop
            while (true)
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
				string[] tokens = rawInput.Split(' ', StringSplitOptions.RemoveEmptyEntries);
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
