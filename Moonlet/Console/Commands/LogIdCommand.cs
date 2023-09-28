namespace Moonlet.Console.Commands
{
	public class LogIdCommand() : CommandBase("id")
	{
		public override CommandResult Run(string[] args)
		{
			if (SelectTool.Instance.selected != null && SelectTool.Instance.selected.TryGetComponent(out KPrefabID kPrefabId))
			{
				DevConsole.Log($"ID: {kPrefabId.PrefabTag}");
				return CommandResult.success;
			}

			if (Grid.IsValidCell(SelectTool.Instance.selectedCell))
			{
				var element = Grid.Element[SelectTool.Instance.selectedCell];
				if (element != null)
				{
					DevConsole.Log($"ID: {element.id}");
					return CommandResult.success;
				}
			}

			return CommandResult.Warning("Nothing is selected.");
		}
	}
}
