using ProductManager.Interface;

namespace ProductManager.Command
{
	public class HelpCommand : ICommand
	{
		public string Name => "Help";

		public string Description => "Display all command";

		public void Execute(string[] args)
		{
			Console.WriteLine("Display all commands");
		}
	}
}
