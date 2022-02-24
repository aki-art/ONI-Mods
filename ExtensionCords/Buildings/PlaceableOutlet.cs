using System.Collections.Generic;

namespace ExtensionCords.Buildings
{
    public class PlaceableOutlet : KMonoBehaviour, ISidescreenButtonControl
    {

        public string SidescreenButtonText => "Reel out";

        public string SidescreenButtonTooltip => "";

        public int ButtonSideScreenSortOrder()
        {
            return 0;
        }

        public void OnSidescreenButtonPressed()
        {
            BuildingDef def = Assets.GetBuildingDef(ExtensionCord.ExtensionCordOutletConfig.ID);
            ReelTool.Instance.Activate(def, new List<Tag> { SimHashes.Copper.CreateTag() });
        }

        public bool SidescreenButtonInteractable()
        {
            return true;
        }

        public bool SidescreenEnabled()
        {
            return true;
        }
    }
}
