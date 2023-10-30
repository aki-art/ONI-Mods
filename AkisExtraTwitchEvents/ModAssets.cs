using FUtility;
using FUtility.FUI;
using System.IO;
using Twitchery.Content.Scripts;
using Twitchery.Content.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Twitchery
{
	public class ModAssets
	{
		private const float SPARKLE_DENSITY = 1000f / 5f;

		public static class Fx
		{
			public static SpawnFXHashes
				pinkPoof = (SpawnFXHashes)Hash.SDBMLower("AETE_PinkPoof"),
				fungusPoof = (SpawnFXHashes)Hash.SDBMLower("AETE_FungusPoof"),
				slimeSplat = (SpawnFXHashes)Hash.SDBMLower("AETE_SlimeSplat"),
				freezeMarker = (SpawnFXHashes)Hash.SDBMLower("AETE_FreezeMarker");

			public static AnimationClip disappearAnimation;
		}

		public static class Colors
		{
			public static Color midasGold = new Color(1f, 1f, 0.4f) * 5f;
			public static Color gold = new Color(1f, 1f, 0.4f);
		}

		public static class Sounds
		{
			public static string
				SPLAT = "aete_deltarune_splat",
				DOORBELL = "aete_doorbell",
				TEA = "aete_tea",
				EGG_SMASH = "aete_egg_smash",
				POP = "aete_pop",
				POLYMORHPH = "aete_polymorph",
				POLYMORHPH_END = "aete_polymorph_end",
				GOLD = "aete_gold",
				VICTORY = "aete_victory",
				WOOD_THUNK = "aete_wood_thunk",
				FART_REVERB = "aete_fart_reverb",
				LEAF = "aete_leaf";

			public static readonly string[] FREEZE_SOUNDS = new[]
			{
				"aete_freeze1",
				"aete_freeze2",
				"aete_freeze3"
			};
		}

		public static class Materials
		{
			public static Material pixelShaderMaterial;
			public static Material ditherMaterial;
			public static Material eggMaterial;
		}

		public static class Prefabs
		{
			public static GameObject
				sparkleCircle,
				pizzaDeliveryAnim,
				sparklePoof,
				slimyPulse,
				adventureScreen,
				freezeGunFx,
				spinnyWheel;
		}

		public static float GetSFXVolume() => KPlayerPrefs.GetFloat("Volume_SFX") * KPlayerPrefs.GetFloat("Volume_Master");

		public static void OnGameAssetsLoaded()
		{
			var kbac = FXHelpers.CreateEffect("aete_pizzatime_kanim", Vector3.zero, set_inactive: true);
			kbac.visibilityType = KAnimControllerBase.VisibilityType.Always;
			kbac.animScale = 0.25f;
			kbac.setScaleFromAnim = false;
			kbac.isMovable = false;
			kbac.materialType = KAnimBatchGroup.MaterialType.UI;
			kbac.animOverrideSize = new Vector2(100, 100);
			kbac.usingNewSymbolOverrideSystem = true;
			kbac.SetLayer(5);

			kbac.gameObject.SetActive(false);

			Prefabs.pizzaDeliveryAnim = kbac.gameObject;
		}

		public static void LoadAll()
		{
			var path = Path.Combine(FUtility.Utils.ModPath, "assets");

			AudioUtil.LoadSound(Sounds.SPLAT, Path.Combine(path, "snd_ralseising1.wav"));
			AudioUtil.LoadSound(Sounds.DOORBELL, Path.Combine(path, "370919__sjturia__doorbell.wav"));
			AudioUtil.LoadSound(Sounds.TEA, Path.Combine(path, "22890__stijn__tea.wav"));
			AudioUtil.LoadSound(Sounds.EGG_SMASH, Path.Combine(path, "617471__cupido-1__egg-fall.wav"));
			AudioUtil.LoadSound(Sounds.POLYMORHPH, Path.Combine(path, "polymorph.wav"));
			AudioUtil.LoadSound(Sounds.POLYMORHPH_END, Path.Combine(path, "polymorph_end.wav"));
			AudioUtil.LoadSound(Sounds.POP, Path.Combine(path, "67089__sunnysidesound__pop_3.wav"));
			AudioUtil.LoadSound(Sounds.GOLD, Path.Combine(path, "396501__alonsotm__firespell1_short_reversed.wav"));
			AudioUtil.LoadSound(Sounds.FREEZE_SOUNDS[0], Path.Combine(path, "freeze1.wav"));
			AudioUtil.LoadSound(Sounds.FREEZE_SOUNDS[1], Path.Combine(path, "freeze2.wav"));
			AudioUtil.LoadSound(Sounds.FREEZE_SOUNDS[2], Path.Combine(path, "freeze3.wav"));
			AudioUtil.LoadSound(Sounds.VICTORY, Path.Combine(path, "snd_dumbvictory.wav"));
			AudioUtil.LoadSound(Sounds.WOOD_THUNK, Path.Combine(path, "land_wood_02.wav"));
			AudioUtil.LoadSound(Sounds.LEAF, Path.Combine(path, "SG_gasGrass_leaf_03.wav"));

			AudioUtil.LoadSound(Sounds.FART_REVERB, Path.Combine(path, "reverb_fart.wav"));

			var bundle = FAssets.LoadAssetBundle("akis_twitch_events", platformSpecific: true);

			foreach (var asset in bundle.GetAllAssetNames())
			{
				Log.Debug(asset);
			}

			Materials.pixelShaderMaterial = bundle.LoadAsset<Material>("Assets/Scripts/Shader Tests/Pixel Art Shader/PixelShaderMat.mat");
			var ditherShader = bundle.LoadAsset<Shader>("Assets/Scripts/Shader Tests/Acerola Pixel Shader/Dither.shader");
			Materials.ditherMaterial = new Material(ditherShader);

			var eggShader = bundle.LoadAsset<Shader>("Assets/Sparkles/egg/EggShader.shader");
			Materials.eggMaterial = bundle.LoadAsset<Material>("Assets/Sparkles/egg/EggMaterial.mat");
			Materials.eggMaterial.shader = eggShader;


			LoadParticles(bundle);
			Prefabs.sparklePoof = LoadParticles(bundle, "Assets/TwitchIntegration/Sparkles.prefab");
			Prefabs.slimyPulse = bundle.LoadAsset<GameObject>("Assets/TwitchIntegration/SlimeToucher.prefab");

			Prefabs.freezeGunFx = LoadFreezeGunFx(bundle);

			LoadAdventureScreen(bundle);
			LoadSpinnyWheel(bundle);
		}

		private static void LoadSpinnyWheel(AssetBundle bundle)
		{
			var prefab = bundle.LoadAsset<GameObject>("Assets/TwitchIntegration/Wheel_tmpconverted.prefab");
			TMPConverter.ReplaceAllText(prefab);

			foreach (var locText in prefab.GetComponentsInChildren<LocText>())
			{
				locText.enableWordWrapping = true;
				locText.enableAutoSizing = true;
				locText.fontSizeMin = 2.6f;
				locText.fontSizeMax = 9.2f;
			}

			prefab.AddComponent<SpinnyWheel>();

			Fx.disappearAnimation = bundle.LoadAsset<AnimationClip>("Assets/Images/Klei/Disappear.anim");

			prefab.AddComponent<Animation>().clip = Fx.disappearAnimation;

			prefab.gameObject.SetActive(false);
			Prefabs.spinnyWheel = prefab;
		}

		private static GameObject LoadFreezeGunFx(AssetBundle bundle)
		{
			var prefab = bundle.LoadAsset<GameObject>("Assets/TwitchIntegration/FreezeGunOverlay.prefab");

			prefab.AddComponent<ParticleKiller>();

			prefab.transform.Find("RotatyOverlay A").gameObject.AddComponent<RotateSlowly>().rotationSpeed = 0.1f;
			prefab.transform.Find("RotatyOverlay B").gameObject.AddComponent<RotateSlowly>().rotationSpeed = -0.15f;

			prefab.SetActive(false);

			return prefab;
		}

		private static void LoadAdventureScreen(AssetBundle bundle)
		{
			Prefabs.adventureScreen = bundle.LoadAsset<GameObject>("Assets/TwitchIntegration/UI/StoryPanel_tmpconverted.prefab");
			TMPConverter.ReplaceAllText(Prefabs.adventureScreen);

			var cardPrefab = Prefabs.adventureScreen.transform.Find("Option");
			var card = cardPrefab.gameObject.AddComponent<Card>();
			card.label = card.transform.Find("Label").GetComponent<LocText>();
		}

		private static void LoadParticles(AssetBundle bundle)
		{
			var prefab = bundle.LoadAsset<GameObject>("Assets/Sparkles/sparklecircle/CircleofSparkle.prefab");
			prefab.SetLayerRecursively(Game.PickupableLayer);
			prefab.SetActive(false);

			var texture = bundle.LoadAsset<Texture2D>("Assets/Sparkles/sparklecircle/simple_sphere.png");

			var material = new Material(Shader.Find("Klei/BloomedParticleShader"))
			{
				mainTexture = texture,
				renderQueue = RenderQueues.Liquid
			};

			var renderer = prefab.GetComponent<ParticleSystemRenderer>();
			renderer.material = material;

			Prefabs.sparkleCircle = prefab;
		}

		private static GameObject LoadParticles(AssetBundle bundle, string path)
		{
			var prefab = bundle.LoadAsset<GameObject>(path);
			prefab.SetLayerRecursively(Game.PickupableLayer);
			prefab.SetActive(false);

			var renderer = prefab.GetComponent<ParticleSystemRenderer>();
			var texture = renderer.material.mainTexture;

			var material = new Material(Shader.Find("Klei/BloomedParticleShader"))
			{
				mainTexture = texture,
				renderQueue = RenderQueues.Liquid
			};

			renderer.material = material;

			return prefab;
		}

		public static void ConfigureSparkleCircle(GameObject sparkles, float radius, Color color)
		{
			if (sparkles.TryGetComponent(out ParticleSystem particles))
			{
				var scale = particles.shape.scale;
				scale.x = radius;
				scale.y = radius;

				var emission = particles.emission;
				emission.rateOverTimeMultiplier = (float)(radius * radius) * SPARKLE_DENSITY;

				var main = particles.main;
				main.startColor = color;
			}

			sparkles.SetActive(false);
			sparkles.SetActive(true);
		}

		public static LineRenderer DrawLine(Vector3 from, Vector3 to, Color color)
		{
			var renderer = AddSimpleLineRenderer(GameUtil.GetActiveTelepad().transform, color, color);
			renderer.positionCount = 2;
			renderer.SetPositions(new[]
			{
				from,
				to
			});

			return renderer;
		}

		public static LineRenderer AddSimpleLineRenderer(Transform transform, Color start, Color end, float width = 0.05f)
		{
			var gameObject = new GameObject("Akis_DebugLineRenderer");
			gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, Grid.GetLayerZ(Grid.SceneLayer.SceneMAX));
			transform.SetParent(transform);

			gameObject.SetActive(true);

			var debugLineRenderer = gameObject.AddComponent<LineRenderer>();

			debugLineRenderer.material = new Material(Shader.Find("Sprites/Default"))
			{
				renderQueue = RenderQueues.Liquid
			};
			debugLineRenderer.startColor = start;
			debugLineRenderer.endColor = end;
			debugLineRenderer.startWidth = debugLineRenderer.endWidth = width;
			debugLineRenderer.positionCount = 2;
			debugLineRenderer.useWorldSpace = true;

			return debugLineRenderer;
		}

		public static Text AddText(Vector3 position, Color color, string msg)
		{
			var gameObject = new GameObject();

			var rectTransform = gameObject.AddComponent<RectTransform>();
			rectTransform.SetParent(GameScreenManager.Instance.worldSpaceCanvas.GetComponent<RectTransform>());
			gameObject.transform.SetPosition(position);
			rectTransform.localScale = new Vector3(0.02f, 0.02f, 1f);

			var text2 = gameObject.AddComponent<Text>();
			text2.font = Assets.DebugFont;
			text2.text = msg;
			text2.color = color;
			text2.horizontalOverflow = HorizontalWrapMode.Overflow;
			text2.verticalOverflow = VerticalWrapMode.Overflow;
			text2.alignment = TextAnchor.MiddleCenter;

			return text2;
		}
	}
}
