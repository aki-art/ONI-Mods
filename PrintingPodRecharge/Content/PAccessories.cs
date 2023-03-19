using Database;
using HarmonyLib;

namespace PrintingPodRecharge.Content
{
    public class PAccessories
    {
        [HarmonyPatch(typeof(AccessorySlots), MethodType.Constructor, typeof(ResourceSet))]
        public class AccessorySlots_TargetMethod_Patch
        {
            public static void Postfix(AccessorySlots __instance, ResourceSet parent)
            {
                var hair = Assets.GetAnim("rrp_bleachedhair_kanim");

                AddAccessories(hair, __instance.Hair, parent);
                AddAccessories(hair, __instance.HatHair, parent);
            }
        }

        public static void AddAccessories(KAnimFile file, AccessorySlot slot, ResourceSet parent)
        {
            var build = file.GetData().build;
            var id = slot.Id.ToLower();

            for (var i = 0; i < build.symbols.Length; i++)
            {
                var symbolName = HashCache.Get().Get(build.symbols[i].hash);
                if (symbolName.StartsWith(id))
                {
                    var accessory = new Accessory(symbolName, parent, slot, file.batchTag, build.symbols[i]);
                    slot.accessories.Add(accessory);
                    HashCache.Get().Add(accessory.IdHash.HashValue, accessory.Id);
                }
            }
        }
    }
}
