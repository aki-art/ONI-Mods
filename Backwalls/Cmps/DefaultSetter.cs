namespace Backwalls.Cmps
{
    public class DefaultSetter : KMonoBehaviour, ISidescreenButtonControl
    {
        [MyCmpReq] Backwall backwall;

        public string SidescreenButtonText => STRINGS.UI.BACKWALLS_DEFAULTSETTERSIDESCREEN.SET_DEFAULT.BUTTON;

        public string SidescreenButtonTooltip => STRINGS.UI.BACKWALLS_DEFAULTSETTERSIDESCREEN.SET_DEFAULT.TOOLTIP;

        public int ButtonSideScreenSortOrder() => -1;

        public int HorizontalGroupID() => -1;

        public void OnSidescreenButtonPressed()
        {
            Mod.Settings.DefaultPattern = backwall.pattern;
            Mod.Settings.DefaultColor = backwall.colorHex;
            Mod.SaveSettings();
        }

        public void SetButtonTextOverride(ButtonMenuTextOverride _) { }

        public bool SidescreenButtonInteractable() => true;

        public bool SidescreenEnabled() => true;
    }
}
