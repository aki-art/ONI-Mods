using Terraformer.Entities;

namespace Terraformer
{
    public class InstantDestroyablePlanet : KMonoBehaviour, ISidescreenButtonControl
    {
        [MyCmpReq]
        WorldContainer worldContainer;

        public string SidescreenButtonText => "Destroy";

        public string SidescreenButtonTooltip => "";

        public bool IsWorldTargetable()
        {
            return 
                !worldContainer.IsStartWorld &&
                worldContainer.IsDiscovered &&
                Mod.WorldDestroyers.GetWorldItems(this.GetMyWorldId()).Count == 0;
        }

        public int ButtonSideScreenSortOrder() => 0;

        public void OnSidescreenButtonPressed()
        {
            WorldDestroyer destroyer = FUtility.Utils.Spawn(WorldDestroyerConfig.ID, worldContainer.minimumBounds).GetComponent<WorldDestroyer>();
            destroyer.Detonate();
        }

        public bool SidescreenButtonInteractable() => IsWorldTargetable();

        public bool SidescreenEnabled() => true;
    }
}
