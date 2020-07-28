using Harmony;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using static DetailsScreen;
using static Utils.Buildings;

namespace Curtain
{

    class HarmonyPatches
    {
        public static GameObject SidescreenPrefab { get; set; }
        public static GameObject SidescreenParent { get; set; }

        public static class Mod_OnLoad
        {
            public static void OnLoad(string path)
            {
                Log.Info("Loaded Curtain doors version " + typeof(Mod_OnLoad).Assembly.GetName().Version.ToString());
            }
        }

        [HarmonyPatch(typeof(Database.BuildingStatusItems), "CreateStatusItems")]
        public static class Database_BuildingStatusItems_CreateStatusItems_Patch
        {
            public static void Postfix()
            {
                var item = new StatusItem(
                         id: "ChangeCurtainControlState",
                         prefix: "BUILDING",
                         icon: "status_item_pending_switch_toggle",
                         icon_type: StatusItem.IconType.Custom,
                         notification_type: NotificationType.Neutral,
                         allow_multiples: false,
                         render_overlay: OverlayModes.None.ID);

                item.resolveStringCallback = delegate (string str, object data)
                {
                    var curtain = (Curtain)data;
                    return str.Replace("{CurrentState}", curtain.RequestedState.ToString());
                };

                ModAssets.ChangeCurtainControlState = item;
            }
        }

        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                var buildingsToRegister = new List<BuildingConfig>()
                {
                    new BuildingConfig(PlasticCurtainConfig.ID, typeof(PlasticCurtainConfig))
                };

                RegisterAllBuildings(buildingsToRegister);

                Strings.Add($"STRINGS.UI.UISIDESCREENS.CURTAIN_SIDE_SCREEN.TITLE", "Curtain test sidescreen");
                Strings.Add($"STRINGS.BUILDING.STATUSITEMS.CHANGECURTAINCONTROLSTATE.NAME", "Pending Curtain State Change: {CurrentState}");
                Strings.Add($"STRINGS.BUILDING.STATUSITEMS.CHANGECURTAINCONTROLSTATE.TOOLTIP", "Waiting for a Duplicant to change control state");
            }
        }


        [HarmonyPatch(typeof(DetailsScreen), "OnPrefabInit")]
        public static class DetailsScreen_OnPrefabInit_Patch
        {
            public static void Postfix()
            {
                // TODO: make safe
                var trInst = Traverse.Create(Instance);
                var screens = trInst.Field<List<SideScreenRef>>("sideScreens").Value;
                string name = typeof(CurtainSideScreen).Name;
                SidescreenParent = trInst.Field<GameObject>("sideScreenContentBody").Value;
                SidescreenPrefab = screens.Find(s => s.name == "Door Toggle Side Screen").screenPrefab.ContentContainer;

                if (SidescreenParent != null && screens != null)
                {
                    var container = new GameObject(name);
                    container.transform.SetParent(SidescreenParent.transform);
                    var curtainSS = container.AddComponent<CurtainSideScreen>();

                    var newScreen = new SideScreenRef
                    {
                        name = "STRINGS.UI.UISIDESCREENS.CURTAIN_SIDE_SCREEN.TITLE",
                        offset = Vector2.zero,
                        screenPrefab = curtainSS,
                        screenInstance = curtainSS
                    };

                    screens.Add(newScreen);
                }
            }
        }
    }
}
