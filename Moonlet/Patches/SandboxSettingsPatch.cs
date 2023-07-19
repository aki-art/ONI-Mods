using HarmonyLib;
using Moonlet.Content.Scripts;
using System;
using static ProcGen.SubWorld;

namespace Moonlet.Patches
{
	public class SandboxSettingsPatch
	{
		[HarmonyPatch(typeof(SandboxSettings), MethodType.Constructor, new Type[] { })]
		public class SandboxSettings_Ctor_Patch
		{
			public static void Postfix(SandboxSettings __instance)
			{
				__instance.AddIntSetting(
					SandboxToolParameterMenuPatch.SETTING_ID,
					value => Moonlet_Mod.Instance.Trigger(ModEvents.OnSandboxZoneTypeChanged, value),
					(int)ZoneType.Sandstone);
			}
		}
	}
}
