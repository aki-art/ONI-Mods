using Klei.AI;

namespace Slag.Content
{
    public class SAmounts
    {
        public const string SHELLGROWTH_ID = "ShellGrowth";
        public const string SHELLINTEGRITY_ID = "ShellIntegrity";

        public static Amount ShellGrowth;
        public static Amount ShellIntegrity;

        public static void Register(Database.Amounts instance)
        {
            ShellGrowth = instance.CreateAmount(
                    SHELLGROWTH_ID,
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

            ShellGrowth.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.PerCycle, null));

            ShellIntegrity = instance.CreateAmount(
                        SHELLINTEGRITY_ID,
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

            ShellIntegrity.SetDisplayer(new StandardAmountDisplayer(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.PerCycle, null));
        }
    }
}
