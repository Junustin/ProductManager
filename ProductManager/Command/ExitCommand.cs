using ProductManager.Interface;
using ProductManager.Services;


namespace ProductManager.Command
{
	public class ExitCommand : ICommand
	{
		public string Name => "Exit";

		public string Description => "Exit application";

		private readonly Action _stopApplication;

		public ExitCommand(Action stopApplication)
		{
			_stopApplication = stopApplication;
		}

		public void Execute(string[] args)
		{
			// Stop application exiting main loop
			ConsoleLogger.LogWarning("Application will exit.");
			_stopApplication();
		}
	}
}
