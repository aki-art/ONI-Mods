using HarmonyLib;
using Twitchery.Content;

namespace Twitchery.Patches
{
	public class GameNavGridsPatch
	{
		[HarmonyPatch(typeof(GameNavGrids), MethodType.Constructor, typeof(Pathfinding))]
		public class GameNavGrids_Ctor_Patch
		{
			public static void Postfix(GameNavGrids __instance, Pathfinding pathfinding)
			{
				TNavGrids.CreateNavGrids(__instance, pathfinding);
			}
		}
	}
}
