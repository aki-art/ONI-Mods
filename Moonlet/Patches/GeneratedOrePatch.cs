using FUtility;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Moonlet.Patches
{
	public class GeneratedOrePatch
	{
		[HarmonyPatch(typeof(GeneratedOre), "LoadGeneratedOre")]
		public class GeneratedOre_LoadGeneratedOre_Patch
		{
			public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
			{
				var codes = orig.ToList();

				// find injection point
				var index = codes.FindIndex(ci => ci.opcode == OpCodes.Newobj);

				if (index == -1)
				{
					Log.Warning("Could not patch GeneratedOre.LoadGeneratedOre");
					return codes;
				}

				var m_InjectedMethod = AccessTools.DeclaredMethod(typeof(GeneratedOre_LoadGeneratedOre_Patch), "InjectedMethod");

				// inject right after the found index
				codes.InsertRange(index + 1, new[]
				{
					new CodeInstruction(OpCodes.Dup),
					new CodeInstruction(OpCodes.Call, m_InjectedMethod)
				});

				return codes;
			}

			private static void InjectedMethod(HashSet<SimHashes> simHashes)
			{
				foreach (var mod in Mod.modLoaders)
					mod.entitiesLoader.LoadDebris(simHashes);
			}
		}
	}
}
