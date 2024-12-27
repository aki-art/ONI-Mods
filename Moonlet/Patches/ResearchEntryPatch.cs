#if TRANSPILERS
using HarmonyLib;
using Moonlet.Scripts;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace Moonlet.Patches
{
	public class ResearchEntryPatch
	{
		[HarmonyPatch(typeof(ResearchEntry), "SetTech")]
		public class ResearchEntry_SetTech_Patch
		{
			public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
			{
				var codes = orig.ToList();

				var m_GetFreeIcon = AccessTools.Method(typeof(ResearchEntry), nameof(ResearchEntry.GetFreeIcon), []);

				var index = codes.FindIndex(ci => ci.Calls(m_GetFreeIcon));

				if (index == -1)
				{
					Log.Warn("Could not patch ResearchEntry.SetTech. (1)");
					return codes;
				}

				var m_GetComponent = AccessTools.Method(typeof(Component), nameof(Component.GetComponent), [], [typeof(ToolTip)]);

				var index2 = codes.FindIndex(index, ci => ci.Calls(m_GetComponent));

				if (index2 == -1)
				{
					Log.Warn("Could not patch ResearchEntry.SetTech. (2)");
					return codes;
				}

				var m_ModifyDLCIcon = AccessTools.DeclaredMethod(typeof(ResearchEntry_SetTech_Patch), "ModifyDLCIcon");

				// inject right after the found index
				codes.InsertRange(index2 + 1,
				[
					// dup tooltip component on stack
					new CodeInstruction(OpCodes.Dup),
					new CodeInstruction(OpCodes.Ldloc, 6),
					new CodeInstruction(OpCodes.Call, m_ModifyDLCIcon)
				]);

				return codes;
			}

			private static void ModifyDLCIcon(ToolTip toolTip, TechItem techItem)
			{
				if (techItem == null || techItem.Id == null)
					return;

				if (toolTip == null)
				{
					Log.Warn("Tooltip is null. ResearchEntry.SetTech/ModifyDLCIcon");
					return;
				}

				Log.Debug($"modifying DLC icon: {(techItem.Id?.ToString() ?? "null")}");

				if (toolTip.TryGetComponent(out HierarchyReferences hierarchyRefs))
				{
					var prefab = Assets.TryGetPrefab(techItem.Id);
					if (prefab != null)
					{
						if (prefab.TryGetComponent(out MoonletComponentHolder holder))
						{
							if (holder.sourceMod != null)
							{
								var data = MoonletMods.Instance.GetModData(holder.sourceMod);
								if (data != null)
								{
									var dlcOverlay = hierarchyRefs.GetReference<KImage>("DLCOverlay");
									dlcOverlay.gameObject.SetActive(true);
									dlcOverlay.color = data.color;
								}
							}
						}
					}
				}
			}
		}
	}
}
#endif
