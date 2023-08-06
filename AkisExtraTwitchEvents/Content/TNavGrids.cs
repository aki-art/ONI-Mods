using Twitchery.Content.Defs.Critters;

namespace Twitchery.Content
{
    public class TNavGrids
	{
		public const string GIANT_CRAB_NAV = "AETE_GiantCrabNav";

		public static void CreateNavGrids(GameNavGrids navGrids, Pathfinding pathfinding)
		{
			CreateCrabNavigation(navGrids, pathfinding, GIANT_CRAB_NAV, FUtility.Utils.MakeCellOffsets(1, GiantCrabConfig.HEIGHT));
		}

/*		private static NavGrid CreateRegularPipNavigation(GameNavGrids navGrids, Pathfinding pathfinding, string id, CellOffset[] bounds)
		{
			var squirrel = navGrids.SquirrelGrid.n
		}
*/
		private static NavGrid CreateCrabNavigation(GameNavGrids navGrids, Pathfinding pathfinding, string id, CellOffset[] bounds)
		{
			var transitions = navGrids.MirrorTransitions(new[]
			{
				// 1 step side
				FloorTransition(1, 0, new CellOffset[] {}, true),
				// 1 step up and side
				FloorTransition(1, 1, new CellOffset[] { new(0, 1)}),
				// jump 1 gap
				FloorTransition(2, 0, new CellOffset[] { new(1, 0), new(1, -1)}),
				// jump down a 2 tall ledge
				FloorTransition(1, -2, new CellOffset[] { new(1, 0), new(1, -1)}),
				// 1 step down and side
				FloorTransition(1, -1, new CellOffset[] { new(1, 0)}),
				// jump up a 2 tall ledge
				FloorTransition(1, 2, new CellOffset[] { new(0, 1), new(0, 2)})
			});

			var data = new[]
			{
				new NavGrid.NavTypeData()
				{
					navType = NavType.Floor,
					idleAnim = "idle_loop"
				}
			};

			var validators = new[]
			{
				new GameNavGrids.FloorValidator(false)
			};

			var navGrid = new NavGrid(
				id,
				transitions,
				data,
				bounds,
				validators,
				5,
				7,
				transitions.Length);

			pathfinding.AddNavGrid(navGrid);

			return navGrid;
		}

		private static NavGrid.Transition FloorTransition(int x, int y, CellOffset[] voidOffsets, bool isLooping = false)
		{
			return new NavGrid.Transition(
				NavType.Floor,
				NavType.Floor,
				x,
				y,
				NavAxis.NA,
				isLooping,
				isLooping,
				true,
				1,
				"",
				voidOffsets,
				new CellOffset[0],
				new NavOffset[0],
				new NavOffset[0],
				true,
				0.5f);
		}

	}
}
