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

            if(Mod.isResculptHere)
            {
                Subscribe((int)GameHashes.RefreshUserMenu, OnRefreshUserMenu);
            }

            Subscribe((int)GameHashes.OperationalChanged, OnOperationalChanged);
            OnOperationalChanged(GetComponent<Operational>().IsOperational);
        }

        private void OnOperationalChanged(object data)
        {
            kbac.SetSymbolVisiblity("light_bloom", (bool)data);
        }

        private void OnRefreshUserMenu(object obj)
        {
            if(!Mod.isResculptHere)
            {
                return;
            }

            var recarveButton = new KIconButtonMenu.ButtonInfo(
                    "action_resculpt",
                    "Recarve",
                    () => Carve(currentFace + 1),
                    tooltipText: "Change Face");

            Game.Instance.userMenu.AddButton(gameObject, recarveButton);

            var rotateButton = new KIconButtonMenu.ButtonInfo(
                    "action_direction_both",
                    Strings.TryGet("PeterHan.Resculpt.ResculptStrings.ROTATE_BUTTON", out var str) ? str.String : "Rotate",
                    () => GetComponent<Rotatable>().Rotate(),
                    tooltipText: Strings.TryGet("PeterHan.Resculpt.ResculptStrings.ROTATE_TOOLTIP", out var str2) ? str2.String : "Rotates artwork. {Hotkey}");

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
