using HarmonyLib;
using Moonlet.Content.Tools;
using UnityEngine;

namespace Moonlet.Patches
{
	public class PlayerControllerPatch
	{
		[HarmonyPatch(typeof(PlayerController), "OnPrefabInit")]
		public class PlayerController_OnPrefabInit_Patch
		{
			public static void Postfix(PlayerController __instance)
			{
				var go = new GameObject(nameof(Moonlet_ZonetypePainterTool));
				var tool = go.AddComponent<Moonlet_ZonetypePainterTool>();

				tool.transform.SetParent(__instance.gameObject.transform);
				tool.gameObject.SetActive(true);
				tool.gameObject.SetActive(false);

				__instance.tools = __instance.tools.AddToArray(tool);
			}
		}
	}
}
