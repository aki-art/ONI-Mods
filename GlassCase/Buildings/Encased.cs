using FUtility;
using Klei.AI;
using KSerialization;
using System;
using UnityEngine;

namespace GlassCase.Buildings
{
    public class Encased : KMonoBehaviour, ISim1000ms
    {
        [MyCmpGet]
        private readonly SimTemperatureTransfer simTemperature;

        [MyCmpGet]
        PrimaryElement pe;

        [Serialize]
        private bool wasSimEnabled;

        [SerializeField]
        public float simulatedThermalConductivity = 0;

        private SimulatedTemperatureAdjuster temperatureAdjuster;

        public void Sim1000ms(float dt)
        {
            bool isSealed = gameObject.HasTag(GameTags.Sealed);
            Log.Debuglog("Has seal?" + isSealed);

            if(!isSealed)
            {
                gameObject.AddTag(GameTags.Sealed); 
            }
            var tileObject = Grid.Objects[this.NaturalBuildingCell(), (int)ObjectLayer.FoundationTile];

            if(tileObject is null || tileObject.GetComponent<GlassCase>() is null)
            {
                Destroy(this);
            }
        }


        private void OnItemSimRegistered(SimTemperatureTransfer stt)
        {
            if (stt == null) return;

            if (Sim.IsValidHandle(stt.SimHandle))
            {
                Log.Debuglog("stt");
                float num = pe.Temperature;
                float heat_capacity = pe.Element.specificHeatCapacity;
                float thermal_conductivity = 0.03f;
                SimMessages.ModifyElementChunkTemperatureAdjuster(stt.SimHandle, num, heat_capacity, thermal_conductivity);
            }
        }

        private void Restore()
        {
            if(wasSimEnabled && simTemperature is object)
            {
                simTemperature.enabled = true;
            }

            gameObject.RemoveTag(GameTags.Sealed);

            Log.Debuglog("Restored");
        }

        protected override void OnCleanUp()
        {
            temperatureAdjuster.CleanUp();
            base.OnCleanUp();
        }

        private readonly AttributeModifier conductivityModifier = new AttributeModifier(
            "ThermalConductivityBarrier",
            0,
            "Unaging",
            true
        );

        protected override void OnSpawn()
        {
            base.OnSpawn();

            gameObject.AddTag(GameTags.Sealed);

            Log.Debuglog("spawned", this.GetProperName());

            if(simTemperature is object && simTemperature.enabled)
            {
                wasSimEnabled = true;
                simTemperature.enabled = false;
                Log.Debuglog("removed temp from " + this.GetProperName());
            }

            gameObject.GetAttributes().Add(conductivityModifier);

            if (GameComps.StructureTemperatures.Has(gameObject))
            {
                //GameComps.StructureTemperatures.Remove(gameObject);
                Log.Debuglog("StructureTemperatures ByPass", this.GetProperName());
                var pe = GetComponent<PrimaryElement>();
                temperatureAdjuster = new SimulatedTemperatureAdjuster(pe.InternalTemperature, pe.Element.specificHeatCapacity, simulatedThermalConductivity, GetComponent<Storage>());

                //HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(gameObject);
                //GameComps.StructureTemperatures.Bypass(handle);
            }
        }
    }
}
