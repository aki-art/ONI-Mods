using FUtility;
using HarmonyLib;
using Moonlet.Content.Scripts;
using ProcGenGame;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace Moonlet.Patches
{
	public class SaveLoaderPatch
	{
		[HarmonyPatch(typeof(SaveLoader), nameof(SaveLoader.LoadFromWorldGen))]
		public class SaveLoader_LoadFromWorldGen_Patch
		{
			public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> orig)
			{
				var codes = orig.ToList();

				var m_PassWorldData = AccessTools.Method(typeof(SaveLoader_LoadFromWorldGen_Patch), "PassWorldData", new[] { typeof(Cluster) });
				var f_m_clusterLayout = AccessTools.Field(typeof(SaveLoader), "m_clusterLayout");

				var index = codes.FindIndex(c => c.StoresField(f_m_clusterLayout));

				if (index == -1)
				{
					Log.Warning("Could not patch SaveLoader.LoadFromWorldGen");
					return codes;
				}

				codes.InsertRange(index + 1, new[]
				{
					new CodeInstruction(OpCodes.Ldarg_0),
					new CodeInstruction(OpCodes.Ldfld, f_m_clusterLayout),
					new CodeInstruction(OpCodes.Call, m_PassWorldData)
				});

				return codes;
			}

			private static void PassWorldData(Cluster cluster)
			{
				if (Moonlet_Mod.Instance == null)
				{
					Log.Warning("Could not update world loading changes.");
					return;
				}

				Moonlet_Mod.Instance.WorldLoaded(cluster);
			}
		}

	}
}
