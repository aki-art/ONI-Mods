using FUtility;
using HarmonyLib;
using PrintingPodRecharge.Content.Cmps;
using PrintingPodRecharge.Content.Items;
using UnityEngine;

namespace PrintingPodRecharge.Patches
{
    public class TelepadPatch
    {
        [HarmonyPatch(typeof(Telepad), "OnPrefabInit")]
        public class Telepad_OnPrefabInit_Patch
        {
            public static void Postfix(Telepad __instance)
            {
                var go = __instance.gameObject;

                var storage = go.AddComponent<Storage>();
                storage.dropOnLoad = false;
                storage.capacityKg = 2f;

                var bioink = go.AddComponent<BioPrinter>();
                bioink.storage = storage;

                var manualDeliveryKG = go.AddComponent<ManualDeliveryKG>();
                manualDeliveryKG.SetStorage(storage);
                manualDeliveryKG.allowPause = false;
                manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
                manualDeliveryKG.RequestedItemTag = Tag.Invalid;
                manualDeliveryKG.refillMass = 2f;
                manualDeliveryKG.MinimumMass = 2f;
                manualDeliveryKG.capacity = 2f;
            }
        }

        [HarmonyPatch(typeof(Telepad), "RejectAll")]
        public class Telepad_RejectAll_Patch
        {
            public static void Postfix(Telepad __instance)
            {
                if(!Mod.Settings.RefundeInk)
                    return;

                var amount = Mod.Settings.RefundBioInkKg;
                var tag = BioInkConfig.DEFAULT;

                if (Mod.Settings.RefundActiveInk &&
                    ImmigrationModifier.Instance.refundBundle != Bundle.None &&
                    BioInkConfig.itemsToBundle.TryGetValue(ImmigrationModifier.Instance.refundBundle, out var activeInk))
                {
                    tag = activeInk;
                }

                var ink = Utils.Spawn(tag, __instance.gameObject.transform.position + Vector3.up);

                ink.GetComponent<PrimaryElement>().Mass = amount;
                Utils.YeetRandomly(ink, true, 3, 4, true);
                PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, STRINGS.ITEMS.BIO_INK.NAME, __instance.transform, Vector3.zero);

                ImmigrationModifier.Instance.SetRefund(Bundle.None);
            }
        }


        [HarmonyPatch(typeof(Telepad), "OnAcceptDelivery")]
        public class Telepad_OnAcceptDelivery_Patch
        {
            public static void Postfix()
            {
                ImmigrationModifier.Instance.SetRefund(Bundle.None);
            }
        }

        [HarmonyPatch(typeof(Telepad.States), "InitializeStates")]
        public class Telepad_States_InitializeStates_Patch
        {
            public static void Postfix(Telepad.States __instance)
            {
                __instance.opening
                    .TriggerOnEnter((GameHashes)ModHashes.PrintEvent, smi => true);

                __instance.close
                    .TriggerOnEnter((GameHashes)ModHashes.PrintEvent, smi => false);
            }
        }
    }
}
