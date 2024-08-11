using FUtility;
using UnityEngine;

namespace GoldenThrone
{
	public class ModAssets
	{
		public static Expression relievedExpression;
		public static Face relievedFace;

		public static class Colors
		{
			public static Color gold = new Color32(255, 245, 86, 255);
		}

		public static class Effects
		{
			public const string RoyallyRelievedID = "GoldenThrone_Monarch";
			public const string TooComfortable = "GoldenThrone_TooComfortable";
		}

		public static class Prefabs
		{
			public static GameObject goldSparkleParticles;
		}

		public static void LateLoadAssets()
		{
			relievedFace = new Face("ah"); // id will be looked up as an animation in the master_head_swap_kanim, "ah" is unused as an expression

			var expressions = Db.Get().Expressions;
			relievedExpression = new Expression("GoldenThrone_Relieved", expressions, relievedFace)
			{
				priority = expressions.Tired.priority - 1
			};

			if (Mod.Settings.UseParticles)
			{
				var bundle = FUtility.FAssets.LoadAssetBundle("goldenthroneassets", platformSpecific: true);
				Log.Assert("bundle", bundle);

				LoadGoldSparkleParticles(bundle);
			}
		}

		private static void LoadGoldSparkleParticles(AssetBundle bundle)
		{
			// sprite format image sliced into 4
			var texture = bundle.LoadAsset<Texture2D>("sparkle_gold");

			// particle system
			var prefab = bundle.LoadAsset<GameObject>("GoldSparkles");
			prefab.SetLayerRecursively(Game.PickupableLayer);

			var material = new Material(Shader.Find("Klei/BloomedParticleShader"));
			material.SetTexture("_MainTex", texture);
			material.SetColor("_TintColor", Colors.gold);
			material.renderQueue = RenderQueues.Liquid; // Sparkle Streaker particles also render here

			var renderer = prefab.GetComponent<ParticleSystemRenderer>();
			renderer.material = material;

			Prefabs.goldSparkleParticles = prefab;

			Log.Debuglog("Loaded prefab: " + Prefabs.goldSparkleParticles.name);
		}
	}
}
