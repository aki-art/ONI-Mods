using FUtility;
using HarmonyLib;
using Moonlet.Scripts;
using Moonlet.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ProcGen.SubWorld;

namespace Moonlet.Patches
{
	public class SandboxToolParameterMenuPatch
	{
		[HarmonyPatch(typeof(SandboxToolParameterMenu), nameof(SandboxToolParameterMenu.ConfigureEntitySelector))]
		public static class SandboxToolParameterMenu_ConfigureEntitySelector_Patch
		{
			public static void Postfix(SandboxToolParameterMenu __instance)
			{
				var menu = SandboxUtil.AddModMenu(
					__instance,
					"Moonlet Temporary",
					Def.GetUISprite(Assets.GetPrefab(CookedMeatConfig.ID)),
					obj => obj is KPrefabID id && id.TryGetComponent<MoonletComponentHolder>(out var holder) && holder.addToSandboxMenu);

				SandboxUtil.UpdateOptions(__instance);
			}
		}

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

				if (ZoneTypeUtil.GetZoneTypes().Count > 0)
					options.AddRange(ZoneTypeUtil.GetZoneTypes().Select(z => (object)z));

				var sprite = Assets.GetSprite("moonlet_biome_tool_marker");

				zoneTypeSelector = new SandboxToolParameterMenu.SelectorValue(
					[.. options],
					zone => menu.settings.SetIntSetting(SETTING_ID, (int)(ZoneType)zone),
					zone => ZoneTypeUtil.TryGetName((ZoneType)(int)zone, out string name) ? name : ((ZoneType)(int)zone).ToString(),
					null,
					zone =>
					{
						Color32 color = World.Instance.zoneRenderData.zoneColours.Length < (int)zone
							? World.Instance.zoneRenderData.zoneColours[(int)zone]
							: Color.magenta;

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
