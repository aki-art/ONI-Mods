using FUtility;
using HarmonyLib;
using KSerialization;
using ONITwitchLib;
using System;
using System.Reflection;
using Twitchery.Patches;

namespace Twitchery.Content.Scripts
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class AkisTwitchEvents : KMonoBehaviour
	{
		public static AkisTwitchEvents Instance;

		[Serialize] public float lastLongBoiSpawn;
		[Serialize] public float lastRadishSpawn;
		[Serialize] internal bool hasRaddishSpawnedBefore;
		[Serialize] public bool hasUnlockedPizzaRecipe;

		public float originalLiquidTransparency;
		public bool hideLiquids;
		public bool eggActive;
		public AETE_EggPostFx eggFx;

		public static ONITwitchLib.EventInfo polymorph;
		public static MinionIdentity polymorphTarget;
		public static string polyTargetName;

		public static string pizzaRecipeID;
		public static string radDishRecipeID;
		public static string frozenHoneyRecipeID;

		public static Danger maxDanger = Danger.None;

		[Serialize] public bool migrated1_7_0;

		public void ApplyLiquidTransparency(WaterCubes waterCubes)
		{
			if (originalLiquidTransparency == 0)
				originalLiquidTransparency = waterCubes.material.GetFloat("_BlendScreen");

			waterCubes.material.SetFloat("_BlendScreen", hideLiquids ? 0 : originalLiquidTransparency);
		}

		public AkisTwitchEvents()
		{
			lastLongBoiSpawn = float.NegativeInfinity;
			lastRadishSpawn = 0;
		}

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			Instance = this;
		}


		public static void OnWorldLoaded()
		{
			var type = Type.GetType("ONITwitch.Settings.GenericModSettings, ONITwitch");

			if (type != null)
			{
				var data = type.GetField("data", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);

				if (data != null)
				{
					var danger = data.GetType().GetField("MaxDanger").GetValue(data);
					maxDanger = (Danger)(int)danger;
				}
			}
		}

		public override void OnSpawn()
		{
			base.OnSpawn();
			if (!migrated1_7_0)
			{
				Log.Debuglog("migrating");

				GameScheduler.Instance.ScheduleNextFrame("migrate hulk strength", _ =>
				{
					foreach (var angry in FindObjectsOfType<AngryTrait>())
					{
						angry.MigrateStrengthStat();
						angry.MigrateHealth();
					}
				});

				migrated1_7_0 = true;
			}
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			Instance = null;
		}

		public void OnDraw()
		{
			polymorphTarget = Components.LiveMinionIdentities.GetRandom();
			polyTargetName = polymorph.FriendlyName = STRINGS.AETE_EVENTS.POLYMOPRH.TOAST.Replace("{Name}", Util.StripTextFormatting(polymorphTarget.GetProperName()));
		}
	}
}
