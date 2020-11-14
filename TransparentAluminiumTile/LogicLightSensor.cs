using KSerialization;
using STRINGS;
using System.Linq;
using UnityEngine;

namespace TransparentAluminium
{
    [SerializationConfig(MemberSerialization.OptIn)]
    class LogicLightSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
    {
        [SerializeField]
        [Serialize]
        private float threshold;
        [SerializeField]
        [Serialize]
        private bool activateAboveThreshold = true;
        private bool wasOn;
        public float rangeMin;
        public float rangeMax = 1f;
        private const int WINDOW_SIZE = 4;
        private float[] samples = new float[WINDOW_SIZE];
        private int sampleIdx;
        private static readonly EventSystem.IntraObjectHandler<LogicLightSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicLightSensor>((component, data) => component.OnCopySettings(data));
        
        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            FindOrAdd<CopyBuildingSettings>();
            Subscribe((int)GameHashes.CopySettings, OnCopySettingsDelegate);
        }

        private void OnCopySettings(object data)
        {
            LogicLightSensor original = ((GameObject)data).GetComponent<LogicLightSensor>();
            if (original != null)
            { 
                Threshold = original.Threshold;
                ActivateAboveThreshold = original.ActivateAboveThreshold;
            }
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            OnToggle += OnSwitchToggled;
            UpdateLogicCircuit();
            UpdateVisualState(true);
            wasOn = switchedOn;
        }

        public void Sim200ms(float dt)
        {
            int cell = Grid.PosToCell(this);
            if (sampleIdx < WINDOW_SIZE)
            {
                samples[sampleIdx] = Grid.LightCount[cell];
                ++sampleIdx;
            }
            else
            {
                sampleIdx = 0;
                float currentValue = CurrentValue;
                if (activateAboveThreshold)
                {
                    if ((currentValue <= (double)threshold || IsSwitchedOn) && (currentValue > (double)threshold || !IsSwitchedOn))
                        return;
                    Toggle();
                }
                else
                {
                    if ((currentValue <= (double)threshold || !IsSwitchedOn) && (currentValue > (double)threshold || IsSwitchedOn))
                        return;
                    Toggle();
                }
            }
        }

        private void OnSwitchToggled(bool toggled_on)
        {
            UpdateLogicCircuit();
            UpdateVisualState();
        }

        public float Threshold
        {
            get => threshold;
            set => threshold = value;
        }

        public bool ActivateAboveThreshold
        {
            get => activateAboveThreshold;
            set => activateAboveThreshold = value;
        }

        public float CurrentValue => samples.Average();

        public float RangeMin => rangeMin;

        public float RangeMax => rangeMax;

        public float GetRangeMinInputField() => rangeMin;

        public float GetRangeMaxInputField() => rangeMax;

        public LocString ThresholdValueName => UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE;

        public string AboveToolTip => UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_ABOVE;

        public string BelowToolTip => UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_BELOW;

        public string Format(float value, bool units) => GameUtil.GetFormattedLux(Mathf.RoundToInt(value));

        public float ProcessedSliderValue(float input) => Mathf.Round(input);

        public float ProcessedInputValue(float input) => input;

        public LocString ThresholdValueUnits() => "lux?"; // GameUtil.GetCurrentMassUnit(desiredState == Element.State.Gas);

        public LocString Title => UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TITLE;

        public ThresholdScreenLayoutType LayoutType => ThresholdScreenLayoutType.SliderBar;

        public int IncrementScale => 1;

        public NonLinearSlider.Range[] GetRanges => NonLinearSlider.GetDefaultRange(RangeMax);

        private void UpdateLogicCircuit() => GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, switchedOn ? 1 : 0);

        private void UpdateVisualState(bool force = false)
        {
            if (!(wasOn != switchedOn | force))
                return;
            wasOn = switchedOn;
            KBatchedAnimController component = GetComponent<KBatchedAnimController>();
            component.Play(switchedOn ? "on_pre" : "on_pst");
            component.Queue(switchedOn ? "on" : "off");
        }

        protected override void UpdateSwitchStatus()
        {
            StatusItem status_item = switchedOn ? 
                Db.Get().BuildingStatusItems.LogicSensorStatusActive : 
                Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
            GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item);
        }

    }
}
