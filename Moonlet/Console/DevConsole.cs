using Moonlet.Scripts.UI;
using System.Collections.Generic;

namespace Moonlet.Console
{
	public class DevConsole
	{
		public static Dictionary<string, BaseConsoleCommand> Commands { get; private set; } = [];
		public static List<string> commandHistory = [];
		public static CellElementEvent cellElementEvent = new("moonlet_consolespawn", "console spawned", false);
		public static string currentProject; // TODO: serialize
		public static BaseConsoleCommand currentCommand;

		public static void RegisterCommand(string ID, BaseConsoleCommand command)
		{
			Commands[ID] = command;
		}

		public static void RegisterCommand(BaseConsoleCommand command)
		{
			if (command is null)
			{
				FUtility.Log.Error($"Null command.");
				return;
			}

			RegisterCommand(command.id.ToLowerInvariant(), command);
		}

		public static CommandResult ParseCommand(string commandStr)
		{
			FUtility.Log.Debug("str: " + commandStr);

			if (commandStr.IsNullOrWhiteSpace())
				return CommandResult.Error("Empty command.");

			var split = commandStr.Split(' ');

			var keyword = split[0].ToLowerInvariant();

			if (Commands.TryGetValue(keyword, out BaseConsoleCommand command))
			{
				command.argumentStrs = split;
				var result = command.Run(split);

				if (result.severity == CommandResult.Severity.Success)
					commandHistory.Add(commandStr);

				return result;
			}

			return CommandResult.Warning($"{split[0]} is not a recognized command.");
		}

		public static void Log(object msg, string color = null)
		{
			if (color != null)
				msg = $"<color={color}>{msg}</color>";

			DevConsoleScreen.Instance.AddLogEntry(msg.ToString());
		}

		public static bool TryGetAutoComplete(string partialEntry, out string value)
		{
			if (!partialEntry.IsNullOrWhiteSpace())
				foreach (var command in Commands.Keys)
				{
					if (command.StartsWith(partialEntry))
					{
						value = command;
						return true;
					}
				}

			value = null;
			return false;
		}
	}
}
