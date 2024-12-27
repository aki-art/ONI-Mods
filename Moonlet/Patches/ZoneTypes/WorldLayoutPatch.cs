
#if TRANSPILERS
using HarmonyLib;
using ProcGen;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace Moonlet.Patches.ZoneTypes
{
	public class WorldLayoutPatch
	{
		// inserting the new zonetypes to a list which normally only iterates Enum.Values
		[HarmonyPatch(typeof(WorldLayout), nameof(WorldLayout.ConvertUnknownCells))]
		public class WorldLayout_ConvertUnknownCells_Patch
		{
			public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
			{
				var codes = orig.ToList();

				var m_Print = AccessTools.Method(typeof(WorldLayout_ConvertUnknownCells_Patch), "Print",
				[
					typeof(List<WeightedSubWorld>),
					typeof(Dictionary<string, List<WeightedSubWorld>>)
				]);

				var index = codes.Count - 2;

				if (index < 0)
					return codes;

				codes.InsertRange(index + 1, new[]
				{
					new CodeInstruction(OpCodes.Ldloc_2), // TODO
					new CodeInstruction(OpCodes.Ldloc_S, 4),
					new CodeInstruction(OpCodes.Call, m_Print)
				});

				return codes;
			}

			public static void Print(List<WeightedSubWorld> subworldsForWorld, Dictionary<string, List<WeightedSubWorld>> subWorlds)
			{
				foreach (var zone in Mod.zoneTypesLoader.GetLoaders())
					subWorlds.Add(zone.template.Id, subworldsForWorld.FindAll(sw => sw.subWorld.zoneType == zone.type));
			}
		}
	}
}

#endif