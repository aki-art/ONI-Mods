using FUtility;
using HarmonyLib;

namespace SoftLockBegone.Patches
{
	internal class ConstructablePatch
	{

		[HarmonyPatch(typeof(Constructable), "OnSpawn")]
		public class Constructable_OnSpawn_Patch
		{
			public static void Prefix(Constructable __instance)
			{
				Log.Assert("__instance.SelectedElementsTags", __instance.SelectedElementsTags);
			}
		}
	}
}
