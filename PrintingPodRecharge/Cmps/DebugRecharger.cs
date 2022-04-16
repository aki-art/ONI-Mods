namespace PrintingPodRecharge.Cmps
{
    public class DebugRecharger : KMonoBehaviour, ISidescreenButtonControl
    {
        public string SidescreenButtonText => "Recharge (debug)";

        public string SidescreenButtonTooltip => "Instantly recharges Printing Pod if the game is in Insta-build Debug Mode.";

        public int ButtonSideScreenSortOrder() => 999;

        public void OnSidescreenButtonPressed()
        {
            Immigration.Instance.timeBeforeSpawn = 0;
        }

        public bool SidescreenButtonInteractable() => true;

        public bool SidescreenEnabled() => DebugHandler.InstantBuildMode;
    }
}
