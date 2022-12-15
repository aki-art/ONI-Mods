using KSerialization;
using UnityEngine;

namespace SpookyPumpkinSO.Content.Buildings
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class CarvedPumpkin : KMonoBehaviour
    {
        [Serialize]
        public int currentFace;

        [MyCmpReq]
        private KBatchedAnimController kbac;

        private const int MAX_INDEX = 7;

        public CarvedPumpkin()
        {
            currentFace = -1;
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            if(currentFace == -1)
            {
                var newIndex = Random.Range(0, MAX_INDEX + 1);
                Carve(newIndex);
            }

            Subscribe((int)GameHashes.RefreshUserMenu, OnRefreshUserMenu);
            Subscribe((int)GameHashes.OperationalChanged, OnOperationalChanged);

            OnOperationalChanged(GetComponent<Operational>().IsOperational);
        }

        private void OnOperationalChanged(object data)
        {
            kbac.SetSymbolVisiblity("light_bloom", (bool)data);
        }

        private void OnRefreshUserMenu(object obj)
        {
            var recarveButton = new KIconButtonMenu.ButtonInfo(
                    "icon_archetype_art",
                    STRINGS.UI.RECARVE,
                    () => Carve(currentFace + 1),
                    tooltipText: global::STRINGS.UI.UISIDESCREENS.ARTABLESELECTIONSIDESCREEN.TITLE);

            Game.Instance.userMenu.AddButton(gameObject, recarveButton);

            var rotateButton = new KIconButtonMenu.ButtonInfo(
                    "action_direction_both",
                    STRINGS.UI.ROTATE,
                    () => GetComponent<Rotatable>().Rotate(),
                    tooltipText: global::STRINGS.UI.BUILDTOOL_ROTATE);

            Game.Instance.userMenu.AddButton(gameObject, rotateButton);
        }

        public void Carve(int index)
        {
            if(index > MAX_INDEX)
            {
                index = 0;
            }

            kbac.Play("jack" + index);
            currentFace = index;
        }
    }
}
