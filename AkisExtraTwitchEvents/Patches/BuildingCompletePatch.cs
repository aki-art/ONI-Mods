using HarmonyLib;
using Twitchery.Utils;

namespace Twitchery.Patches
{
	public class BuildingCompletePatch
	{
		private static readonly Tag[] protectedTags =
		[
			GameTags.Gravitas
		];

		[HarmonyPatch(typeof(BuildingComplete), "OnSpawn")]
		public class Building_OnSpawn_Patch
		{
			public static void Postfix(BuildingComplete __instance)
			{
				if (__instance.HasTag(GameTags.Telepad))
				{
					var circleAround = ProcGen.Util.GetFilledCircle(__instance.transform.position, 5f);
					foreach (var offset in circleAround)
					{
						var cell = Grid.PosToCell(offset);
						AGridUtil.protectedCells.Add(cell);
					}
				}

				var isProtected = __instance.Def.ShowInBuildMenu == false || __instance.HasAnyTags(protectedTags);

				if (isProtected)
					foreach (var cell in __instance.placementCells)
						AGridUtil.protectedCells.Add(cell);
			}
		}
	}
}
