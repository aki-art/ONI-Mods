using HarmonyLib;
using Twitchery.Content.Scripts;
using UnityEngine;
using static UnityEngine.UI.Image;

namespace Twitchery.Patches
{
	public class GravePatch
	{
		[HarmonyPatch(typeof(Grave), "OnStorageChanged")]
		public class Grave_OnStorageChanged_Patch
		{
			public static void Prefix(Grave __instance, ref object data)
			{
				if (data is GameObject minion && __instance.TryGetComponent(out AETE_GraveStoneMinionStorage graveStorage))
				{
					__instance.graveName = minion.name;
					graveStorage.OnDupeBuried(minion);
					data = null;
				}
			}
		}
	}
}
