using HarmonyLib;

namespace Slag.Patches
{
    public class AmountsPatch
    {
        [HarmonyPatch(typeof(Database.Amounts), "Load")]
        public static class Amounts_Load_Patch
        {
            public static void Postfix(Database.Amounts __instance)
            {
                ModAssets.Amounts.ShellGrowth = __instance.CreateAmount(
                    "ShellGrowth",
                    0f,
                    100f,
                    false,
                    Units.Flat,
                    0.35f,
                    true,
                    "STRINGS.CREATURES",
                    "ui_icon_stamina",
                    "attribute_stamina",
                    "mod_stamina");

                ModAssets.Amounts.ShellGrowth.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.PerCycle, null));
            }
        }
    }
}
