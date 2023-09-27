namespace Moonlet.Console.Commands
{
	public class HelpCommand() : CommandBase("help")
	{
		public override CommandResult Run(string[] args)
		{
			foreach(var command in DevConsole.Commands)
			{
				DevConsole.Log($"<b>{command.Key}</b>: {command.Value.Description()}");
			}

			return CommandResult.Success();
		}
	}
}
