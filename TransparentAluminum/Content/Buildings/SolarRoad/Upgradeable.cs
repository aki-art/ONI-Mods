using KSerialization;
using UnityEngine;

namespace TransparentAluminum.Content.Buildings.SolarRoad
{
    internal class Upgradeable : KMonoBehaviour, ISidescreenButtonControl
    {
        [SerializeField]
        public int maxLevel;

        [Serialize]
        public int currentLevel;

        public string SidescreenButtonText => "Upgrade";

        public string SidescreenButtonTooltip => $"{currentLevel} / {maxLevel}";

        public int ButtonSideScreenSortOrder()
        {
            return 0;
        }

        public void OnSidescreenButtonPressed()
        {
            Trigger((int)ModHashes.OnBuildingUpgraded, ++currentLevel);
        }

        public bool SidescreenButtonInteractable()
        {
            return currentLevel < maxLevel;
        }

        public bool SidescreenEnabled()
        {
            return true;
        }
    }
}
