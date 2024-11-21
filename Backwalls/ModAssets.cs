using Backwalls.CustomTile;
using FUtility.FUI;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Backwalls
{
	internal class ModAssets
	{
		public static GameObject wallSidescreenPrefab;
		public static GameObject settingsDialogPrefab;
		public static GameObject contextMenuPrefab;

		public static Texture2D blankTileTex;

		public static Dictionary<string, Sprite> uiSprites;

		public static CustomTileLoader customTiles;

		public static Texture2D borderTex;
		public static Texture2D maskTex;

		public static Material shinyTileMaterial;
		public static Material rainbowTileMaterial;

		public class Tags
		{
			public static readonly Tag noBackwall = TagManager.Create("BW_NoBackwall");
		}

		public static readonly Color[] colors = new[]
		{
			new Color(0.4862745f, 0.4862745f, 0.4862745f),
			new Color(0f, 0f, 0.9882353f),
			new Color(0f, 0f, 0.7372549f),
			new Color(0.26666668f, 0.15686275f, 0.7372549f),
			new Color(0.5803922f, 0f, 0.5176471f),
			new Color(0.65882355f, 0f, 0.1254902f),
			new Color(0.65882355f, 0.0627451f, 0f),
			new Color(0.53333336f, 0.078431375f, 0f),
			new Color(0.3137255f, 0.1882353f, 0f),
			new Color(0f, 0.47058824f, 0f),
			new Color(0f, 0.40784314f, 0f),
			new Color(0f, 0.34509805f, 0f),
			new Color(0f, 0.2509804f, 0.34509805f),
			new Color(0f, 0f, 0f),
			new Color(0.7372549f, 0.7372549f, 0.7372549f),
			new Color(0f, 0.47058824f, 0.972549f),
			new Color(0f, 0.34509805f, 0.972549f),
			new Color(0.40784314f, 0.26666668f, 0.9882353f),
			new Color(0.84705883f, 0f, 0.8f),
			new Color(0.89411765f, 0f, 0.34509805f),
			new Color(0.972549f, 0.21960784f, 0f),
			new Color(0.89411765f, 0.36078432f, 0.0627451f),
			new Color(0.6745098f, 0.4862745f, 0f),
			new Color(0f, 0.72156864f, 0f),
			new Color(0f, 0.65882355f, 0f),
			new Color(0f, 0.65882355f, 0.26666668f),
			new Color(0f, 0.53333336f, 0.53333336f),
			new Color(0f, 0f, 0f),
			new Color(0.972549f, 0.972549f, 0.972549f),
			new Color(0.23529412f, 0.7372549f, 0.9882353f),
			new Color(0.40784314f, 0.53333336f, 0.9882353f),
			new Color(0.59607846f, 0.47058824f, 0.972549f),
			new Color(0.972549f, 0.47058824f, 0.972549f),
			new Color(0.972549f, 0.34509805f, 0.59607846f),
			new Color(0.972549f, 0.47058824f, 0.34509805f),
			new Color(0.9882353f, 0.627451f, 0.26666668f),
			new Color(0.972549f, 0.72156864f, 0f),
			new Color(0.72156864f, 0.972549f, 0.09411765f),
			new Color(0.34509805f, 0.84705883f, 0.32941177f),
			new Color(0.34509805f, 0.972549f, 0.59607846f),
			new Color(0f, 0.9098039f, 0.84705883f),
			new Color(0.47058824f, 0.47058824f, 0.47058824f), // default
			new Color(0.9882353f, 0.9882353f, 0.9882353f),
			new Color(0.6431373f, 0.89411765f, 0.9882353f),
			new Color(0.72156864f, 0.72156864f, 0.972549f),
			new Color(0.84705883f, 0.72156864f, 0.972549f),
			new Color(0.972549f, 0.72156864f, 0.972549f),
			new Color(0.972549f, 0.72156864f, 0.7529412f),
			new Color(0.9411765f, 0.8156863f, 0.6901961f),
			new Color(0.9882353f, 0.8784314f, 0.65882355f),
			new Color(0.972549f, 0.84705883f, 0.47058824f),
			new Color(0.84705883f, 0.972549f, 0.47058824f),
			new Color(0.72156864f, 0.972549f, 0.72156864f),
			new Color(0.72156864f, 0.972549f, 0.84705883f),
			new Color(0f, 0.9882353f, 0.9882353f),
			new Color(0.84705883f, 0.84705883f, 0.84705883f)
		};

		public static void LoadAssets()
		{
			var bundle = FAssets.LoadAssetBundle("backwallassets", platformSpecific: true);
			wallSidescreenPrefab = bundle.LoadAsset<GameObject>("Assets/backwalls/WallSidescreen.prefab");
			settingsDialogPrefab = bundle.LoadAsset<GameObject>("Assets/backwalls/SettingsDialog.prefab");
			contextMenuPrefab = bundle.LoadAsset<GameObject>("Assets/backwalls/ContextMenu.prefab");

			TMPConverter.ReplaceAllText(wallSidescreenPrefab);
			TMPConverter.ReplaceAllText(settingsDialogPrefab);
			TMPConverter.ReplaceAllText(contextMenuPrefab);

			blankTileTex = FAssets.LoadTexture(Path.Combine(Utils.AssetsPath, "blank_tile.png"));

			var shader = bundle.LoadAsset<Shader>("Assets/backwalls/TileShader.shader");
			borderTex = bundle.LoadAsset<Texture2D>("Assets/backwalls/border.png");
			maskTex = bundle.LoadAsset<Texture2D>("Assets/backwalls/mask.png");
			customTiles = new CustomTileLoader(Path.Combine(Mod.configFolder, "custom_walls"), new Material(shader));

			var shaderBundle = FAssets.LoadAssetBundle("backwalls_tileshader", platformSpecific: true);
			shinyTileMaterial = shaderBundle.LoadAsset<Material>("Assets/TileFake/TileMaterial.mat");
			rainbowTileMaterial = shaderBundle.LoadAsset<Material>("Assets/TileFake/SGTRainbowMat.mat");
		}
	}
}
