using ProductManager.Interface;
using ProductManager.Services;

namespace ProductManager.Command
{
	public class HelpCommand : ICommand
	{
		public string Name => "Help";

		public string Description => "Display all command";

		public void Execute(string[] args)
		{
			ConsoleLogger.LogInfo("Display all commands");
		}
	}
}
