using KSerialization;

namespace SnowSculptures.Content.Buildings
{
    public class SnowPile : Artable
    {
        [Serialize]
        public int petCapacity;

        [Serialize]
        public bool broken;

        public int MAX_PET = 65;
        private const int DOG_CRITICAL_THRESHOLD = 15;
        public const string SNOWDOG = "SnowSculptures_SnowSculpture_Snowdog";

        private bool pettable;

        [MyCmpReq]
        private KBatchedAnimController kbac;

        [MyCmpReq]
        private SnowPile sculpture;

        [MyCmpReq]
        private GlassCaseSealable sealable;


        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

            SetOffsetTable(OffsetGroups.InvertedStandardTable);
            multitoolContext = SnowBeam.CONTEXT;
            multitoolHitEffectTag = "fx_harvest_splash";
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            Subscribe((int)GameHashes.RefreshUserMenu, OnRefreshUserMenu);
            kbac.initialMode = KAnim.PlayMode.Paused;

            if(sealable.IsCased)
            {
                PutInCase(sealable.glassCase);
            }

            UpdateDog(sealable.glassCase);
        }

        public void Pet()
        {
            petCapacity++;
            UpdateDog(sealable?.glassCase);
        }

        public void PutInCase(GlassCase glassCase)
        {
            if(pettable)
            {
                kbac.Play("dog_cased", KAnim.PlayMode.Paused);
            }
        }

        public void TakeOutFromCase(GlassCase glassCase)
        {
            if (pettable)
            {
                kbac.Play("variant_9", KAnim.PlayMode.Paused);
            }
        }

        public override void SetStage(string stage_id, bool skip_effect)
        {
            base.SetStage(stage_id, skip_effect);

            if(sealable.IsCased)
            {
                PutInCase(sealable.glassCase);
            }

            UpdateDog(sealable.glassCase);
        }

        public void UpdateDog(GlassCase glassCase)
        {
            pettable = sculpture.CurrentStage == SNOWDOG;

            if(!pettable)
            {
                if(sealable != null)
                {
                    glassCase.kbac.Play("base");
                }

                kbac.SetPositionPercent(0);
                petCapacity = 0;
                broken = false;

                return;
            }

            kbac.Play("variant_9_cased", KAnim.PlayMode.Paused);

            if (petCapacity > MAX_PET)
            {
                return;
            }

            if(petCapacity == DOG_CRITICAL_THRESHOLD && !broken)
            {
                if(sealable != null)
                {
                    glassCase.kbac.Play("broken_pre", KAnim.PlayMode.Once);
                    //glassCase.kbac.Queue("broken", KAnim.PlayMode.Paused);
                }
            }
            else if(petCapacity > DOG_CRITICAL_THRESHOLD)
            {
                glassCase.kbac.Play("broken", KAnim.PlayMode.Paused);
                broken = true;
            }
            else
            {
                glassCase.kbac.Play("base", KAnim.PlayMode.Paused);
                broken = false;
            }

            kbac.SetPositionPercent((float)petCapacity / MAX_PET);
        }

        private void OnRefreshUserMenu(object obj)
        {
            if (pettable)
            {
                var button = new KIconButtonMenu.ButtonInfo("action_switch_toggle", "Pet", Pet);
                Game.Instance.userMenu.AddButton(gameObject, button);
            }
        }
    }
}
