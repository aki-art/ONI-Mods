using KSerialization;
using ONITwitchLib;
using PeterHan.PLib.Core;
using System.Collections.Generic;
using Twitchery.Content.Events;

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

		public static System.Action OnDrawFn;

		private bool isHotTubActive;
		public bool HotTubActive
		{
			get => isHotTubActive;
			set
			{
				if (value)
					CreateOrEnableFireOverlay();

				if (fireOverlay != null)
					fireOverlay.OnHotTubToggled(value);

				isHotTubActive = value;
			}
		}

		public float originalLiquidTransparency;
		public bool hideLiquids;
		public bool eggActive;
		public AETE_EggPostFx eggFx;

		public static ONITwitchLib.EventInfo polymorphEvent;
		public static MinionIdentity polymorphTarget;
		public static string polyTargetName;

		public static ONITwitchLib.EventInfo encouragePipEvent;
		public static RegularPip regularPipTarget;
		public static string regularPipTargetName;

		private FireOverlay fireOverlay;

		public class TargetingEvent<T>
		{
			public ONITwitchLib.EventInfo eventInfo;
			public T target;
			public string minionName;
		}

		public static string pizzaRecipeID;
		public static string radDishRecipeID;
		public static string frozenHoneyRecipeID;

		public static Danger MaxDanger
		{
			get
			{
				var dict = ONITwitchLib.Core.TwitchSettings.GetSettingsDictionary();

				if (dict != null && ONITwitchLib.Core.TwitchSettings.GetSettingsDictionary().TryGetValue("MaxDanger", out var result))
				{
					//return (Danger)System.Convert.ToInt32(result);
					return (Danger)(long)result;
				}

				return Danger.High;
			}
		}

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

		public override void OnSpawn()
		{
			base.OnSpawn();
			OnDraw();
		}

		public static void OnWorldLoaded()
		{
			RegularPip.regularPipCache.Clear();
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
			TwitchEvents.OnDraw();
		}

		private static void UpdateEncouragePipTarget()
		{
			regularPipTarget = GetUpgradeablePip();
		}

		private static void UpdatePolymorphTarget()
		{
			/*			polymorphTarget = Components.LiveMinionIdentities.GetRandom();

						if (polymorphTarget != null)
							polyTargetName = polymorphEvent.FriendlyName = STRINGS.AETE_EVENTS.POLYMOPRH.TOAST.Replace("{Name}", Util.StripTextFormatting(polymorphTarget.GetProperName()));*/
		}

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
			/*			encouragePipEvent?.Group.SetWeight(encouragePipEvent, Mod.regularPips.Count > 0
							? TwitchEvents.Weights.RARE
							: TwitchEvents.Weights.COMMON);*/
		}

		public void CreateOrEnableFireOverlay()
		{
			/*			if (!hotTubActive)
							return;*/
			if (fireOverlay == null)
			{
				var ui = Instantiate(ModAssets.Prefabs.fireOverlay);
				ui.SetParent(GameScreenManager.Instance.ssOverlayCanvas);
				ui.gameObject.SetActive(false);

				fireOverlay = ui.AddComponent<FireOverlay>();
			}

			if (!isHotTubActive)
				return;

			fireOverlay.gameObject.SetActive(true);
			fireOverlay.Play();
		}

		public void HideFireOverlay()
		{
			if (fireOverlay != null && fireOverlay.isActiveAndEnabled)
			{
				fireOverlay.Stop();
				fireOverlay.gameObject.SetActive(false);
			}
		}
	}
}
