namespace Moonlet.Console.Commands
{
	public class ReloadCommand() : CommandBase("reload")
	{
		public override CommandResult Run(string[] args)
		{
			if (args.Length == 1)
			{
				// reload all
			}
			else
			{
				// find mod, reload it
			}

			return CommandResult.Error("Command not yet implemented.");
		}
	}
}
