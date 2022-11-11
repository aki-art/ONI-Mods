using KSerialization;
using System;

namespace SnowSculptures.Content.Buildings
{
    public class SnowPile : Artable
    {
        private const int MAX_PET = 65;
        private const int DOG_CRITICAL_THRESHOLD = 15;
        private const string SNOWDOG = "SnowSculptures_SnowSculpture_Snowdog";

        [Serialize]
        public int petCapacity;

        private bool pettable;

        [MyCmpReq]
        private KBatchedAnimController kbac;

        [MyCmpReq]
        private SnowPile sculpture;

        [MyCmpReq]
        private GlassCaseSealable sealable;

        [MyCmpReq]
        private Rotatable rotatable;

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
            Subscribe((int)GameHashes.Rotated, OnRotated);
            Subscribe(SnowHashes.Sealed, data => PutInCase(data as GlassCase));
            Subscribe(SnowHashes.UnSealed, data => TakeOutFromCase(data as GlassCase));
        }

        private void OnRotated(object data)
        {
            if(sealable.glassCase != null)
            {
                sealable.glassCase.UpdateAnimation(rotatable.IsRotated);
            }
        }

        public void Pet()
        {
            petCapacity++;
            UpdateDog();
            UpdateBroken();
        }

        public void PutInCase(GlassCase glassCase)
        {
            if(glassCase == null)
            {
                return;
            }

            if(pettable)
            {
                kbac.Play("dog_cased", KAnim.PlayMode.Paused);
            }

            UpdateDog();
            UpdateBroken();
        }

        public void TakeOutFromCase(GlassCase _)
        {
            if (pettable)
            {
                kbac.Play("variant_9", KAnim.PlayMode.Paused);
            }

            UpdateDog();
            UpdateBroken();
        }

        private void UpdateBroken()
        {
            if (sealable.glassCase != null)
            {
                sealable.glassCase.ToggleBroken(pettable && petCapacity >= DOG_CRITICAL_THRESHOLD, rotatable.IsRotated);
            }
        }

        public override void SetStage(string stage_id, bool skip_effect)
        {
            base.SetStage(stage_id, skip_effect);

            if(sealable.IsCased)
            {
                PutInCase(sealable.glassCase);
            }

            UpdateDog();
            UpdateBroken();
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

            if(petCapacity < DOG_CRITICAL_THRESHOLD && petCapacity >= DOG_CRITICAL_THRESHOLD - 2)
            {
                AudioUtil.PlaySound(ModAssets.Sounds.CUICA_DRUM, transform.position, KPlayerPrefs.GetFloat("Volume_SFX"));
            }

            kbac.Play("variant_9_cased", KAnim.PlayMode.Paused);
            kbac.SetPositionPercent((float)petCapacity / MAX_PET);
        }

        private void OnRefreshUserMenu(object obj)
        {
            if (pettable)
            {
                var button = new KIconButtonMenu.ButtonInfo("action_switch_toggle", "Pet", Pet, is_interactable: petCapacity < MAX_PET);
                Game.Instance.userMenu.AddButton(gameObject, button);
            }
        }
    }
}
