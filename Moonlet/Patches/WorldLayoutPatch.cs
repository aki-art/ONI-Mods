using HarmonyLib;
using Moonlet.ZoneTypes;
using ProcGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace Moonlet.Patches
{
	public class WorldLayoutPatch
	{
		// inserting the new zonetypes to a list which normally iterates Enum.Values
		[HarmonyPatch(typeof(WorldLayout), "ConvertUnknownCells")]
		public class WorldLayout_ConvertUnknownCells_Patch
		{
			public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
			{
				var codes = orig.ToList();

				var m_Print = AccessTools.Method(typeof(WorldLayout_ConvertUnknownCells_Patch), "Print", new Type[]
					{
						typeof(List<WeightedSubWorld>),
						typeof(Dictionary<string, List<WeightedSubWorld>>)
					});

				var index = codes.Count - 2;

				if (index < 0)
					return codes;

				codes.InsertRange(index + 1, new[]
				{
					new CodeInstruction(OpCodes.Ldloc_2),
					new CodeInstruction(OpCodes.Ldloc_S, 4),
					new CodeInstruction(OpCodes.Call, m_Print)
				});

				return codes;
			}

			public static void Print(List<WeightedSubWorld> subworldsForWorld, Dictionary<string, List<WeightedSubWorld>> dictionary)
			{
				foreach (var zone in ZoneTypeUtil.zones)
					dictionary.Add(zone.id, subworldsForWorld.FindAll(sw => sw.subWorld.zoneType == zone.type));
			}
		}
	}
}
