using HarmonyLib;
using PeterHan.PLib.Core;

namespace GoldenThrone.Patches
{
	public class AssetsPatch
	{
		[HarmonyPatch(typeof(Assets), "OnPrefabInit")]
		public class Assets_OnPrefabInit_Patch
		{
			public static void Postfix()
			{
				PGameUtils.CopySoundsToAnim("gold_flush_kanim", "toiletflush_kanim");
			}
		}
	}
}
