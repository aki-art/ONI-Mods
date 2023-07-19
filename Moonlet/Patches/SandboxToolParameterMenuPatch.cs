using HarmonyLib;
using Moonlet.ZoneTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ProcGen.SubWorld;

namespace Moonlet.Patches
{
	internal class SandboxToolParameterMenuPatch
	{
		public static SandboxToolParameterMenu.SelectorValue zoneTypeSelector;
		public const string SETTING_ID = "SandboxTools.Moonlet_SelectedZoneType";

		[HarmonyPatch(typeof(SandboxToolParameterMenu), "OnSpawn")]
		public class SandboxToolParameterMenu_OnSpawn_Patch
		{
			public static void Prefix(SandboxToolParameterMenu __instance)
			{
				ConfigureZoneTypesSelector(__instance);
				__instance.SpawnSelector(zoneTypeSelector);
			}

			private static void ConfigureZoneTypesSelector(SandboxToolParameterMenu menu)
			{
				var options = new List<object>();

				foreach (ZoneType vanillaType in Enum.GetValues(typeof(ZoneType)))
					options.Add(vanillaType);

				if (ZoneTypeUtil.GetCount() > 0)
					options.AddRange(ZoneTypeUtil.GetZoneTypes().Select(z => (object)z));

				var sprite = Assets.GetSprite("moonlet_biome_tool_marker");

				zoneTypeSelector = new SandboxToolParameterMenu.SelectorValue(
					options.ToArray(),
					zone => menu.settings.SetIntSetting(SETTING_ID, (int)(ZoneType)zone),
					zone => ZoneTypeUtil.TryGetName((ZoneType)(int)zone, out string name) ? name : ((ZoneType)(int)zone).ToString(),
					null,
					zone =>
					{
						var color = World.Instance.zoneRenderData.GetZoneColor((ZoneType)(int)zone);
						return new Tuple<Sprite, Color>(sprite, color);
					},
					"Zone Type");
			}
		}

		[HarmonyPatch(typeof(SandboxToolParameterMenu), "DisableParameters")]
		public class SandboxToolParameterMenu_DisableParameters_Patch
		{
			public static void Postfix()
			{
				zoneTypeSelector.row.SetActive(false);
			}
		}
	}
}
