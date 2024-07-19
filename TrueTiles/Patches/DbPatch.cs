using HarmonyLib;
using TrueTiles.Cmps;

namespace TrueTiles.Patches
{
	public class DbPatch
	{
		[HarmonyPatch(typeof(Db), "Initialize")]
		public static class Db_Initialize_Patch
		{
			public static void Prefix()
			{
				//TexturePacksManager.Instance.ReadUserSettings();
				Mod.ScanOtherMods();

				// my own UI assets
				ModAssets.LateLoadAssets();

				// initializing components
				var go = Global.Instance.gameObject;
				go.AddComponent<TileAssets>();
				go.AddComponent<TileAssetLoader>();
				go.AddComponent<TexturePacksManager>();

				// Loads pack data
				//TexturePacksManager.Instance.LoadAllPacksFromFolder(Path.Combine(Mod.ModPath, "tiles"));

				if (Mod.addonPacks != null)
				{
					foreach (var pack in Mod.addonPacks)
					{
						TexturePacksManager.Instance.LoadPack(pack);
					}
				}

				// Should be loaded very last
				TexturePacksManager.Instance.LoadExteriorPacks();

				// sort packs
				TexturePacksManager.Instance.SortPacks();

				// Load actual assets and textures
				TileAssetLoader.Instance.LoadEnabledPacks(TexturePacksManager.Instance.packs);
			}
		}
	}
}
