using Harmony;
using Klei.AI;
using KSerialization;
using System;
using UnityEngine;

namespace TransparentAluminium
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class UpgradeableSolarPanel : Generator, ISaveLoadable, ISidescreenButtonControl
    {
        [SerializeField]
        public float wattPerLux;

        [SerializeField]
        public float maxWatts;

        [SerializeField]
        public float multiplier = 0.5f;

        [SerializeField]
        public int maxLevels = 5;

        [SerializeField]
        public Tag targetUpgrade;

        public new float WattageRating => 333;

        public new float BaseWattageRating => 333;

        AttributeInstance powerAttribute
        {
            get => Traverse.Create(this).Field("generatorOutputAttribute").GetValue<AttributeInstance>();
            set => Traverse.Create(this).Field("generatorOutputAttribute").SetValue(value);
        }

        [Serialize]
        public int CurrentLevel { get; set; } = 1;
        public float WattsPerLux => Mathf.Pow(multiplier, (float)CurrentLevel) * wattPerLux;
        public float MaxWatt => Mathf.Pow(multiplier, (float)CurrentLevel) * maxWatts;

        private MeterController meter;
        private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;
        private StatesInstance smi;
        private Guid statusHandle;
        private readonly CellOffset[] solarCellOffsets = Tuning.solarCellOffsets;

        protected override void OnSpawn()
        {
            base.OnSpawn();
            Subscribe((int)GameHashes.ActiveChanged, OnActiveChanged);

            smi = new StatesInstance(this);
            smi.StartSM();

            accumulator = Game.Instance.accumulators.Add("Element", this);

            BuildingDef def = GetComponent<BuildingComplete>().Def;
            int cell = Grid.PosToCell(this);

            for (int index = 0; index < def.WidthInCells; ++index)
            {
                int x = index - (def.WidthInCells - 1) / 2;
                int num = Grid.OffsetCell(cell, new CellOffset(x, 0));
                SimMessages.SetCellProperties(num, 39);
                Grid.Foundation[num] = true;
                Grid.SetSolid(num, true, CellEventLogger.Instance.SimCellOccupierForceSolid);
                World.Instance.OnSolidChanged(num);
                GameScenePartitioner.Instance.TriggerEvent(num, GameScenePartitioner.Instance.solidChangedLayer, null);
                Grid.RenderedByWorld[num] = false;
            }

            meter = new MeterController(GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[4]
            {
              "meter_target",
              "meter_fill",
              "meter_frame",
              "meter_OL"
            });

        }

        protected override void OnCleanUp()
        {
            smi.StopSM("cleanup");
            BuildingDef def = GetComponent<BuildingComplete>().Def;
            int cell = Grid.PosToCell(this);
            for (int index = 0; index < def.WidthInCells; ++index)
            {
                int x = index - (def.WidthInCells - 1) / 2;
                int num = Grid.OffsetCell(cell, new CellOffset(x, 0));
                SimMessages.ClearCellProperties(num, (byte)39);
                Grid.Foundation[num] = false;
                Grid.SetSolid(num, false, CellEventLogger.Instance.SimCellOccupierForceSolid);
                World.Instance.OnSolidChanged(num);
                GameScenePartitioner.Instance.TriggerEvent(num, GameScenePartitioner.Instance.solidChangedLayer, null);
                Grid.RenderedByWorld[num] = true;
            }
            Game.Instance.accumulators.Remove(accumulator);
            base.OnCleanUp();
        }

        protected void OnActiveChanged(object data)
        {
            StatusItem status_item = ((Operational)data).IsActive ? Db.Get().BuildingStatusItems.Wattage : Db.Get().BuildingStatusItems.GeneratorOffline;
            GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, this);
        }

        private void UpdateStatusItem()
        {
            selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.Wattage);
            if (statusHandle == Guid.Empty)
            {
                statusHandle = selectable.AddStatusItem(ModAssets.UpgradeableSolarWattStatus, this);
            }
            else
            {
                GetComponent<KSelectable>().ReplaceStatusItem(statusHandle, ModAssets.UpgradeableSolarWattStatus, this);
            }
        }

        public override void EnergySim200ms(float dt)
        {
            base.EnergySim200ms(dt);
            ushort circuitId = CircuitID;
            operational.SetFlag(wireConnectedFlag, circuitId != ushort.MaxValue);

            if (!operational.IsOperational)
                return;

            float W = 0.0f;

            foreach (CellOffset solarCellOffset in solarCellOffsets)
            {
                int lux = Grid.LightIntensity[Grid.OffsetCell(Grid.PosToCell(this), solarCellOffset)];
                W += lux * WattsPerLux;
            }

            operational.SetActive(W > 0.0);
            float cappedW = Mathf.Clamp(W, 0.0f, MaxWatt);

            Game.Instance.accumulators.Accumulate(accumulator, cappedW * dt);

            if (cappedW > 0.0)
                GenerateJoules(Mathf.Max(cappedW * dt, 1f * dt));

            meter.SetPositionPercent(Game.Instance.accumulators.GetAverageRate(accumulator) / MaxWatt);
            UpdateStatusItem();
        }

        public float CurrentWattage => Game.Instance.accumulators.GetAverageRate(accumulator);

        public class StatesInstance : GameStateMachine<States, StatesInstance, UpgradeableSolarPanel, object>.GameInstance
        {
            public StatesInstance(UpgradeableSolarPanel master) : base(master) { }
        }

        public class States : GameStateMachine<States, StatesInstance, UpgradeableSolarPanel>
        {
            public State idle;

            public override void InitializeStates(out BaseState default_state)
            {
                default_state = idle;
                idle.DoNothing();
            }
        }
        public string SidescreenTitleKey => "";

        public string SidescreenStatusMessage => $"Current Level: {CurrentLevel}\nWattsPerLux: {WattsPerLux}\nMaxWatt: {MaxWatt}";

        public string SidescreenButtonText => "Upgrade";

        public void OnSidescreenButtonPressed()
        {
            CurrentLevel++;
        }

    }
}
