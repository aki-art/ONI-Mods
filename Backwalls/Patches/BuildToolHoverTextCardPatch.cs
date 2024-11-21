using Backwalls.Cmps;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace Backwalls.Patches
{
	public class BuildToolHoverTextCardPatch
	{
		[HarmonyPatch(typeof(BuildToolHoverTextCard), "UpdateHoverElements")]
		public class BuildToolHoverTextCard_UpdateHoverElements_Patch
		{
			public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
			{
				var codes = orig.ToList();

				var m_GetComponent = AccessTools.Method(
					typeof(GameObject),
					nameof(GameObject.GetComponent),
					[],
					[typeof(Rotatable)]);

				var index = codes.FindIndex(ci => ci.Calls(m_GetComponent));

				if (index == -1)
				{
					Log.Warning("cannot transpiler");
					return codes;
				}

				var m_AddDrawer = AccessTools.DeclaredMethod(typeof(BuildToolHoverTextCard_UpdateHoverElements_Patch), "AddDrawer");

				codes.InsertRange(index + 1,
				[
					new CodeInstruction(OpCodes.Ldarg_0),
					new CodeInstruction(OpCodes.Call, m_AddDrawer)
				]);

				return codes;
			}

			private static void AddDrawer(BuildToolHoverTextCard instance)
			{
				if (instance.currentDef == null)
					return;

				if (instance.currentDef.ObjectLayer == ObjectLayer.Backwall)
				{
					var drawer = HoverTextScreen.Instance.drawer;

					drawer.NewLine();
					drawer.AddIndent(8);

					var on = Backwalls_SmartBuildTool.Instance.IsToolActive()
						? $"<color=#FFFF00>{(global::STRINGS.UI.UISIDESCREENS.TIMEDSWITCHSIDESCREEN.ON)}</color>"
						: $"<color=#AAAAAA>{(global::STRINGS.UI.UISIDESCREENS.TIMEDSWITCHSIDESCREEN.OFF)}</color>";

					var str = string.Format(STRINGS.UI.BACKWALLS_MISC.SMART_CURSOR, on, GameUtil.GetActionString(BWActions.SmartBuildAction.GetKAction()));

					drawer.DrawText(str, instance.Styles_Instruction.Standard);
				}
			}
		}
	}
}
