using FUtility;
using HarmonyLib;
using KSerialization;
using System.Collections.Generic;

namespace SpookyPumpkinSO.Content.Cmps
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class FacePaint : KMonoBehaviour
    {
        [MyCmpGet]
        private Accessorizer accessorizer;

        [Serialize]
        public string currentFaceAccessory;

        [Serialize]
        public string originalFaceAccessory;

        [Serialize]
        public bool hasCustomMouth;

        [MyCmpReq]
        private MinionIdentity identity;

        [MyCmpReq]
        private KBatchedAnimController kbac;

        private static AccessTools.FieldRef<Accessorizer, List<ResourceRef<Accessory>>> ref_accessories;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            ref_accessories = AccessTools.FieldRefAccess<Accessorizer, List<ResourceRef<Accessory>>>("accessories");
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            originalFaceAccessory = Db.Get().Accessories.Get(accessorizer.bodyData.mouth).Id;
            Log.Debuglog("ORIGINAL FACE IS " + originalFaceAccessory);

            OnLoadGame();

            if (hasCustomMouth)
            {
                Apply(currentFaceAccessory);
            }

            Mod.facePaints.Add(this);
        }

        public void Apply(string accessory)
        {
            if (accessorizer != null)
            {
                ReplaceAccessory(accessory);
                currentFaceAccessory = accessory;

                hasCustomMouth = true;
            }
        }

        private void ReplaceAccessory(string accessory)
        {
            var mouthSlot = Db.Get().AccessorySlots.Mouth;
            var newAccessory = mouthSlot.Lookup(accessory);
            var currentAccessory = accessorizer.GetAccessory(mouthSlot);
            Log.Debuglog($"replacing accessory from {currentAccessory.Id} to {accessory}");

            if (newAccessory == null)
            {
                Log.Warning($"Could not add accessory {accessory}, it was not found in the database.");
                return;
            }

            accessorizer.RemoveAccessory(currentAccessory);
            accessorizer.AddAccessory(newAccessory);
            accessorizer.ApplyAccessories();
        }

        public void Remove()
        {
            if (accessorizer != null)
            {
                ReplaceAccessory(originalFaceAccessory);
                currentFaceAccessory = null;
            }

            hasCustomMouth = false;
        }

        public void OnLoadGame()
        {
            if (hasCustomMouth)
            {
                ChangeAccessorySlot(currentFaceAccessory);
            }
        }

        public void OnSaveGame()
        {
            if (hasCustomMouth)
            {
                ChangeAccessorySlot(originalFaceAccessory);
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

            var bodyData = accessorizer.bodyData;
            bodyData.mouth = value;

            var items = ref_accessories(accessorizer);
            var slot = Db.Get().AccessorySlots.Mouth;
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
    }
}
