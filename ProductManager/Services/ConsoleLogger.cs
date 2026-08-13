using Spectre.Console;

namespace ProductManager.Services
{
	public static class ConsoleLogger
	{
		public static void LogInfo(string message)
		{
			AnsiConsole.MarkupLine($"[bold blue] Info:[/] {message}");
		}

		public static void LogInfo(string message, bool newLine)
		{
			if(newLine)
			{
				AnsiConsole.MarkupLine($"[bold blue] Info: [/] {message}");
			}
			else
				AnsiConsole.Markup($"[bold blue] Info: [/]{message}");
		}

		public static void LogSuccess(string message) 
		{
			AnsiConsole.MarkupLine($"[bold green] Success: [/]{message}");
		}
		
		public static void LogWarning(string message)
		{
			AnsiConsole.MarkupLine($"[bold yellow] Warning: [/]{message}");
		}
		public static void LogWarning(string message, bool newLine)
		{
			if(newLine)
			{
				AnsiConsole.MarkupLine($"[bold yellow] Warning: [/]{message}");
			}
			else
				AnsiConsole.Markup($"[bold yellow] Warning: [/]{message}");
		}
		public static void LogError(string message)
		{
			AnsiConsole.MarkupLine($"[bold red] Error: [/]{message}");
		}
	}
}
