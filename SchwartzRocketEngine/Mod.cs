using FUtility;
using HarmonyLib;
using KMod;

namespace SchwartzRocketEngine
{
    public class Mod : UserMod2
    {
        public const string ID = "SchwartzRocketEngine";

        public static string Prefix(string v) => $"{ID}_{v}";

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            AddSchwartzEngineToButtons();
        }

        private void AddSchwartzEngineToButtons()
        {
            AddItemToButtons(Buildings.SchwartzEngineClusterConfig.ID, HEPEngineConfig.ID);
            AddItemToButtons(Buildings.SpaceBallHabitatModuleConfig.ID, HabitatModuleMediumConfig.ID);
            AddItemToButtons(Buildings.LuxurySportsShipConfig.ID, HabitatModuleMediumConfig.ID);
        }

        private void AddItemToButtons(string ID, string afterID)
        {
            int index = SelectModuleSideScreen.moduleButtonSortOrder.IndexOf(afterID);

            if (index == -1)
            {
                SelectModuleSideScreen.moduleButtonSortOrder.Add(ID);
                Log.Warning($"Cannot insert {ID} next to {afterID} in the menu. Putting it to the end instead.");
                return;
            }

            SelectModuleSideScreen.moduleButtonSortOrder.Insert(index, ID);
        }
    }
}
