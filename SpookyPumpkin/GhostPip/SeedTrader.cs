using FUtility;
using Harmony;
using KSerialization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpookyPumpkin.GhostPip
{
    public class SeedTrader : StateMachineComponent<SeedTrader.SMInstance>, ISim4000ms
    {
        public ManualDeliveryKG delivery;
        Storage storage;
        [Serialize]
        public bool TreatRequested;
        [Serialize]
        public bool IsConsumed = true;
        [Serialize]
        public Tag treatTag = GrilledPrickleFruitConfig.ID;
        public Tag defaultTag = GrilledPrickleFruitConfig.ID;
        private bool storage_recursion_guard;
        public HashSet<Tag> possibleTreats;
        bool queueReroll = false;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            possibleTreats = new HashSet<Tag>() { defaultTag };
            foreach (string treat in ModAssets.ReadPipTreats())
            {
                Tag treatTag = treat.ToTag();
                if (Assets.TryGetPrefab(treatTag) != null)
                    possibleTreats.Add(treatTag);
            }
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            storage = GetComponent<Storage>();
            delivery = GetComponent<ManualDeliveryKG>();

            Subscribe((int)GameHashes.OnStorageChange, OnStorageChange);
            GameClock.Instance.Subscribe((int)GameHashes.NewDay, OnNewDay);

            smi.StartSM();

            RefreshTreatChore();
            RefreshConsumedState();
        }

        private void OnNewDay(object obj) => queueReroll = true;

        public void RollNewTreat()
        {
            queueReroll = false;
            treatTag = RollUnique();
            delivery.RequestedItemTag = treatTag;
            RefreshSideScreen();
        }

        Tag RollUnique()
        {
            if (possibleTreats == null || possibleTreats.Count == 0)
                return defaultTag;

            if (possibleTreats.Count == 1)
                return possibleTreats.First();

            Tag result = treatTag;
            int attempt = 0;
            while (result == treatTag && attempt++ < 100)
            {
                int index = Random.Range(0, possibleTreats.Count - 1);
                result = possibleTreats.ElementAt(index);
            }

            return result;
        }

        public void RefreshSideScreen()
        {
            if (GetComponent<KSelectable>().IsSelected)
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

        private void RefreshConsumedState() => smi.sm.IsFed.Set(!IsConsumed, smi);
        private void RefreshTreatChore() => delivery.Pause(!TreatRequested, "No treat requested");

        private void OnStorageChange(object data)
        {
            if (storage_recursion_guard)
                return;

            storage_recursion_guard = true;

            if (IsConsumed)
            {
                var treat = storage.FindFirst(treatTag);
                if(treat != null)
                {
                    storage.ConsumeIgnoringDisease(treat);
                    Treat();
                }
            }

            storage_recursion_guard = false;
        }

        public void Sim4000ms(float dt)
        {
            if(queueReroll && IsDeliveryPaused())
                RollNewTreat();
        }

        private bool IsDeliveryPaused() => Traverse.Create(delivery).Field("paused").GetValue<bool>();

        public class States : GameStateMachine<States, SMInstance, SeedTrader>
        {
#pragma warning disable 649
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
#pragma warning restore

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
            Vector3 seedOffset = new Vector3(0, 1);
            int seedCount = 0;

            public SMInstance(SeedTrader master) : base(master) { }

            public void SetupNextTrade()
            {
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
                GameObject seed = Utils.Spawn(PumpkinPlantConfig.SEED_ID, transform.position + seedOffset, Grid.SceneLayer.Ore);
                Utils.Yeet(seed, true, 2, 4, true);
                PlaySound(GlobalAssets.GetSound("squirrel_plant_barf"));
                PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, STRINGS.CREATURES.SPECIES.SEEDS.SP_PUMPKIN.NAME, transform, Vector3.zero);

                if (seedCount-- <= 0)
                    smi.sm.Dice.Set(true, smi);
            }
        }
    }
}