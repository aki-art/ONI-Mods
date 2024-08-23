using Moonlet.TemplateLoaders.WorldgenLoaders;
using ProcGen;
using ProcGenGame;
using System.Collections.Generic;

namespace Moonlet.Patches
{
	public class MobSpawningPatch
	{
		//[HarmonyPatch(typeof(MobSpawning), "IsSuitableMobSpawnPoint")]
		public class MobSpawning_IsSuitableMobSpawnPoint_Patch
		{
			public static bool Prefix(int cell, Mob mob, Sim.Cell[] cells, ref HashSet<int> alreadyOccupiedCells, ref bool __result)
			{
				if (mob.location == MobLoader.AnyWater)
				{
					__result = IsInWater(cell, mob, cells, ref alreadyOccupiedCells);
					return false;
				}
				else if (mob.location == MobLoader.Liquid)
				{
					__result = IsInLiquid(cell, mob, cells, ref alreadyOccupiedCells);
					return false;
				}

				return true;
			}

			private static bool IsAlreadyOccupied(int cell, Mob mob, ref HashSet<int> alreadyOccupiedCells)
			{
				for (int widthIterator = 0; widthIterator < mob.width; ++widthIterator)
				{
					for (int index = 0; index < mob.height; ++index)
					{
						int cell1 = MobSpawning.MobWidthOffset(cell, widthIterator);
						if (!Grid.IsValidCell(cell1) || !Grid.IsValidCell(Grid.CellAbove(cell1)) || !Grid.IsValidCell(Grid.CellBelow(cell1)) || alreadyOccupiedCells.Contains(cell1))
						{
							return true;
						}
					}
				}

				return false;
			}

			private static bool IsInLiquid(int cell, Mob mob, Sim.Cell[] cells, ref HashSet<int> alreadyOccupiedCells)
			{
				if (IsAlreadyOccupied(cell, mob, ref alreadyOccupiedCells))
					return false;

				var element = ElementLoader.elements[cells[cell].elementIdx];
				var above = ElementLoader.elements[cells[Grid.CellAbove(cell)].elementIdx];

				return element.IsLiquid && above.IsLiquid;
			}

			private static bool IsInWater(int cell, Mob mob, Sim.Cell[] cells, ref HashSet<int> alreadyOccupiedCells)
			{
				if (IsAlreadyOccupied(cell, mob, ref alreadyOccupiedCells))
					return false;

				var element = ElementLoader.elements[cells[cell].elementIdx];
				var above = ElementLoader.elements[cells[Grid.CellAbove(cell)].elementIdx];

				return element.HasTag(GameTags.AnyWater) && above.HasTag(GameTags.AnyWater);
			}
		}
	}
}
