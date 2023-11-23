using DecorPackB.Content.Scripts;
using HarmonyLib;
using UnityEngine;

namespace DecorPackB.Patches
{
	public class CapacityControlSideScreenPatch
	{
		[HarmonyPatch(typeof(CapacityControlSideScreen), "IsValidForTarget")]
		public class CapacityControlSideScreen_IsValidForTarget_Patch
		{
			public static void Postfix(GameObject target, ref bool __result)
			{
				if (__result && target.TryGetComponent(out Pot pot))
					__result = pot.ShouldShowSettings;
			}
		}
	}
}
