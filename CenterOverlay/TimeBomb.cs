using FMOD.Studio;
using UnityEngine;

namespace CenterOverlay
{
    // unimplemented/unfinished. would blow up the building its on eventually
    class TimeBomb : KMonoBehaviour, ISim1000ms
    {
        private const int damagePerSecond = 2;
        public void Sim1000ms(float dt)
        {
            BuildingHP hp = gameObject.GetComponent<BuildingHP>();

            if(hp.HitPoints >= 3)
            {
                hp.gameObject.Trigger((int)GameHashes.DoBuildingDamage, new BuildingHP.DamageSourceInfo
                {
                    damage = damagePerSecond,
                    source = "Wrongside",
                    popString = "Wrong side"
                });
            }
            else
            {
                var position = transform.GetPosition();
                Game.Instance.SpawnFX(SpawnFXHashes.MeteorImpactMetal, position, 0f); 
                hp.gameObject.Trigger((int)GameHashes.DoBuildingDamage, new BuildingHP.DamageSourceInfo
                {
                    damage = 9999,
                    source = "Wrongside",
                    popString = "Wrong side"
                });

                PlayImpactSound(position);
            }
        }
        private void PlayImpactSound(Vector3 pos)
        {
            string sound = GlobalAssets.GetSound("Meteor_Large_Impact");
            if (CameraController.Instance.IsAudibleSound(pos, sound))
            {
                float volume = 3;
                pos.z = 0f;
                EventInstance instance = KFMOD.BeginOneShot(sound, pos, volume);
                instance.setParameterValue("userVolume_SFX", KPlayerPrefs.GetFloat("Volume_SFX"));
                KFMOD.EndOneShot(instance);
            }
        }
    }
}