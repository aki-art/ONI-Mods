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
            CreateKigurumi("unicorn", __instance);
        }

        private static void CreateKigurumi(string name, EquippableFacades facades)
        {
            facades.resources.Add(new EquippableFacadeResource(
                $"kigurumi_{name}",
                $"kigurumi_{name}_kanim",
                HoodedKigurumiConfig.ID,
                $"kigurumi_{name}_kanim"));

            facades.resources.Add(new EquippableFacadeResource(
                $"kigurumi_{name}_hoodless",
                $"kigurumi_{name}_hoodless_kanim",
                HoodedKigurumiConfig.ID,
                $"kigurumi_{name}_hoodless_kanim"));
        }
    }
}
