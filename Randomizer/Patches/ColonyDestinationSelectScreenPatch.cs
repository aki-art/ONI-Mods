using FUtility;
using HarmonyLib;
using Randomizer.Content.Scripts;

namespace Randomizer.Patches
{
    public class ColonyDestinationSelectScreenPatch
    {
        [HarmonyPatch(typeof(ColonyDestinationSelectScreen), "LaunchClicked")]
        public class ColonyDestinationSelectScreen_LaunchClicked_Patch
        {
            public static bool Prefix(ColonyDestinationSelectScreen __instance)
            {
                if(DestinationSelectPanel.ChosenClusterCategorySetting == ModDb.RANDOM_CLUSTER_CATEGORY)
                {
                    var screen = Util.KInstantiateUI<InfoDialogScreen>(
                        ScreenPrefabs.Instance.InfoDialogScreen.gameObject, 
                        FUtility.FUI.Helper.GetACanvas("info").gameObject, 
                        true);

                    var generator = screen.gameObject.AddComponent<WorldRandomizerGenerator>();

                    var infoScreenLineItem = Util.KInstantiateUI<InfoScreenLineItem>(
                        screen.lineItemTemplate.gameObject, 
                        screen.contentContainer);

                    generator.Configure(infoScreenLineItem, __instance);

                    screen
                        .SetHeader("header")
                        .AddLineItem("Generating world...", "")
                        .AddOption("Cancel", _ =>
                        {
                            generator.OnCancel();
                            screen.Deactivate();
                        })
                        .AddOption("(debug) Continue", _ => __instance.NavigateForward());

                    return false;
                }

                return true;
            }

        }
    }
}
