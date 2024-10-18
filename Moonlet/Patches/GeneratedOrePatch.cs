using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

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
					Log.Warn("Could not patch GeneratedOre.LoadGeneratedOre");
					return codes;
				}

				var m_InjectedMethod = AccessTools.DeclaredMethod(typeof(GeneratedOre_LoadGeneratedOre_Patch), nameof(LoadOres));

				// inject right after the found index
				codes.InsertRange(index + 1,
				[
					new CodeInstruction(OpCodes.Dup),
					new CodeInstruction(OpCodes.Call, m_InjectedMethod)
				]);

				return codes;
			}

			private static void LoadOres(HashSet<SimHashes> simHashes)
			{
				Mod.debrisLoader.ApplyToActiveLoaders(template => template.LoadContent(simHashes));
			}
		}
	}
}
