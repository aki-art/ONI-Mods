using HarmonyLib;
using ProcGen;
using System.Collections.Generic;

namespace Moonlet.Patches
{
	public class WorldsPatches
	{
		[HarmonyPatch(typeof(Worlds), nameof(Worlds.LoadReferencedWorlds))]
		public class ProcGen_Worlds_LoadReferencedWorlds_Patch
		{
			public static void Prefix(ISet<string> referencedWorlds)
			{
				Mod.worldMixingLoader.ApplyToActiveLoaders(mixing =>
				{
					mixing.LoadContent(ref referencedWorlds);
				});

				Mod.worldsLoader.ApplyToActiveLoaders(world =>
				{
					if (referencedWorlds.Contains(world.id))
						world.LoadContent();
				});
			}
		}
	}
}
