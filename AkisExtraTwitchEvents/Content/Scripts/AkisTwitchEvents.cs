using FUtility;
using KSerialization;
using System;
using System.Collections.Generic;
using System.Linq;
using Twitchery.Content.Defs;
using Twitchery.Content.Events;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class AkisTwitchEvents : KMonoBehaviour, ISim4000ms
    {
        public static AkisTwitchEvents Instance;

        [Serialize]
        public float lastLongBoiSpawn;

        [Serialize]
        public float lastRadishSpawn;

        [Serialize]
        internal bool hasRaddishSpawnedBefore;

        [Serialize]
        public bool hasUnlockedPizzaRecipe;

        //[Serialize]
        //public List<Ref<KPrefabID>> wormies;

        public static string pizzaRecipeID;
        public static string radDishRecipeID;

        public AkisTwitchEvents()
        {
            lastLongBoiSpawn = float.NegativeInfinity;
            lastRadishSpawn = 0;
        }
/*
        public void AddLongWormy(GameObject wormy)
        {
            if (wormy != null && wormy.TryGetComponent(out KPrefabID kPrefabID))
            {
                wormies ??= new();
                wormies.Add(new Ref<KPrefabID>(kPrefabID));
            }
        }

        public bool HasWormy(GameObject wormy)
        {
            if(wormies == null)
            {
                return false;
            }

            if (wormy.TryGetComponent(out KPrefabID kPrefabID))
            {
                return (wormies.Any(w => w != null && w.Get() == kPrefabID));
            }

            return false;
        }

        public void RemoveWormy(GameObject wormy)
        {
            if (wormy.TryGetComponent(out KPrefabID kPrefabID))
            {
                wormies?.RemoveAll(w => w != null && w.Get() == kPrefabID);
            }
        }
*/
        public override void OnPrefabInit()
        {
            Instance = this;
        }

        public override void OnSpawn()
        {
            Log.Debuglog("akis twitch events spawn");
            base.OnSpawn();
        }

        public override void OnCleanUp()
        {
            base.OnCleanUp();
            Instance = null;
        }

        public void Sim4000ms(float dt)
        {
            if(TwitchEvents.myEvents.Count > 0)
            {
                foreach(var ev in TwitchEvents.myEvents)
                {
                    Log.Debuglog($"{ev.GetID()} {ev.Condition(null)}");
                }
            }
        }
    }
}
