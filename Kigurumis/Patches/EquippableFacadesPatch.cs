using Database;
using HarmonyLib;
using Kigurumis.Content;

namespace Kigurumis.Patches
{
    [HarmonyPatch(typeof(EquippableFacades), "Load")]
    public class EquippableFacades_Load_Patch
    {
        public static void Postfix(EquippableFacades __instance)
        {
            __instance.resources.Add(new EquippableFacadeResource(
                "Kigurumi_Unicorn_HoodDown",
                "kigurumi_unicorn_hoodon_kanim", //"kigurumi_01_kanim",
                KigurumiConfig.ID,
                "kigurumi_unicorn_hoodon_kanim"));
        }
    }
}
