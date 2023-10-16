using Klei.AI;

namespace Moonlet.Console.Commands
{
	public class RemoveEffectCommand() : CommandBase("removeeffect")
	{
		public override CommandResult Run()
		{
			var args = argumentStrs;

			var go = SelectTool.Instance.selected;

			if (go == null)
				return CommandResult.emptySelection;

			if (!go.TryGetComponent(out Effects effects))
				return CommandResult.Warning("Selected object has no Effects.");

			effects.Remove(args[1]);

			return CommandResult.success;
		}
	}
}
