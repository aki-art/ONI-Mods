namespace Bomb
{
    // for now just adds a button
    class BigBomb : KMonoBehaviour, ISidescreenButtonControl
    {
        [MyCmpReq]
        private Bomb2 bomb;

        public string SidescreenTitleKey => "Bomb.STRINGS.UI.BIGBOMB.SIDESCREEN.TITLE";

        public string SidescreenStatusMessage => STRINGS.UI.BIGBOMB.SIDESCREEN.MESSAGE;

        public string SidescreenButtonText => STRINGS.UI.BIGBOMB.SIDESCREEN.BUTTON;

        public void OnSidescreenButtonPressed()
        {
            bomb.Explode();
        }
    }
}
