using HarmonyLib;
using ProcGen;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Moonlet.Patches
{
	public class ColonyDestinationAsteroidBeltDataPatch
	{
		[HarmonyPatch]
		public class ColonyDestinationAsteroidBeltData_GenerateTraitDescriptors_Patch
		{
			public static IEnumerable<MethodBase> TargetMethods()
			{
				yield return AccessTools.Method(typeof(ColonyDestinationAsteroidBeltData), "GenerateTraitDescriptors", []);
				yield return AccessTools.Method(typeof(ColonyDestinationAsteroidBeltData), "GenerateTraitDescriptors", [typeof(ProcGen.World), typeof(bool)]);
			}

			public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
			{
				var codes = orig.ToList();

				var f_filePath = typeof(WorldTrait).GetField("filePath");
				var f_icon = typeof(WorldTrait).GetField("icon");

				var m_GetActualIcon = AccessTools.DeclaredMethod(typeof(ColonyDestinationAsteroidBeltData_GenerateTraitDescriptors_Patch), nameof(GetActualIcon));

				for (int i = codes.Count - 1; i >= 0; i--)
				{
					var instruction = codes[i];
					if (instruction.LoadsField(f_filePath))
						codes[i] = new CodeInstruction(OpCodes.Call, m_GetActualIcon);
				}

				return codes;
			}

			private static string GetActualIcon(WorldTrait trait)
			{
				Log.Debug("getting actual icon " + trait.traitTags.Contains(ModTags.MoonletWorldTrait.name));
				if (Mod.traitsLoader.TryGet(trait.filePath, out _))
				{
					Log.Debug($"moonlet trait: {trait.filePath}");
					if (!trait.icon.IsNullOrWhiteSpace())
						Log.Debug($"has override icon {trait.icon}");
				}

				return !trait.icon.IsNullOrWhiteSpace() && trait.traitTags.Contains(ModTags.MoonletWorldTrait.name)
					? trait.icon
					: trait.filePath;
			}
		}
	}
}
