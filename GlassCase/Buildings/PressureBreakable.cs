using STRINGS;
using UnityEngine;

namespace GlassCase.Buildings
{
    public class PressureBreakable : KMonoBehaviour
    {
        [MyCmpGet]
        OccupyArea occupyArea;

        [MyCmpGet]
        BuildingHP HP;

        protected override void OnSpawn()
        {
            base.OnSpawn();
            //HP.SetHitPoints((int)(HP.MaxHitPoints * 0.1f));
                        
            gameObject.Trigger(-(int)GameHashes.DoBuildingDamage, new BuildingHP.DamageSourceInfo
            {
                damage = Mathf.RoundToInt(10),
                source = BUILDINGS.DAMAGESOURCES.LIQUID_PRESSURE,
                popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.LIQUID_PRESSURE
            });
        }
    }
}
