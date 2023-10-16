namespace Moonlet.Console.Commands
{
	public class HelpCommand() : CommandBase("help")
	{
		public const int SPACING = 50;

		public override CommandResult Run()
		{
			var msg = "";

			foreach (var command in DevConsole.Commands)
			{
				msg += $"<b>{command.Key}</b> <color=#cccccc>{command.Value.Description()}</color>\n";
			}

			DevConsole.Log(msg);

			return CommandResult.success;
		}
	}
}
