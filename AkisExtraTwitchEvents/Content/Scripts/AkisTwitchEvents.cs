using FUtility;
using HarmonyLib;
using KSerialization;
using ONITwitchLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using Twitchery.Content.Events;
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

		public bool hotTubActive;

		public float originalLiquidTransparency;
		public bool hideLiquids;
		public bool eggActive;
		public AETE_EggPostFx eggFx;

		public static ONITwitchLib.EventInfo polymorphEvent;
		public static MinionIdentity polymorphTarget;
		public static string polyTargetName;
#if WIP_EVENTS
		public static ONITwitchLib.EventInfo encouragePipEvent;
		public static RegularPip regularPipTarget;
		public static string regularPipTargetName;
#endif

		public static string pizzaRecipeID;
		public static string radDishRecipeID;
		public static string frozenHoneyRecipeID;

		public static Danger maxDanger = Danger.None;

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
#if WIP_EVENTS
			RegularPip.regularPipCache.Clear();
#endif
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			Instance = null;
		}

		public void OnDraw()
		{
			UpdatePolymorphTarget();
			UpdateEncouragePipTarget();
		}

		private static void UpdateEncouragePipTarget()
		{
#if WIP_EVENTS
			regularPipTarget = GetUpgradeablePip();
#endif
		}

		private static void UpdatePolymorphTarget()
		{
			polymorphTarget = Components.LiveMinionIdentities.GetRandom();

			if (polymorphTarget != null)
				polyTargetName = polymorphEvent.FriendlyName = STRINGS.AETE_EVENTS.POLYMOPRH.TOAST.Replace("{Name}", Util.StripTextFormatting(polymorphTarget.GetProperName()));
		}
#if WIP_EVENTS
		public static bool HasUpgradeablePip() => regularPipTarget != null;

		public static RegularPip GetUpgradeablePip()
		{
			if (Mod.regularPips.Count > 0)
			{
				var potentialPips = new List<RegularPip>();

				foreach (var pip in Mod.regularPips.items)
				{
					if (pip.potentialNextSkills != null && pip.potentialNextSkills.Count > 0)
						potentialPips.Add(pip);
				}

				if (potentialPips.Count > 0)
				{
					regularPipTarget = potentialPips.GetRandom();

					if (regularPipTarget != null)
						regularPipTargetName = encouragePipEvent.FriendlyName = STRINGS.AETE_EVENTS.ENCOURAGE_REGULAR_PIP.TOAST.Replace("{Name}", Util.StripTextFormatting(regularPipTarget.GetProperName()));

					return regularPipTarget;
				}
			}

			return null;
		}

		public static void UpdateRegularPipWeight()
		{
			encouragePipEvent?.Group.SetWeight(encouragePipEvent, Mod.regularPips.Count > 0
				? TwitchEvents.Weights.RARE
				: TwitchEvents.Weights.COMMON);
		}
#endif
	}
}
