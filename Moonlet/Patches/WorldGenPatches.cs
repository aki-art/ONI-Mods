using HarmonyLib;
using ProcGenGame;

namespace Moonlet.Patches
{
	internal class WorldGenPatches
	{

		[HarmonyPatch(typeof(WorldGen), "SpawnMobsAndTemplates")]
		public class WorldGen_SpawnMobsAndTemplates_Patch
		{
			public static void Prefix(WorldGen __instance)
			{
				Log.Debug("SpawnMobsAndTemplates " + __instance.Settings.mutatedWorldData.world.name);
				if (__instance.Settings.mutatedWorldData.mobs?.MobLookupTable == null)
				{
					Log.Debug("No mobs defined");
				}
				else
				{
					foreach (var mob in __instance.Settings.mutatedWorldData.mobs?.MobLookupTable)
					{
						Log.Debug($"{mob.Key}: {mob.Value}");
					}
				}
			}
		}
	}
}
