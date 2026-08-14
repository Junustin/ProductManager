using ProductManager.Command;
using ProductManager.Interface;
using ProductManager.Services;
using ProductManager.Repository;

namespace ProductManager
{
    internal class Program
    {
		static void Main(string[] args)
        {
			// Start
			ConsoleLogger.LogInfo("Welcome to product manager!");
			bool isRunning = true;

			// Bootstraps initialize database
			DatabaseInitializer.Initialize();

			// Initilize all components
			CommandDispatcher dispatcher = new CommandDispatcher();
			IProductRepository productRepo = new SqliteProductRepository();
			IOrderRepository orderRepo = new SqliteOrderRepository();

			// Register/Init all commands
			dispatcher.RegisterCommand(new AddCommand(productRepo));
			dispatcher.RegisterCommand(new ListCommand(productRepo));
			dispatcher.RegisterCommand(new DeleteCommand(productRepo));
			dispatcher.RegisterCommand(new UpdateCommand(productRepo));
			dispatcher.RegisterCommand(new BuyCommand(productRepo, orderRepo));
			dispatcher.RegisterCommand(new ClearAllCommand(productRepo, orderRepo));
			dispatcher.RegisterCommand(new HelpCommand());
			dispatcher.RegisterCommand(new ExitCommand(() => isRunning = false));

			// Program loop
            while (isRunning)
            {
				ConsoleLogger.LogInfo("Please enter command");
                ConsoleLogger.LogInfo("> ", false);
				string? rawInput = Console.ReadLine();

				// handle blank or empty input
                if(string.IsNullOrWhiteSpace(rawInput))
                {
					ConsoleLogger.LogError("Please enter valid command\nType help for all command");
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
