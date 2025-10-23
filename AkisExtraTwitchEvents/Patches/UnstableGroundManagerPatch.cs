using HarmonyLib;
using System.Collections.Generic;
using Twitchery.Content;
using UnityEngine;

namespace Twitchery.Patches
{
	public class UnstableGroundManagerPatch
	{
		[HarmonyPatch(typeof(UnstableGroundManager), nameof(UnstableGroundManager.OnPrefabInit))]
		public class UnstableGroundManager_OnPrefabInit_Patch
		{
			public static void Prefix(UnstableGroundManager __instance)
			{
				var effects = new List<UnstableGroundManager.EffectInfo>(__instance.effects);
				var referenceEffect = effects.Find(e => e.element == SimHashes.Sand);

				if (referenceEffect.prefab == null)
					return;

				var effect = CreateEffect(Elements.YellowSnow, Elements.YellowSnow.id, referenceEffect.prefab);

				__instance.effects = __instance.effects.AddToArray(effect);
			}


			private static UnstableGroundManager.EffectInfo CreateEffect(SimHashes element, string prefabId, GameObject referencePrefab)
			{
				var prefab = Object.Instantiate(referencePrefab);
				prefab.name = $"Unstable{prefabId}";

				return new UnstableGroundManager.EffectInfo()
				{
					prefab = prefab,
					element = element
				};
			}
		}
	}
}
