using FUtility;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Twitchery
{
	public class ModAssets
	{
		private const float SPARKLE_DENSITY = 1000f / 5f;

		public static class Fx
		{
			public static SpawnFXHashes pinkPoof = (SpawnFXHashes)Hash.SDBMLower("AETE_PinkPoof");
			public static SpawnFXHashes fungusPoof = (SpawnFXHashes)Hash.SDBMLower("AETE_FungusPoof");
		}

		public static class Colors
		{
			public static Color midasGold = new Color (1f, 1f, 0.4f) * 5f;
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
				GOLD = "aete_gold";
		}

		public static class Materials
		{
			public static Material pixelShaderMaterial;
			public static Material ditherMaterial;
			public static Material eggMaterial;
		}

		public static class Prefabs
		{
			public static GameObject sparkleCircle;
		}

		public static float GetSFXVolume() => KPlayerPrefs.GetFloat("Volume_SFX") * KPlayerPrefs.GetFloat("Volume_Master");

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

			var bundle = FUtility.Assets.LoadAssetBundle("akis_twitch_events", platformSpecific: true);

			foreach (var asset in bundle.GetAllAssetNames())
			{
				Log.Debuglog(asset);
			}

			Materials.pixelShaderMaterial = bundle.LoadAsset<Material>("Assets/Scripts/Shader Tests/Pixel Art Shader/PixelShaderMat.mat");
			var ditherShader = bundle.LoadAsset<Shader>("Assets/Scripts/Shader Tests/Acerola Pixel Shader/Dither.shader");
			Materials.ditherMaterial = new Material(ditherShader);

			var eggShader = bundle.LoadAsset<Shader>("Assets/Sparkles/egg/EggShader.shader");
			Materials.eggMaterial = bundle.LoadAsset<Material>("Assets/Sparkles/egg/EggMaterial.mat");
			Materials.eggMaterial.shader = eggShader;

			LoadParticles(bundle);
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
		}

		public static LineRenderer DrawLine(Vector3 from,  Vector3 to, Color color)
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
