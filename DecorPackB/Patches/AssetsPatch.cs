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
				foreach (var decor in Assets.BlockTileDecorInfos)
				{
					Log.Debug("Decor: " + decor.name);
					foreach (var item in decor.atlas.items)
					{
						Log.Debug($"\t - name:\t{item.name}");
						Log.Debug($"\t - UVs");
						foreach (var uv in item.uvs)
							Log.Debug($"\t\t{uv}");

						Log.Debug($"\t - indices");
						foreach (var uv in item.indices)
							Log.Debug($"\t\t{uv}");

						Log.Debug($"\t - vertices");
						foreach (var uv in item.vertices)
							Log.Debug($"\t\t{uv}");

						Log.Debug($"\t - Box: {item.uvBox}");
					}
					Log.Debug("-----------------------------------------------------------------");
				}
			}
		}
	}
}
