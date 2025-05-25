using FUtility.FUI;
using System.IO;
using TMPro;
using Twitchery.Content.Scripts;
using Twitchery.Content.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Twitchery
{
	public class ModAssets
	{
		private const float SPARKLE_DENSITY = 1000f / 5f;

		public static class TMP_GradientPresetIDs
		{
			public const string SUPER_DUPE = "AETE_SuperDupe";
		}

		public static class Fx
		{
			public static SpawnFXHashes
				pinkPoof = (SpawnFXHashes)Hash.SDBMLower("AETE_PinkPoof"),
				fungusPoof = (SpawnFXHashes)Hash.SDBMLower("AETE_FungusPoof"),
				bigZap = (SpawnFXHashes)Hash.SDBMLower("AETE_Zap"),
				slimeSplat = (SpawnFXHashes)Hash.SDBMLower("AETE_SlimeSplat"),
				eggSplat = (SpawnFXHashes)Hash.SDBMLower("AETE_EggSplat"),
				freezeMarker = (SpawnFXHashes)Hash.SDBMLower("AETE_FreezeMarker");

			public static AnimationClip disappearAnimation;
		}

		public static class Colors
		{
			public static Color midasGold = new Color(1f, 1f, 0.4f) * 5f;
			public static Color gold = new Color(1f, 1f, 0.4f);
			public static Color sweatBlue = Util.ColorFromHex("5897f5");

		}

		public static class Sounds
		{
			public static int
				SPLAT = Hash.SDBMLower("aete_deltarune_splat"),
				DOORBELL = Hash.SDBMLower("aete_doorbell"),
				TEA = Hash.SDBMLower("aete_tea"),
				EGG_SMASH = Hash.SDBMLower("aete_egg_smash"),
				POP = Hash.SDBMLower("aete_pop"),
				POLYMORHPH = Hash.SDBMLower("aete_polymorph"),
				POLYMORHPH_END = Hash.SDBMLower("aete_polymorph_end"),
				GOLD = Hash.SDBMLower("aete_gold"),
				VICTORY = Hash.SDBMLower("aete_victory"),
				WOOD_THUNK = Hash.SDBMLower("aete_wood_thunk"),
				FART_REVERB = Hash.SDBMLower("aete_fart_reverb"),
				LEAF = Hash.SDBMLower("aete_leaf"),
				ROCK = Hash.SDBMLower("aete_rock"),
				SNIP = Hash.SDBMLower("aete_snip"),
				WARNING = Hash.SDBMLower("aete_warning"),
				VACUUM = Hash.SDBMLower("aete_vacuum"),
				ELECTRIC_SHOCK = Hash.SDBMLower("aete_electricshock"),
				CERAMIC_BREAK = Hash.SDBMLower("aete_ceramicbreak"),
				LASER_CHARGE = Hash.SDBMLower("aete_laser_charge"),
				LASER = Hash.SDBMLower("aete_laser"),
				FLUSH = Hash.SDBMLower("aete_flush"),
				SLURP = Hash.SDBMLower("aete_slurp"),
				PASTA = Hash.SDBMLower("aete_pasta"),
				SUBNAUTICA = Hash.SDBMLower("aete_subnautica");

			public static readonly int[] PLOP_SOUNDS =
			[
				Hash.SDBMLower("aete_plop0"),
				Hash.SDBMLower("aete_plop1"),
				Hash.SDBMLower("aete_plop2"),
				Hash.SDBMLower("aete_plop3"),
			];

			public static readonly int[] FREEZE_SOUNDS =
			[
				Hash.SDBMLower("aete_freeze1"),
				Hash.SDBMLower("aete_freeze2"),
				Hash.SDBMLower("aete_freeze3")
			];

			public static readonly int[] BALLOON_DEFLATE =
			[
				Hash.SDBMLower("aete_balloondeflate1"),
				Hash.SDBMLower("aete_balloondeflate2")
			];
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
				shockwave,
				adventureScreen,
				freezeGunFx,
				magicalFloxFx,
				fireOverlay,
				solarStormQuad,
				spinnyWheel,
				forestToucher,
				overlayQuad,
				carcersCursePrompt,
				deathLaser,
				superDupeTrail,
				bigWormHead,
				bigWormBody,
				bigWormButt,
				smallWormHead,
				smallWormBody,
				smallWormButt,
				oilDripFx;

			public static GameObject fallingStuffOverlay;
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
			TmpExtensions();

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

			AudioUtil.LoadSound(Sounds.PLOP_SOUNDS[0], Path.Combine(path, "plorp_1.wav"));
			AudioUtil.LoadSound(Sounds.PLOP_SOUNDS[1], Path.Combine(path, "plorp_2.wav"));
			AudioUtil.LoadSound(Sounds.PLOP_SOUNDS[2], Path.Combine(path, "plorp_3.wav"));
			AudioUtil.LoadSound(Sounds.PLOP_SOUNDS[3], Path.Combine(path, "plorp_4.wav"));

			AudioUtil.LoadSound(Sounds.VICTORY, Path.Combine(path, "snd_dumbvictory.wav"));
			AudioUtil.LoadSound(Sounds.WOOD_THUNK, Path.Combine(path, "land_wood_02.wav"));
			AudioUtil.LoadSound(Sounds.LEAF, Path.Combine(path, "SG_gasGrass_leaf_03.wav"));
			AudioUtil.LoadSound(Sounds.FART_REVERB, Path.Combine(path, "reverb_fart.wav"));
			AudioUtil.LoadSound(Sounds.ROCK, Path.Combine(path, "560510__nox_sound__explosion_debris_short_stereo.wav"));
			AudioUtil.LoadSound(Sounds.SNIP, Path.Combine(path, "629513__abir19__scissorssnips.wav"));
			AudioUtil.LoadSound(Sounds.WARNING, Path.Combine(path, "SG_HUD_techView_off_short.wav"));
			AudioUtil.LoadSound(Sounds.VACUUM, Path.Combine(path, "81573__bennstir__vacuum1.wav"));
			AudioUtil.LoadSound(Sounds.ELECTRIC_SHOCK, Path.Combine(path, "ElectricShock.wav"));
			AudioUtil.LoadSound(Sounds.CERAMIC_BREAK, Path.Combine(path, "554367__nox_sound__foley_impact_smash_tiles_stereo.wav"));
			AudioUtil.LoadSound(Sounds.LASER_CHARGE, Path.Combine(path, "brimstone charge up_1.wav"));
			AudioUtil.LoadSound(Sounds.LASER, Path.Combine(path, "brimstone lazer_2.wav"));
			AudioUtil.LoadSound(Sounds.FLUSH, Path.Combine(path, "flush.wav"));
			AudioUtil.LoadSound(Sounds.SLURP, Path.Combine(path, "433962__brunchik__slurp.wav"));
			AudioUtil.LoadSound(Sounds.PASTA, Path.Combine(path, "zapsplat_food_pasta_dried_handful_drop_into_empty_metal_pan_35059.wav"));
			AudioUtil.LoadSound(Sounds.SUBNAUTICA, Path.Combine(path, "subnautica.wav"));

			AudioUtil.LoadSound(Sounds.BALLOON_DEFLATE[0], Path.Combine(path, "fungus_balloon_death_1.wav"));
			AudioUtil.LoadSound(Sounds.BALLOON_DEFLATE[1], Path.Combine(path, "fungus_balloon_death_2.wav"));

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
			Prefabs.forestToucher = bundle.LoadAsset<GameObject>("Assets/TwitchIntegration/ForestToucher.prefab");


			Prefabs.forestToucher.transform.Find("mask").GetComponent<SpriteRenderer>().material = new Material(Shader.Find("Klei/BloomedParticleShader"))
			{
				mainTexture = bundle.LoadAsset<Texture2D>("Assets/TwitchIntegration/mask.png"),
				renderQueue = 4500
			};

			Prefabs.forestToucher.transform.Find("Particle System").GetComponent<ParticleSystemRenderer>().material = new Material(Shader.Find("TextMeshPro/Sprite"))
			{
				mainTexture = bundle.LoadAsset<Texture2D>("Assets/TwitchIntegration/leaf.png"),
				renderQueue = 4500
			};

			Prefabs.carcersCursePrompt = bundle.LoadAsset<GameObject>("Assets/TwitchIntegration/CarcerPrompt_tmpconverted.prefab");
			TMPConverter.ReplaceAllText(Prefabs.carcersCursePrompt);

			Prefabs.shockwave = bundle.LoadAsset<GameObject>("Assets/TwitchIntegration/ShockWave.prefab");
			Prefabs.freezeGunFx = LoadFreezeGunFx(bundle);

			LoadAdventureScreen(bundle);
			LoadSpinnyWheel(bundle);
			LoadSolarStorm(bundle);
			LoadSimpleOverlay(bundle);

			var fxBundle = FAssets.LoadAssetBundle("aetn_fireoverlay", platformSpecific: true);

			LoadFireOverlay(fxBundle);
			LoadDeathLaser(fxBundle);
			Prefabs.superDupeTrail = fxBundle.LoadAsset<GameObject>("Assets/AkisExtraTwitchEvents/SuperDupeTrail.prefab");
			var trailMat = new Material(Shader.Find("TextMeshPro/Sprite"));
			trailMat.renderQueue = RenderQueues.WorldTransparent;
			Prefabs.superDupeTrail.GetComponentInChildren<TrailRenderer>().material = trailMat;

			Prefabs.bigWormHead = fxBundle.LoadAsset<GameObject>("Assets/AkisExtraTwitchEvents/worm/LargeWormHeadPrefab.prefab");
			Prefabs.bigWormBody = fxBundle.LoadAsset<GameObject>("Assets/AkisExtraTwitchEvents/worm/LargeWormBodyPrefab.prefab");
			Prefabs.bigWormButt = fxBundle.LoadAsset<GameObject>("Assets/AkisExtraTwitchEvents/worm/LargeWormButtPrefab.prefab");

			Prefabs.smallWormHead = fxBundle.LoadAsset<GameObject>("Assets/AkisExtraTwitchEvents/worm/SmallWormHead.prefab");
			Prefabs.smallWormBody = fxBundle.LoadAsset<GameObject>("Assets/AkisExtraTwitchEvents/worm/SmallWormBodyPrefab.prefab");
			Prefabs.smallWormButt = fxBundle.LoadAsset<GameObject>("Assets/AkisExtraTwitchEvents/worm/SmallWormButtPrefab.prefab");

			Prefabs.magicalFloxFx = bundle.LoadAsset<GameObject>("Assets/TwitchIntegration/MagicalFloxOverlay Variant.prefab");
			LoadOilDripFx(bundle);

			LoadFallingStuffOverlay(fxBundle);
		}

		private static void LoadOilDripFx(AssetBundle bundle)
		{
			Prefabs.oilDripFx = bundle.LoadAsset<GameObject>("Assets/TwitchIntegration/OilDrip.prefab");

			Prefabs.oilDripFx.GetComponent<ParticleSystemRenderer>().material = new Material(Shader.Find("TextMeshPro/Sprite"))
			{
				mainTexture = bundle.LoadAsset<Texture2D>("Assets/TwitchIntegration/status_item_plant_liquid.png"),
				renderQueue = 4500
			};

			Prefabs.oilDripFx.transform.Find("Sparkles").GetComponent<ParticleSystemRenderer>().material = new Material(Shader.Find("TextMeshPro/Sprite"))
			{
				mainTexture = bundle.LoadAsset<Texture2D>("Assets/TwitchIntegration/star.png"),
				renderQueue = 4500
			};
		}

		private static void LoadFallingStuffOverlay(AssetBundle fxBundle)
		{
			Prefabs.fallingStuffOverlay = fxBundle.LoadAsset<GameObject>("Assets/AkisExtraTwitchEvents/FallingStuffOverlay.prefab");
		}

		// this only works for VALUES for TMP tags. tag names use a hash with << 3 instead usually
		// i also technically skip some checks and rely on myself not using special characters
		private static int StringToTMPHash(string str)
		{
			var hash = 0;
			var chars = str.ToCharArray();

			for (int i = 0; i < chars.Length && chars[i] != 0 && chars[i] != 60; ++i)
			{
				if (chars[i] != 34)
				{
					hash = (hash << 5) + hash ^ chars[i];
				}
			}

			return hash;
		}

		private static void TmpExtensions()
		{
			var orange = Util.ColorFromHex("FF6600");
			var lime = Util.ColorFromHex("B4FF04");

			MaterialReferenceManager.AddColorGradientPreset(
				StringToTMPHash(TMP_GradientPresetIDs.SUPER_DUPE),
				new TMP_ColorGradient(
					orange, orange,
					lime, lime));
		}

		private static void LoadDeathLaser(AssetBundle fxBundle)
		{
			Prefabs.deathLaser = fxBundle.LoadAsset<GameObject>("Assets/AkisExtraTwitchEvents/laser/DeathLaser.prefab");
			var laserTex = fxBundle.LoadAsset<Texture2D>("Assets/AkisExtraTwitchEvents/laser/laser.png");
			var material = fxBundle.LoadAsset<Material>("Assets/AkisExtraTwitchEvents/laser/DeathLaserMaterial_smallbeam.mat");
			material.mainTexture = laserTex;
		}

		private static void LoadSimpleOverlay(AssetBundle bundle)
		{
			Prefabs.overlayQuad = bundle.LoadAsset<GameObject>("Assets/TwitchIntegration/Shaders/SimpleOverlay.prefab");
		}

		private static void LoadSolarStorm(AssetBundle bundle)
		{
			Prefabs.solarStormQuad = bundle.LoadAsset<GameObject>("Assets/TwitchIntegration/SolarStormOverlay.prefab");
		}

		private static void LoadFireOverlay(AssetBundle bundle)
		{
			Prefabs.fireOverlay = bundle.LoadAsset<GameObject>("FireOverlay");
		}

		private static void LoadSpinnyWheel(AssetBundle bundle)
		{
			var prefab = bundle.LoadAsset<GameObject>("Assets/TwitchIntegration/Wheel_tmpconverted.prefab");
			//TMPConverter.ReplaceAllText(prefab);

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
			//TMPConverter.ReplaceAllText(Prefabs.adventureScreen);

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
				var shape = particles.shape;
				shape.scale = new Vector3(radius, radius, 1f);

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
			var gameObject = new GameObject("text");

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
