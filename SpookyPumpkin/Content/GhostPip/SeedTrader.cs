using FUtility;
using KSerialization;
using SpookyPumpkinSO.Content.Plants;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpookyPumpkinSO.Content.GhostPip
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class SeedTrader : StateMachineComponent<SeedTrader.SMInstance>, ISim4000ms
	{
		[Serialize] public bool TreatRequested;
		[Serialize] public bool IsConsumed = true;
		[Serialize] public Tag treatTag = GrilledPrickleFruitConfig.ID;

		[MyCmpGet] public ManualDeliveryKG delivery;
		[MyCmpGet] private Storage storage;
		[MyCmpReq] private KSelectable kSelectable;

		public Tag defaultTag = GrilledPrickleFruitConfig.ID;
		private bool storage_recursion_guard;
		public HashSet<Tag> possibleTreats;
		private bool queueReroll = false;

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			GetPossiblePipTreats();
		}

		private void GetPossiblePipTreats()
		{
			possibleTreats = new HashSet<Tag>();

			foreach (var treat in ModAssets.ReadPipTreats())
			{
				var item = Assets.TryGetPrefab(treat);

				if (item != null && item.TryGetComponent(out Pickupable _))
					possibleTreats.Add(treat);
			}

			if (possibleTreats.Count == 0)
				possibleTreats.Add(defaultTag);

			// if a prefab was uninstalled, or option was removed
			if (!possibleTreats.Contains(treatTag))
				RollNewTreat();
		}

		public override void OnSpawn()
		{
			base.OnSpawn();

			Subscribe((int)GameHashes.OnStorageChange, OnStorageChange);
			GameClock.Instance.Subscribe((int)GameHashes.NewDay, OnNewDay);

			smi.StartSM();

			RefreshTreatChore();
			RefreshConsumedState();
		}

		private void OnNewDay(object obj)
		{
			if (!TreatRequested)
				queueReroll = true;
		}

		public void RollNewTreat()
		{
			queueReroll = false;
			treatTag = RollForAvailable(16);
			delivery.RequestedItemTag = treatTag;
			RefreshSideScreen();
		}

		// tries several times to find something the user actually has
		private Tag RollForAvailable(int tries)
		{
			var worldInventory = ClusterManager.Instance.GetWorld(gameObject.GetMyWorldId()).worldInventory;
			for (var i = 0; i < tries - 1; i++)
			{
				var result = RollUnique();
				if (worldInventory.GetAmount(result, false) > 0)
				{
					return result;
				}
			}

			return RollUnique();
		}

		private Tag RollUnique()
		{
			if (possibleTreats == null || possibleTreats.Count == 0)
			{
				return defaultTag;
			}

			if (possibleTreats.Count == 1)
			{
				return possibleTreats.First();
			}

			var result = treatTag;
			var attempt = 0;
			while (result == treatTag && attempt++ < 100)
			{
				var index = Random.Range(0, possibleTreats.Count - 1);
				result = possibleTreats.ElementAt(index);
			}

			return result;
		}

		public void RefreshSideScreen()
		{
			if (kSelectable.IsSelected)
				DetailsScreen.Instance.Refresh(gameObject);
		}

		public void RequestTreat(bool request)
		{
			TreatRequested = request;
			RefreshTreatChore();
		}

		private void Treat()
		{
			SetConsumed(false);
			RequestTreat(false);
			RefreshTreatChore();
			RefreshSideScreen();
		}

		private void SetConsumed(bool consumed)
		{
			IsConsumed = consumed;
			RefreshConsumedState();
		}

		private void RefreshConsumedState()
		{
			smi.sm.IsFed.Set(!IsConsumed, smi);
		}

		private void RefreshTreatChore()
		{
			delivery.Pause(!TreatRequested, "No treat requested");
		}

		private void OnStorageChange(object data)
		{
			if (storage_recursion_guard)
				return;

			storage_recursion_guard = true;

			if (IsConsumed)
			{
				var treat = storage.FindFirst(treatTag);
				if (treat != null)
				{
					storage.ConsumeIgnoringDisease(treat);
					Treat();
				}
			}

			storage_recursion_guard = false;
		}

		public void Sim4000ms(float dt)
		{
			if (queueReroll && IsDeliveryPaused())
				RollNewTreat();
		}

		private bool IsDeliveryPaused() => delivery.IsPaused;

		public class States : GameStateMachine<States, SMInstance, SeedTrader>
		{
			public State idle;
			public TradingStates trading;
			public BoolParameter IsFed;
			public BoolParameter Dice;

			public class TradingStates : State
			{
				public State pre;
				public State giveseed;
				public State complete;
				public State pst;
			}

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = idle;

				idle
					.Enter(smi => smi.SetupNextTrade())
					.ParamTransition(IsFed, trading.pre, IsTrue);

				trading.pre
					.Enter(smi => smi.AddMouthOverride("sq_mouth_cheeks"))
					.ScheduleGoTo(0.4f, trading.giveseed)
					.Exit(smi => smi.RemoveMouthOverride());

				trading.giveseed
					.Enter(smi => smi.SpawnSeed())
					.ParamTransition(Dice, trading.pst, IsTrue)
					.GoTo(trading.pre);

				trading.pst
					.PlayAnim("growup_pst")
					.Enter(smi => smi.master.SetConsumed(true))
					.GoTo(idle);
			}
		}

		public class SMInstance : GameStateMachine<States, SMInstance, SeedTrader, object>.GameInstance
		{
			private Vector3 seedOffset = new Vector3(0, 1);
			private int seedCount = 0;

			public SMInstance(SeedTrader master) : base(master) { }

			public void SetupNextTrade()
			{
				if (!smi.master.IsDeliveryPaused())
					smi.master.RollNewTreat();

				seedCount = Random.Range(1, 4);
				smi.sm.Dice.Set(false, smi);
			}

			public void AddMouthOverride(string anim)
			{
				var component = master.GetComponent<SymbolOverrideController>();
				var symbol = master.GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbol(anim);

				if (symbol != null)
					component.AddSymbolOverride("sq_mouth", symbol);
			}

			public void RemoveMouthOverride()
			{
				master.GetComponent<SymbolOverrideController>().TryRemoveSymbolOverride("sq_mouth");
			}

			public void SpawnSeed()
			{
				var seed = Utils.Spawn(PumpkinPlantConfig.SEED_ID, transform.position + seedOffset, Grid.SceneLayer.Ore);
				Utils.YeetRandomly(seed, true, 2, 4, true);

				PlaySound(GlobalAssets.GetSound("squirrel_plant_barf"));
				PopFXManager.Instance.SpawnFX(
					PopFXManager.Instance.sprite_Resource,
					STRINGS.CREATURES.SPECIES.SEEDS.SP_PUMPKIN.NAME,
					transform,
					Vector3.zero);

				if (seedCount-- <= 0)
					smi.sm.Dice.Set(true, smi);
			}
		}
	}
}
