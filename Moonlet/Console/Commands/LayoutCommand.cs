using ProcGen;
using ProcGenGame;
using System.Collections.Generic;

namespace Moonlet.Console.Commands
{
	internal class LayoutCommand() : CommandBase("layout")
	{
		public override void SetupArguments()
		{
			base.SetupArguments();
			arguments =
			[
				new ArgumentInfo[]
				{
					new StringArgument("worldId", "WorldId", optional: false),
					new IntArgument("seed", "Seed", optional: true),
				}
			];
		}

		public override CommandResult Run()
		{
			if (GetStringArgument(1, out var worldId))
			{
				var seed = GetIntArgument(2, out var seedNum) ? seedNum : 0;

				var world = SettingsCache.worlds.GetWorldData(worldId);
				if (world == null)
					return CommandResult.Warning("No world with this id.");

				Log.Debug("Creating new worldgen");
				var worldGen = new WorldGen(worldId, [], [], false);

				Log.Debug("Creating new WorldLayout");
				var layout = new WorldLayout(worldGen, 0);
				Log.Debug("Generating Overworld");
				var tree = layout.GenerateOverworld(worldGen.Settings.world.layoutMethod == ProcGen.World.LayoutMethod.PowerTree, true);

				Log.Debug($"Generated Overworld with {tree.ChildCount()} childcount, and {tree.Count()} count.");
				Log.Debug("Populating subworlds...");
				layout.PopulateSubworlds();
				Log.Debug("Completing layout...");
				worldGen.CompleteLayout(UpdateFn);
				Log.Debug("Generation finished.");

				return CommandResult.success;
			}

			return CommandResult.argCountError;
		}

		private bool UpdateFn(StringKey stringKeyRoot, float completePercent, WorldGenProgressStages.Stages stage)
		{
			DevConsole.Log($"{stage} {completePercent}%");
			return true;
		}
	}
}
