/*using HarmonyLib;
using ProcGen;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace Moonlet.Patches
{
	internal class CodexEntryGeneratorPatch
	{

		*//*		[HarmonyPatch(typeof(CodexEntryGenerator), "GenerateBiomeEntries")]
				public class CodexEntryGenerator_GenerateBiomeEntries_Patch
				{
					public static void Postfix(Dictionary<string, CodexEntry>)
					{
					}
				}*//*

		//[HarmonyPatch(typeof(CodexEntryGenerator), "GenerateBiomeEntries")]
		public class CodexEntryGenerator_GenerateBiomeEntries_Patch
		{
			public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
			{
				var codes = orig.ToList();
				var m_name = AccessTools.PropertyGetter(typeof(SubWorld), "name");

				var m_InjectedMethod = AccessTools.DeclaredMethod(typeof(CodexEntryGenerator_GenerateBiomeEntries_Patch), nameof(GetName));

				for (int i = 0; i < codes.Count; i++)
				{
					var code = codes[i];
					if (code.Calls(m_name))
						codes[i + 1] = new CodeInstruction(OpCodes.Call, m_InjectedMethod);
				}
				return codes;
			}

			private static string GetName(string subWorldName)
			{
				if (Mod.subWorldsLoader.TryGet(subWorldName, out var subWorldsLoader))
				{
					return subWorldsLoader.template.Category.IsNullOrWhiteSpace()
						? subWorldName
						: $"subworlds/{subWorldsLoader.template.Category}/placeholder";
				}

				return subWorldName;
			}
		}
	}
}
*/