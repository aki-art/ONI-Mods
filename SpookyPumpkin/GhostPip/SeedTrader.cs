using KSerialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SpookyPumpkin.GhostPip
{
    class SeedTrader : StateMachineComponent<SeedTrader.SMInstance>
    {
        public ManualDeliveryKG delivery;
        Storage storage;
        [Serialize]
        public bool TreatRequested;
        [Serialize]
        public bool IsConsumed = true;
        private static readonly Tag treatTag = GrilledPrickleFruitConfig.ID; 
        private bool storage_recursion_guard;

        protected override void OnSpawn()
        {
            base.OnSpawn();
            storage = GetComponent<Storage>();
            delivery = GetComponent<ManualDeliveryKG>();
            Subscribe((int)GameHashes.OnStorageChange, OnStorageChange);

            smi.StartSM();
            RefreshTreatChore();
            RefreshConsumedState();
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
                seedCount = UnityEngine.Random.Range(1, 4);
                smi.sm.Dice.Set(false, smi);
            }

            public void AddMouthOverride(string anim)
            {
                var component = master.GetComponent<SymbolOverrideController>();
                var symbol = master.GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbol(anim);
                if (symbol != null)
                    component.AddSymbolOverride("sq_mouth", symbol);
            }

            public void RemoveMouthOverride() => master.GetComponent<SymbolOverrideController>().TryRemoveSymbolOverride("sq_mouth");

            public void SpawnSeed()
            {
                var seed = GameUtil.KInstantiate(Assets.GetPrefab(PumpkinPlantConfig.SEED_ID), transform.position + seedOffset, Grid.SceneLayer.Ore);
                seed.SetActive(true);
                PlaySound(GlobalAssets.GetSound("squirrel_plant_barf"));

                var vec = UnityEngine.Random.insideUnitCircle.normalized;
                vec.y = Mathf.Abs(vec.y);
                vec += new Vector2(0f, UnityEngine.Random.Range(0, 1f));
                vec *= UnityEngine.Random.Range(2, 4);

                if (GameComps.Fallers.Has(seed))
                    GameComps.Fallers.Remove(seed);

                GameComps.Fallers.Add(seed, vec);
                PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, STRINGS.CREATURES.SPECIES.SEEDS.SP_PUMPKIN.NAME, transform, Vector3.zero);

                if (seedCount-- <= 0)
                    smi.sm.Dice.Set(true, smi);
            }
        }
    }
}