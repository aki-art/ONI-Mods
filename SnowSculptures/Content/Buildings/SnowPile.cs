using KSerialization;

namespace SnowSculptures.Content.Buildings
{
    public class SnowPile : Sculpture
    {
        [Serialize]
        public int petCapacity;

        public const int MAX_PET = 16;
        public const string SNOWDOG = "SnowSculptures_SnowSculpture_Snowdog";

        private bool pettable;

        [MyCmpReq]
        private KBatchedAnimController kbac;

        [MyCmpReq]
        private Sculpture sculpture;

        protected override void OnSpawn()
        {
            base.OnSpawn();

            UpdateDog();
            Subscribe((int)GameHashes.RefreshUserMenu, OnRefreshUserMenu);
            kbac.initialMode = KAnim.PlayMode.Paused;
        }

        public void Pet()
        {
            petCapacity++;
            UpdateDog();
        }

        public override void SetStage(string stage_id, bool skip_effect)
        {
            base.SetStage(stage_id, skip_effect);
            UpdateDog();
        }

        public void UpdateDog()
        {
            pettable = sculpture.CurrentStage == SNOWDOG;

            if(!pettable)
            {
                kbac.SetPositionPercent(0);
                petCapacity = 0;

                return;
            }

            kbac.PlayMode = KAnim.PlayMode.Paused;

            if (petCapacity > MAX_PET)
            {
                return;
            }

            kbac.SetPositionPercent((float)petCapacity / MAX_PET);
        }

        private void OnRefreshUserMenu(object obj)
        {
            if (pettable)
            {
                var button = new KIconButtonMenu.ButtonInfo("action_switch_toggle", "Pet", Pet, tooltipText: "WIP", is_interactable: false);
                Game.Instance.userMenu.AddButton(gameObject, button);
            }
        }
    }
}
