using ProcGen;
using ProcGenGame;

namespace Moonlet.Console.Commands
{
	internal class LayoutCommand() : BaseConsoleCommand("layout")
	{
		public override void SetupArguments()
		{
			base.SetupArguments();
			expectedArgumentVariations =
			[
				[
					new StringArgument("worldId", "WorldId", optional: false),
					new IntArgument("seed", "Seed", optional: true),
				]
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

				var worldGen = new WorldGen(worldId, [], [], false);

				var layout = new WorldLayout(worldGen, 0);
				var tree = layout.GenerateOverworld(worldGen.Settings.world.layoutMethod == ProcGen.World.LayoutMethod.PowerTree, true);

				layout.PopulateSubworlds();
				worldGen.CompleteLayout(UpdateFn);

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
