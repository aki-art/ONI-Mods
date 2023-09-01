using System.Collections.Generic;
using UnityEngine;

namespace DecorPackA
{
	public class ModAssets
	{
		public static class Materials
		{
			public static Material fancyReflectionMat;
		}

		public static class Prefabs
		{
			public static GameObject
				sparklesParticles,
				categoryHeaderPrefab;

			public static Dictionary<string, GameObject> scatterLampPrefabs = new();
		}

		public static class Tags
		{
			public static Tag
				stainedGlassDye = TagManager.Create(Mod.PREFIX + "StainedGlassMaterial"),
				stainedGlass = TagManager.Create(Mod.PREFIX + "StainedGlass", "Stained Glass"),
				noPaint = TagManager.Create("NoPaint"), // MaterialColor mod uses this
				noBackwall = TagManager.Create("NoBackwall"); // Background Tiles mod uses this

			public static TagSet extraGlassDyes = new();
		}

		public static class Colors
		{
			// "out of range" values are used to make some effect stronger, Unity does not clamp color values
			public static Color
				bloodRed = Color.red * 1.7f,
				gold = new(1.3f, 0.96f, 0.5f),
				uraniumGreen = new(0f, 3f, 0.6f),
				extraPink = new(1.5f, 0, 0.7f),
				extraGreen = new(0f, 16.45f, 12.11f),
				extraOrange = new(2.55f, 0.66f, 0),
				palePink = new(1f, 0.6f, 0.7f),
				lavender = new(0.7f, 0.6f, 1f),
				corium = new(0f, 2f, 0),
				W_H_I_T_E = new(15f, 15f, 15f),
				abyssalite = new(0, 0.48f, 2.55f);
		}

		public static void Load()
		{
			var bundle = FUtility.Assets.LoadAssetBundle("decorpacki_assets", platformSpecific: true);
			LoadParticles(bundle);
			LoadMaterials(bundle);

			Prefabs.scatterLampPrefabs = new()
			{
				["discoball"] = bundle.LoadAsset<GameObject>("Assets/DecorPackI/ScatterParticles/MoodLampParticlesDisco.prefab"),
				["scatteringyellow"] = bundle.LoadAsset<GameObject>("Assets/DecorPackI/ScatterParticles/MoodLampParticlesYellow.prefab"),
				["scatteringblue"] = bundle.LoadAsset<GameObject>("Assets/DecorPackI/ScatterParticles/MoodLampParticlesBlue.prefab"),
				["scatteringgreen"] = bundle.LoadAsset<GameObject>("Assets/DecorPackI/ScatterParticles/MoodLampParticlesRad.prefab"),
				["scatteringpurple"] = bundle.LoadAsset<GameObject>("Assets/DecorPackI/ScatterParticles/MoodLampParticlesPurple.prefab"),
			};
		}

		private static void LoadMaterials(AssetBundle bundle)
		{
			var shinyShader = bundle.LoadAsset<Shader>("Assets/DecorPackI/ShinySculptures/ShinyShader.shader");
			var diamondNormalButItTiles = bundle.LoadAsset<Texture2D>("Assets/DecorPackI/ShinySculptures/normal_diamond.png");
			var shinyMat = bundle.LoadAsset<Material>("Assets/DecorPackI/ShinySculptures/ShinyMat.mat");
			shinyMat.shader = shinyShader;
			shinyMat.renderQueue = RenderQueues.Liquid + 1;
			shinyMat.SetTexture("_Mask", diamondNormalButItTiles);
			shinyMat.SetTextureOffset("_Mask", new Vector2(0.1f, 0.1f));
			shinyMat.SetFloat("_Strength", 1.7f);
			shinyMat.SetFloat("_Minimum", 0.3f);

			Materials.fancyReflectionMat = shinyMat;
		}

		private static void LoadParticles(AssetBundle bundle)
		{
			var prefab = bundle.LoadAsset<GameObject>("Assets/DecorPackI/Particles/Sparkles/Sparkles.prefab");
			prefab.SetLayerRecursively(Game.PickupableLayer);
			prefab.SetActive(false);

			var texture = bundle.LoadAsset<Texture2D>("Assets/DecorPackI/Particles/Sparkles/star.png");

			var material = new Material(Shader.Find("Klei/BloomedParticleShader"))
			{
				mainTexture = texture,
				renderQueue = RenderQueues.Liquid
			};

			var renderer = prefab.GetComponent<ParticleSystemRenderer>();
			renderer.material = material;

			Prefabs.sparklesParticles = prefab;
		}
	}
}
