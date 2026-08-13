using ProductManager.Interface;
using ProductManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.Command
{
	public class CommandDispatcher
	{
		private readonly Dictionary<string, ICommand> _commands = new(StringComparer.OrdinalIgnoreCase);

		public void RegisterCommand(ICommand command)
		{
			_commands[command.Name] = command;	
		}

		public void Dispatch(string commandName, string[] args)
		{
			if(_commands.TryGetValue(commandName, out var command))
			{
				command.Execute(args);
			}
			else
			{
				ConsoleLogger.LogError($"'{commandName}' command does not exist, Type 'help' for options.");
			}
		}
	}
}
