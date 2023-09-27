namespace Moonlet.Console.Commands
{
	public class LogIdCommand() : CommandBase("id")
	{
		public override CommandResult Run(string[] args)
		{
			if(SelectTool.Instance.selected != null)
			{
				DevConsole.Log($"ID: {SelectTool.Instance.selected.PrefabID()}");
				return CommandResult.success;
			}

			return CommandResult.Warning("Nothing is selected.");
		}
	}
}
