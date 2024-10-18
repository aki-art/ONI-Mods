﻿using Klei.AI;

namespace Moonlet.Console.Commands
{
	public class AddEffectCommand() : BaseConsoleCommand("addeffect")
	{
		public override CommandResult Run()
		{
			var args = argumentStrs;
			var go = SelectTool.Instance.selected;

			if (go == null)
				return CommandResult.Warning("Nothing is selected");

			if (!go.TryGetComponent(out Effects effects))
				return CommandResult.Warning("Selected object cannot have effects.");

			if (Db.Get().effects.TryGet(args[1]) == null)
				return CommandResult.Warning($"No effect with ID {args[1]}");

			effects.Add(args[1], false);


			return CommandResult.success;
		}
	}
}
