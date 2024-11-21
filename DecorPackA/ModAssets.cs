using FUtility.FUI;
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
				settingsWindow,
				brushIcon,
				categoryHeaderPrefab,
				rotatableMoodlampSidescreen,
				particleSelectorSidescreen,
				tintableMoodlampSidescreen;


			public static Dictionary<string, GameObject> scatterLampPrefabs = new();
		}

		public static class PARTICLE_PREFAB_NAMES
		{
			public const string DISCO = "discoball";
			public const string SCATTERING = "scattering";
		}

		public static class Textures
		{
			public static Dictionary<string, Texture2D> particles = new();
		}

		public static class Tags
		{
			public static Tag
				stainedGlassDye = TagManager.Create(Mod.PREFIX + "StainedGlassMaterial"),
				stainedGlass = TagManager.Create(Mod.PREFIX + "StainedGlass", "Stained Glass"),
				noPaint = TagManager.Create("NoPaint"), // MaterialColor mod uses this
				tintable = TagManager.Create("DecorPackA_TintableLamp"),
				rotatable = TagManager.Create("DecorPackA_RotatableLamp"),
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
				extraPurple = new(1.54f, 0.71f, 2.21f),
				extraGreen = new(0f, 16.45f, 12.11f),
				extraOrange = new(2.55f, 0.66f, 0),
				palePink = new(1f, 0.6f, 0.7f),
				lavender = new(0.7f, 0.6f, 1f),
				corium = new(0f, 2f, 0),
				W_H_I_T_E = new(15f, 15f, 15f),
				abyssalite = new(0, 0.48f, 2.55f),
				extraBlue = new(0, 0.48f, 5f);
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

		public static void Load()
		{
			var bundle = FUtility.FAssets.LoadAssetBundle("decorpacki_assets", platformSpecific: true);
			LoadParticles(bundle);
			//LoadMaterials(bundle);

			Prefabs.scatterLampPrefabs = new()
			{
				[PARTICLE_PREFAB_NAMES.DISCO] = bundle.LoadAsset<GameObject>("Assets/DecorPackI/ScatterParticles/MoodLampParticlesDisco.prefab"),
				[PARTICLE_PREFAB_NAMES.SCATTERING] = bundle.LoadAsset<GameObject>("Assets/DecorPackI/ScatterParticles/MoodLampParticlesPurple.prefab"),
			};

			Prefabs.rotatableMoodlampSidescreen = bundle.LoadAsset<GameObject>("Assets/DecorPackI/UI/KnobSideScreen.prefab");
			Prefabs.tintableMoodlampSidescreen = bundle.LoadAsset<GameObject>("Assets/DecorPackI/UI/ColorSelectorSideScreen.prefab");
			Prefabs.particleSelectorSidescreen = bundle.LoadAsset<GameObject>("Assets/DecorPackI/UI/ParticleSelectorSideScreen.prefab");
			Prefabs.brushIcon = bundle.LoadAsset<GameObject>("Assets/DecorPackI/BrushIcon.prefab");

			Textures.particles = new()
			{
				{ "stars", bundle.LoadAsset<Texture2D>("Assets/DecorPackI/ScatterParticles/stars_particles.png") },
				{ "rad", bundle.LoadAsset<Texture2D>("Assets/DecorPackI/ScatterParticles/radiaton.png") },
				{ "disco", bundle.LoadAsset<Texture2D>("Assets/DecorPackI/ScatterParticles/disco_particles.png") },
				{ "invaders", bundle.LoadAsset<Texture2D>("Assets/DecorPackI/ScatterParticles/invaders.png") },
				{ "hearts", bundle.LoadAsset<Texture2D>("Assets/DecorPackI/ScatterParticles/hearts.png") },
				{ "soft", bundle.LoadAsset<Texture2D>("Assets/DecorPackI/ScatterParticles/soft.png") },
			};

			//var settingsBundle = FAssets.LoadAssetBundle("universal_settingsdialog_v1_1", platformSpecific: true);
			Prefabs.settingsWindow = bundle.LoadAsset<GameObject>("Assets/DecorPackI/UI/DecorPackA_SettingsDialogB_tmpconverted.prefab");

			TMPConverter.ReplaceAllText(Prefabs.tintableMoodlampSidescreen);
			TMPConverter.ReplaceAllText(Prefabs.settingsWindow);
		}

		/*		private static void LoadMaterials(AssetBundle bundle)
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
				}*/

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
