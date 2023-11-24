using HarmonyLib;

namespace DecorPackB.Patches
{
	public class AssetsPatch
	{
		[HarmonyPatch(typeof(Assets), "OnPrefabInit")]
		public class Assets_OnPrefabInit_Patch
		{
			public static void Prefix(Assets __instance)
			{
				FUtility.Assets.LoadSprites(__instance);


			}

			public static void Postfix(Assets __instance)
			{
				MakeMovable("FossilBitsLarge");
				MakeMovable("FossilBitsSmall");
			}

			private static void MakeMovable(string prefabId)
			{
				var prefab = Assets.TryGetPrefab("FossilBitsLarge");
				prefab.AddOrGet<Pickupable>();
				prefab.AddOrGet<Movable>();
			}
		}
	}
}
