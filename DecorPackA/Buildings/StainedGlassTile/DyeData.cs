using FUtility;
using Klei.AI;
using KSerialization;
using System;
using System.Collections.Generic;

namespace DecorPackA.Buildings.StainedGlassTile
{
    //[SerializationConfig(MemberSerialization.OptIn)]
    public class DyeData : KMonoBehaviour
    {
        //[Serialize]
        //public static Dictionary<int, float> temperature = new Dictionary<int, float>();

        protected override void OnSpawn()
        {
            base.OnSpawn();
            GameplayEventManager.Instance.Subscribe((int)GameHashes.NewBuilding, OnNewBuilding);
        }

        private void OnNewBuilding(object obj)
        {
            Log.Debuglog("NEW BUILDING");

            if (obj is BonusEvent.GameplayEventData data && 
                data.workable is Constructable constructable && 
                constructable.TryGetComponent(out Storage storage) &&
                storage.items.Count > 1)
                //data.building is BuildingComplete building && 
                //building.TryGetComponent(out DyeInsulator dyeInsulator))
            {
                //var temp = storage[1].GetComponent<PrimaryElement>().Temperature;
                //dyeInsulator.SetDyeTemperature(temp);
                // Log.Debuglog($"Temperature is {temp}");
                Log.Debuglog($"condition");
            }
        }
    }
}
