using FUtility;
using HarmonyLib;
using KSerialization;
using System.Collections.Generic;
using UnityEngine;

namespace PrintingPodRecharge.Cmps
{
    // TODO: support modded hairstyles
    public class HairDye : KMonoBehaviour
    {
        public static Dictionary<MinionStartingStats, Color> rolledHairs = new Dictionary<MinionStartingStats, Color>();

        [SerializeField]
        [Serialize]
        public Color hairColor;

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
                ChangeAccessorySlot(serializedHair);
            }
        }

        //[HarmonyPatch(typeof(Accessorizer), "ApplyAccessories")]
        public class Accessorizer_ApplyAccessories_Patch
        {
            public static void Prefix(Accessorizer __instance)
            {
                Log.Debuglog("Applyaccessories");
                if (__instance.TryGetComponent(out HairDye dye) && dye.dyedHair)
                {
                    Log.Debuglog("trying to restore accessorize");
                    dye.OnLoadGame();
                }
            }
        }

        // make sure a vanilla hair is saved as the body data, so if this mod is removed, these dupes can still load and exist
        private void ChangeAccessorySlot(HashedString value)
        {
            if (!value.IsValid)
            {
                return;
            }

            identity.bodyData.hair = value;

            var items = ref_accessories(accessorizer);
            for (var i = 0; i < items.Count; i++)
            {
                var item = items[i];
                var accessory = item.Get();
                if (accessory.slot.Id == "Hair")
                {
                    items[i] = new ResourceRef<Accessory>(Db.Get().Accessories.Get(value));

                    // force refresh the symbol
                    var newAccessory = items[i].Get();
                    kbac.GetComponent<SymbolOverrideController>().AddSymbolOverride(newAccessory.slot.targetSymbolId, newAccessory.symbol, 0);

                    return;
                }
            }
        }

        public static bool Apply(KMonoBehaviour dupe, KBatchedAnimController kbac = null)
        {
            if (dupe != null && dupe.TryGetComponent(out HairDye dye) && dye.dyedHair)
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

        public static void TintHair(KBatchedAnimController kbac, Color color)
        {
            kbac.SetSymbolTint("snapto_hair", color);
            kbac.SetSymbolTint("snapto_hair_always", color);
            kbac.SetSymbolTint("snapto_hat_hair", color);
        }
    }
}
