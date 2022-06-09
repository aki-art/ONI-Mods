using DecorPackB.Content;
using HarmonyLib;

namespace DecorPackB.Patches
{
    public class AccessorySlotsPatch
    {
        [HarmonyPatch(typeof(Database.AccessorySlots), MethodType.Constructor, typeof(ResourceSet))]
        public class Database_AccessorySlots_Ctor_Patch
        {
            public static void Postfix(Database.AccessorySlots __instance)
            {
                DPAccessories.Register(__instance);
            }
        }
    }
}
