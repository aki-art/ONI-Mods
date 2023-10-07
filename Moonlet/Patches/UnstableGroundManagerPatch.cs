using HarmonyLib;
using System.Collections.Generic;
using static UnstableGroundManager;

namespace Moonlet.Patches
{
	public class UnstableGroundManagerPatch
	{
		[HarmonyPatch(typeof(UnstableGroundManager), nameof(UnstableGroundManager.OnPrefabInit))]
		public class UnstableGroundManager_OnPrefabInit_Patch
		{
			public static void Prefix(ref EffectInfo[] ___effects)
			{
				if (!Mod.elementsLoader.IsActive())
					return;

				var effects = new List<EffectInfo>(___effects);
				var referenceEffect = effects.Find(e => e.element == SimHashes.Sand);

				if (referenceEffect.prefab == null)
					return;

				Mod.elementsLoader.CreateUnstableFallers(ref effects, referenceEffect);

				___effects = effects.ToArray();
			}
		}
	}
}
