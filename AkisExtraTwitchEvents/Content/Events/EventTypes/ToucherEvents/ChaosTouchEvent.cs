using FUtility;
using ONITwitchLib;
using System.Collections.Generic;
using System.Linq;
using Twitchery.Content.Scripts.Touchers;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class ChaosTouchEvent() : TwitchEventBase("ChaosTouch")
	{
		public override bool Condition()
		{
			if (targetElements == null)
				SetElements();

			return targetElements != null && targetElements.Count > 0;
		}

		public override Danger GetDanger() => Danger.Medium;

		public override int GetWeight() => WEIGHTS.COMMON;

		public static List<SimHashes> targetElements;

		public static void SetElements()
		{
			if (ElementLoader.elements == null)
				return;


			var danger = (Danger)(long)ONITwitchLib.Core.TwitchSettings.GetSettingsDictionary()["MaxDanger"];

			Log.Debug(danger);

			if (Random.value < 0.66f)
				danger = (Danger)Mathf.Clamp((int)danger, 0, 4);

			targetElements = new List<SimHashes>
			{
				SimHashes.Algae,
				SimHashes.Aluminum,
				SimHashes.AluminumOre,
				SimHashes.Brine,
				SimHashes.Carbon,
				SimHashes.Ceramic,
				SimHashes.Clay,
				SimHashes.Copper,
				SimHashes.Cuprite,
				SimHashes.Diamond,
				SimHashes.Dirt,
				SimHashes.Electrum,
				SimHashes.Ethanol,
				SimHashes.Fertilizer,
				SimHashes.FoolsGold,
				SimHashes.Fossil,
				SimHashes.Glass,
				SimHashes.GoldAmalgam,
				SimHashes.Granite,
				SimHashes.Graphite,
				SimHashes.HardPolypropylene,
				SimHashes.Hydrogen,
				SimHashes.IgneousRock,
				SimHashes.Iron,
				SimHashes.IronOre,
				SimHashes.Isoresin,
				SimHashes.Katairite,
				SimHashes.Lead,
				SimHashes.Lime,
				SimHashes.MaficRock,
				SimHashes.Methane,
				SimHashes.Milk,
				SimHashes.MilkFat,
				SimHashes.Sand,
				SimHashes.Naphtha,
				SimHashes.Niobium,
				SimHashes.Obsidian,
				SimHashes.Oxygen,
				SimHashes.OxyRock,
				SimHashes.Petroleum,
				SimHashes.Phosphorite,
				SimHashes.Phosphorus,
				SimHashes.Polypropylene,
				SimHashes.Regolith,
				SimHashes.Resin,
				SimHashes.Rust,
				SimHashes.Salt,
				SimHashes.SaltWater,
				SimHashes.Sand,
				SimHashes.SandStone,
				SimHashes.SedimentaryRock,
				SimHashes.Sulfur,
				SimHashes.SuperCoolant,
				SimHashes.Tungsten,
				SimHashes.ViscoGel,
				SimHashes.Water,
				SimHashes.Wolframite
			};

			if (DlcManager.IsExpansion1Active())
			{
				targetElements.Add(SimHashes.Cobalt);
				targetElements.Add(SimHashes.Cobaltite);
				targetElements.Add(SimHashes.Mud);
				targetElements.Add(SimHashes.Sucrose);
			}

			if (danger >= Danger.Medium)
			{
				targetElements.Add(SimHashes.DirtyIce);
				targetElements.Add(SimHashes.Ice);
				targetElements.Add(SimHashes.BrineIce);
				targetElements.Add(SimHashes.CrushedIce);
				targetElements.Add(SimHashes.MilkIce);
				targetElements.Add(SimHashes.CrudeOil);
				targetElements.Add(SimHashes.LiquidSulfur);
				targetElements.Add(SimHashes.LiquidOxygen);
			}

			if (danger <= Danger.High && DlcManager.IsExpansion1Active())
				targetElements.Add(SimHashes.NuclearWaste);

			if (danger >= Danger.Deadly)
			{
				targetElements.Add(SimHashes.LiquidOxygen);
				targetElements.Add(SimHashes.LiquidHydrogen);
				//targetElements.Add(SimHashes.Magma);
				//targetElements.Add(SimHashes.MoltenGold);
				//targetElements.Add(SimHashes.MoltenGlass);
			}
		}

		public override void Run()
		{
			if (targetElements == null || targetElements.Count == 0)
				SetElements();

			var go = new GameObject("ChaosToucher");
			var toucher = go.AddComponent<ChaosToucher>();
			toucher.lifeTime = ModTuning.MIDAS_TOUCH_DURATION;
			toucher.radius = 3f;
			toucher.cellsPerUpdate = 4;

			go.SetActive(true);

			toucher.Roll();

			ToastManager.InstantiateToast(
				"",
				"Everything I Touch is... ????");
		}

		private static bool IsAllowed(Element element)
		{
			return element.hardness < byte.MaxValue
				&& element.state > Element.State.Vacuum
				&& element.state < Element.State.Unbreakable
				&& !element.disabled
				&& element.id != SimHashes.Creature
				&& IsSubstanceConfigured(element)
				&& !element.HasTag(TTags.disableChaosToucherTarget);
		}

		private static bool IsSubstanceConfigured(Element element)
		{
			if (element.substance == null)
				return false;

			if (element.oreTags == null || !element.oreTags.Contains(GameTags.Unstable))
				return true;

			var isUnstableConfigured = World.Instance.TryGetComponent(out UnstableGroundManager manager) && manager.runtimeInfo.ContainsKey(element.id);

			return isUnstableConfigured;
		}
	}
}
