/*using KSerialization;
using System;
using System.Collections;
using UnityEngine;

namespace SpookyPumpkin
{
    public class SeedTrader : StateMachineComponent<SeedTrader.SMInstance>
    {
        [Serialize]
        public bool deliveryRequested;
        [MyCmpReq] Storage storage;
        private Chore fetchChore;
        public Tag treatTag = GrilledPrickleFruitConfig.ID;

        protected override void OnSpawn()
        {
            base.OnSpawn();

            if (deliveryRequested && fetchChore == null)
                CreateFetchChore();

            smi.StartSM();
        }

        public void InitiateTrade()
        {
            smi.sm.deliverySelected.Set(true, smi);
        }

        protected void CreateFetchChore()
        {
            if (fetchChore != null)
                return;

            float rmnQuantity = 1f - storage.GetAmountAvailable(treatTag);

            fetchChore = new FetchChore(
                choreType: Db.Get().ChoreTypes.Fetch,
                destination: storage,
                amount: rmnQuantity,
                tags: new Tag[]
                {
                    treatTag
                },
                on_complete: (obj) => deliveryRequested = false,
                on_begin: null,
                on_end: null,
                operational_requirement: FetchOrder2.OperationalRequirement.Functional);

            MaterialNeeds.Instance.UpdateNeed(treatTag, rmnQuantity);
            deliveryRequested = true;
        }

        public void CancelActiveRequest()
        {
            if (fetchChore != null)
            {
                MaterialNeeds.Instance.UpdateNeed(treatTag, -1f);
                fetchChore.Cancel("User canceled");
                fetchChore = null;
            }

            deliveryRequested = false;
            Trigger((int)GameHashes.UIRefreshData, this);
        }

        private void EmptyStorage()
        {
            foreach (var item in storage.items)
                Util.KDestroyGameObject(item);
        }

        private bool HasFruit => storage.GetAmountAvailable(treatTag) >= 1f;

        public class States : GameStateMachine<States, SMInstance, SeedTrader>
        {
            public State idle;
            public State waiting;
            public State receivedFruit;
            public State receivedFruitPre;
            public State receivedFruitPst;

            public BoolParameter deliverySelected;

            public override void InitializeStates(out BaseState default_state)
            {
                default_state = idle;
                idle
                    .ParamTransition(deliverySelected, waiting, IsTrue);
                waiting
                    .Enter(smi => smi.master.CreateFetchChore())
                    .ParamTransition(deliverySelected, idle, IsFalse)
                    .EventTransition(GameHashes.OnStorageChange, receivedFruitPre, smi => smi.master.HasFruit)
                    .Exit(smi => smi.master.CancelActiveRequest());
                receivedFruitPre
                    .Enter(smi => smi.AddMouthOverride("sq_mouth_cheeks"))
                    .ScheduleGoTo(1f, receivedFruit);
                receivedFruit
                    .Enter(smi => smi.GiveSeed());
                receivedFruitPst
                    .PlayAnim("growup_pst")
                    .GoTo(idle);
            }
        }

        internal void ToggleFetch()
        {
            Debug.Log("toggling fetches from " + deliveryRequested);
            if (!deliveryRequested)
                InitiateTrade();
            else
                CancelActiveRequest();
            Debug.Log("to " + deliveryRequested);
        }

        public class SMInstance : GameStateMachine<States, SMInstance, SeedTrader, object>.GameInstance
        {
            public SMInstance(SeedTrader master) : base(master) { }

            public void RemoveMouthOverride() => master.GetComponent<SymbolOverrideController>().TryRemoveSymbolOverride("sq_mouth");

            public void AddMouthOverride(string anim)
            {
                SymbolOverrideController component = master.GetComponent<SymbolOverrideController>();
                KAnim.Build.Symbol symbol = master.GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbol(anim);
                if (symbol == null)
                    return;
                component.AddSymbolOverride("sq_mouth", symbol);
            }

            internal void GiveSeed()
            {
                RemoveMouthOverride();
                master.EmptyStorage();

                master.StartCoroutine(SpawnSeeds(UnityEngine.Random.Range(1, 3)));

            }

            private void SpawnSeed()
            {
                var seed = GameUtil.KInstantiate(Assets.GetPrefab(PumpkinPlantConfig.SEED_ID), transform.position, Grid.SceneLayer.Ore);
                seed.SetActive(true);
                PlaySound(GlobalAssets.GetSound("squirrel_plant_barf"));

                var vec = UnityEngine.Random.insideUnitCircle.normalized;
                vec.y = Mathf.Abs(vec.y);
                vec += new Vector2(0f, UnityEngine.Random.Range(0, 1f));
                vec *= UnityEngine.Random.Range(2, 4);

                if (GameComps.Fallers.Has(seed))
                    GameComps.Fallers.Remove(seed);

                GameComps.Fallers.Add(seed, vec);
                Trigger((int)GameHashes.UIRefreshData, this);
            }

            IEnumerator SpawnSeeds(int amount)
            {
                var count = 0;
                while (count++ < amount)
                {
                    SpawnSeed();
                    yield return new WaitForSeconds(.3f);
                }

                smi.GoTo(sm.receivedFruitPst);
            }
        }
    }
}
*/