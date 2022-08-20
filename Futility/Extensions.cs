using ProcGen;
using System.Collections.Generic;
using System.Linq;

namespace FUtility
{
    public static class Extensions
    {
        public static GameStateMachine<SMType, SMIType, MasterType, DefType>.State DebugStatusItem<SMType, SMIType, MasterType, DefType>(this GameStateMachine<SMType, SMIType, MasterType, DefType>.State state, object msg) 
            where SMType : GameStateMachine<SMType, SMIType, MasterType, DefType> 
            where SMIType : GameStateMachine<SMType, SMIType, MasterType, DefType>.GameInstance 
            where MasterType : IStateMachineTarget
        {
#if DEBUG
            state.ToggleStatusItem(msg.ToString(), "");
#endif
            return state;
        }

        public static GameStateMachine<SMType, SMIType, MasterType, DefType>.State LogMe<SMType, SMIType, MasterType, DefType>(this GameStateMachine<SMType, SMIType, MasterType, DefType>.State state, object msg)
            where SMType : GameStateMachine<SMType, SMIType, MasterType, DefType>
            where SMIType : GameStateMachine<SMType, SMIType, MasterType, DefType>.GameInstance
            where MasterType : IStateMachineTarget
        {
#if DEBUG
            state
                .Enter(smi => Log.Debuglog("entered: " + state.name))
                .Exit(smi => Log.Debuglog("exited: " + state.name));
#endif
            return state;
        }

        public static void FAddAll<T>(this IEnumerable<T> enumerator, params T[] items)
        {
            FAddAll(enumerator, items.AsEnumerable());
        }

        public static void FAddAll<T>(this IEnumerable<T> enumerator, IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                enumerator.Append(item);
            }
        }

        public static T GetWeightedRandom<T>(this IEnumerable<T> enumerator, SeededRandom rand = null) where T : IWeighted
        {
            if (enumerator == null || enumerator.Count() ==  0) return default;

            float totalWeight = enumerator.Sum(n => n.weight);
            float treshold = rand == null ? UnityEngine.Random.value : rand.RandomValue();
            treshold *= totalWeight;

            float num3 = 0.0f;
            foreach (T item in enumerator)
            {
                num3 += item.weight;
                if (num3 > treshold)
                    return item;
            }

            return enumerator.GetEnumerator().Current;
        }
    }
}
