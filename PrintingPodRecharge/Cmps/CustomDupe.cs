using FUtility;
using HarmonyLib;
using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PrintingPodRecharge.Cmps
{
    // TODO: support modded hairstyles
    public class CustomDupe : KMonoBehaviour
    {
        public static Dictionary<MinionStartingStats, MinionData> rolledData = new Dictionary<MinionStartingStats, MinionData>();

        public struct MinionData
        {
            public Color hairColor;
            public string descKey;

            public MinionData(Color hairColor, string descKey)
            {
                this.hairColor = hairColor;
                this.descKey = descKey;
            }
        }

        [SerializeField]
        [Serialize]
        public Color hairColor;

        [SerializeField]
        [Serialize]
        public string descKey;

        [Serialize]
        public bool dyedHair;

        [MyCmpReq]
        private KBatchedAnimController kbac;

        [MyCmpReq]
        private MinionIdentity identity;

        [MyCmpReq]
        private Accessorizer accessorizer;

        private HashedString serializedHair;

        [Serialize]
        public HashedString runtimeHair;

        [Serialize]
        public int hairID;

        private static AccessTools.FieldRef<Accessorizer, List<ResourceRef<Accessory>>> ref_accessories;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            ref_accessories = AccessTools.FieldRefAccess<Accessorizer, List<ResourceRef<Accessory>>>("accessories");
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            if(Strings.TryGet($"STRINGS.DUPLICANTS.PERSONALITIES.{descKey}.DESC", out var desc))
            {
                var key = "STRINGS.DUPLICANTS.PERSONALITIES." + identity.nameStringKey + ".DESC";
                Strings.Add(key, desc.String);
            }

            if (dyedHair)
            {
                TintHair(kbac, hairColor);
            }

            var hashCache = HashCache.Get();
            serializedHair = hashCache.Add(hashCache.Get(identity.bodyData.hair).Replace("hair_bleached", "hair"));
            OnLoadGame();
        }

        public void OnLoadGame()
        {
            if (dyedHair)
            {
                ChangeAccessorySlot(runtimeHair);
            }
        }

        public void OnSaveGame()
        {
            if (dyedHair)
            {
                Log.Debuglog("Saving game for " + this.GetProperName());
                ChangeAccessorySlot(serializedHair);
            }
        }

        //[HarmonyPatch(typeof(Accessorizer), "ApplyAccessories")]
        public class Accessorizer_ApplyAccessories_Patch
        {
            public static void Prefix(Accessorizer __instance)
            {
                if (__instance.TryGetComponent(out CustomDupe dye) && dye.dyedHair)
                {
                    dye.OnLoadGame();
                }
            }
        }

        // make sure a vanilla hair is saved as the body data, so if this mod is removed, these dupes can still load and exist
        private void ChangeAccessorySlot(HashedString value)
        {
            if (!value.IsValid)
            {
                Log.Debuglog("not valid value");
                return;
            }

            Log.Debuglog("Changing accessory slot to " + HashCache.Get().Get(value));

            identity.bodyData.hair = value;

            var items = ref_accessories(accessorizer);
            var slot = Db.Get().AccessorySlots.Hair;
            var accessories = Db.Get().Accessories;

            for (var i = 0; i < items.Count; i++)
            {
                var item = items[i];
                var accessory = item.Get();
                if (accessory.slot == slot)
                {
                    Log.Debuglog("changing slot");
                    items[i] = new ResourceRef<Accessory>(accessories.Get(value));

                    // force refresh the symbol
                    var newAccessory = items[i].Get();
                    kbac.GetComponent<SymbolOverrideController>().AddSymbolOverride(newAccessory.slot.targetSymbolId, newAccessory.symbol, 0);

                    return;
                }
            }
        }

        public static bool Apply(KMonoBehaviour dupe, KBatchedAnimController kbac = null)
        {
            if (dupe != null && dupe.TryGetComponent(out CustomDupe dye) && dye.dyedHair)
            {
                kbac = kbac ?? dupe.GetComponent<KBatchedAnimController>();
                if (kbac == null)
                {
                    return false;
                }

                TintHair(kbac, dye.hairColor);

                return true;
            }

            return false;
        }

        private static KAnimHashedString hair = new KAnimHashedString("snapTo_hair");
        private static KAnimHashedString hairAlways = new KAnimHashedString("snapTo_hair_always");
        private static KAnimHashedString hatHair = new KAnimHashedString("snapTo_hat_hair");

        public static void TintHair(KBatchedAnimController kbac, Color color, bool wait = false)
        {
            if(kbac == null)
            {
                Log.Warning("Cannot dye the hair of a dupe with no KBatchedAnimController.");
                return;
            }

            if(!hair.IsValid() || !hairAlways.IsValid() || !hatHair.IsValid())
            {
                Log.Warning("Invalid hash");
                return;
            }

            if(wait)
            {
                GameScheduler.Instance.ScheduleNextFrame("tint hair", obj => TintHairInternal(kbac, color));
}
            else
            {
                TintHairInternal(kbac, color);
            }
        }

        private static void TintHairInternal(KBatchedAnimController kbac, Color color)
        {
            kbac.SetSymbolTint(Db.Get().AccessorySlots.Hair.targetSymbolId, color);
            kbac.SetSymbolTint(Db.Get().AccessorySlots.HairAlways.targetSymbolId, color);
            kbac.SetSymbolTint(Db.Get().AccessorySlots.HatHair.targetSymbolId, color);
            Log.Debuglog("trying to tint " + hair);
        }
    }
}
