using ProductManager.Interface;


namespace ProductManager.Command
{
	public class ExitCommand : ICommand
	{
		public string Name => "Exit";

		public string Description => "Exit application";

		public void Execute(string[] args)
		{
			// Stop application exiting main loop
			Console.WriteLine("Application exit");
		}
	}
}
