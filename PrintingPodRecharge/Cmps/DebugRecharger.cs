namespace PrintingPodRecharge.Cmps
{
    public class DebugRecharger : KMonoBehaviour, ISidescreenButtonControl
    {
        public string SidescreenButtonText => "Recharge (debug)";

        public string SidescreenButtonTooltip => "";

        public int ButtonSideScreenSortOrder() => 0;

        public void OnSidescreenButtonPressed()
        {
            Immigration.Instance.timeBeforeSpawn = 0;
        }

        public bool SidescreenButtonInteractable() => true;

        public bool SidescreenEnabled() => DebugHandler.InstantBuildMode;
    }
}
