using FUtility;
using HarmonyLib;
using PrintingPodRecharge.Cmps;
using PrintingPodRecharge.Items;
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

                go.AddComponent<DebugRecharger>();

                var bioink = go.AddComponent<BioPrinter>();
                bioink.storage = storage;

                var manualDeliveryKG = go.AddComponent<ManualDeliveryKG>();
                manualDeliveryKG.SetStorage(storage);
                manualDeliveryKG.allowPause = false;
                manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
                manualDeliveryKG.requestedItemTag = Tag.Invalid;
                manualDeliveryKG.refillMass = 2f;
                manualDeliveryKG.minimumMass = 2f;
                manualDeliveryKG.capacity = 2f;
            }
        }

        [HarmonyPatch(typeof(Telepad), "RejectAll")]
        public class Telepad_RejectAll_Patch
        {
            public static void Postfix(Telepad __instance)
            {
                var amount = DebugHandler.InstantBuildMode ? 50f : 1f;
                var ink = Utils.Spawn(BioInkConfig.DEFAULT, __instance.gameObject.transform.position + Vector3.up);

                ink.GetComponent<PrimaryElement>().Mass = amount;
                Utils.YeetRandomly(ink, true, 3, 4, true);
                //PlaySound(GlobalAssets.GetSound("squirrel_plant_barf"));
                PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, STRINGS.ITEMS.BIO_INK.NAME, __instance.transform, Vector3.zero);

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
