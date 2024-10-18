namespace Moonlet.Console.Commands
{
	public class SetProjectCommand() : BaseConsoleCommand("project")
	{
		public override void SetupArguments()
		{
			base.SetupArguments();
			arguments =
			[
				new ArgumentInfo[]
				{
					new StringArgument("modId", "StaticID of a Moonlet mod", optional: false)
				}
			];
		}

		public override CommandResult Run()
		{
			if (GetStringArgument(1, out var projectId))
			{
				if (!MoonletMods.Instance.moonletMods.ContainsKey(projectId))
					return CommandResult.Warning("Not a mod.");

				DevConsole.currentProject = projectId;

				return CommandResult.success;
			}

			return CommandResult.Warning("Could not parse projectId.");
		}
	}
}
