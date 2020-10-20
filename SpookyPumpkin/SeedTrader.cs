using KSerialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SpookyPumpkin
{
    class SeedTrader : StateMachineComponent<SeedTrader.SMInstance>, ISidescreenButtonControl
    {
        [Serialize]
        public bool IsConsumed;
        [Serialize]
        bool deliverSeed = false;
        Pickupable pumpkinSeed;
        [MyCmpReq] Storage storage;
        private Chore fetchChore;
        Tag requestedEntityTag;

        protected override void OnSpawn()
        {
            base.OnSpawn();
            smi.StartSM();
        }

        protected void CreateFetchChore(Tag entityTag)
        {
            if (fetchChore != null || !entityTag.IsValid)
                return;

            float rmnQuantity = 1f - storage.GetAmountAvailable(requestedEntityTag);
            requestedEntityTag = entityTag;

            fetchChore = new FetchChore (
                choreType: Db.Get().ChoreTypes.Fetch,
                destination: storage,
                amount: rmnQuantity,
                tags: new Tag[]
                {
                    entityTag
                }, 
                on_complete: null,
                on_begin: null,
                on_end: null, 
                operational_requirement: FetchOrder2.OperationalRequirement.Functional);

            MaterialNeeds.Instance.UpdateNeed(entityTag, rmnQuantity);
        }

        public void CancelActiveRequest()
        {
            if (fetchChore != null)
            {
                MaterialNeeds.Instance.UpdateNeed(requestedEntityTag, -1f);
                fetchChore.Cancel("User canceled");
                fetchChore = null;
            }

            requestedEntityTag = Tag.Invalid;
        }

        private bool HasFruit => storage.GetAmountAvailable(requestedEntityTag) >= 1f;

        public class States : GameStateMachine<States, SMInstance, SeedTrader>
        {
            public State idle;
            public State waiting;
            public State receivedFruit;

            public BoolParameter deliverySelected;

            public override void InitializeStates(out BaseState default_state)
            {
                default_state = idle;
                idle
                    .Enter(smi => smi.GivePumpkinSeed())
                    .ParamTransition(deliverySelected, waiting, IsTrue);
                waiting
                    .Enter(smi => smi.master.CreateFetchChore(GrilledPrickleFruitConfig.ID))
                    .ParamTransition(deliverySelected, idle, IsFalse)
                    .EventTransition(GameHashes.OnStorageChange, receivedFruit, smi => smi.master.HasFruit);
                receivedFruit
                    .Enter(smi => smi.GiveSeed())
                    .PlayAnim("growup_pst");
            }
        }


        public class SMInstance : GameStateMachine<States, SMInstance, SeedTrader, object>.GameInstance
        {
            public SMInstance(SeedTrader master) : base(master) { }

            public void GivePumpkinSeed()
            {
                AddMouthOverride();

                Storage storage = smi.GetComponent<Storage>();

                if (storage.GetUnitsAvailable(PumpkinPlantConfig.SEED_ID) <= 0)
                {
                    var seedPrefab = Assets.GetPrefab(PumpkinPlantConfig.SEED_ID);
                    var seed = Util.KInstantiate(seedPrefab, master.transform.position);
                    seed.SetActive(true);

                    master.pumpkinSeed = seed.GetComponent<Pickupable>();
                    smi.GetComponent<Storage>().Store(seed);
                    smi.GetComponent<Storage>().showInUI = true;
                }
            }

            public void RemoveMouthOverride() => master.GetComponent<SymbolOverrideController>().TryRemoveSymbolOverride("sq_mouth");


            public void AddMouthOverride()
            {
                SymbolOverrideController component = master.GetComponent<SymbolOverrideController>();
                KAnim.Build.Symbol symbol = master.GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbol("sq_mouth_cheeks");
                if (symbol == null)
                    return;
                component.AddSymbolOverride("sq_mouth", symbol);
            }

            internal void GiveSeed()
            {
                RemoveMouthOverride();
                master.storage.Drop(master.requestedEntityTag);
            }
        }

        public string SidescreenTitleKey => "seed";

        public string SidescreenStatusMessage => "It seems this Pip would love a treat!";

        public string SidescreenButtonText => deliverSeed ? "Cancel" : "Deliver a Fruit";

        public void OnSidescreenButtonPressed()
        {
            deliverSeed = !deliverSeed;
            smi.sm.deliverySelected.Set(deliverSeed, smi);
            if(!deliverSeed)
            {
                CancelActiveRequest();
            }
        }
    }
}
