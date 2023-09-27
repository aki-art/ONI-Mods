using Moonlet.Scripts.UI;
using System;
using System.Collections.Generic;

namespace Moonlet.Console
{
	public class DevConsole
	{
		public static Dictionary<string, CommandBase> Commands { get; private set; } = new();
		public static List<string> commandHistory = new();

		public static void RegisterCommand(string ID, CommandBase command)
		{
			Commands[ID] = command;
		}

		public static void RegisterCommand(CommandBase command)
		{
			if (command is null)
			{
				FUtility.Log.Error($"Null command.");
				return;
			}

			RegisterCommand(command.id, command);
		}

		public static CommandResult ParseCommand(string commandStr)
		{
			if (commandStr.IsNullOrWhiteSpace())
				return CommandResult.Error("Empty command.");

			var split = commandStr.Split(' ');

			if (Commands.TryGetValue(split[0], out CommandBase command))
			{
				var result = command.Run(split);

				if (result.severity == CommandResult.Severity.Success)
					commandHistory.Add(commandStr);

				return result;
			}

			return CommandResult.Warning($"{split[0]} is not a recognized command.");
		}

		public static void Log(string msg)
		{
			DevConsoleScreen.Instance.AddLogEntry(msg);
		}
	}
}
