using FMOD.Studio;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace InteriorDecorationVolI.Buildings.Aquarium
{
    class FishYeeterTank : KMonoBehaviour, ISidescreenButtonControl
    {
        private KBatchedAnimController effect;
        public string SidescreenTitleKey => "Addlater";

        public string SidescreenStatusMessage => null;

        public string SidescreenButtonText => "Release fish";

        public void OnSidescreenButtonPressed()
        {
            var position = gameObject.transform.GetPosition();
            position.y += 0.75f;
            Game.Instance.SpawnFX(SpawnFXHashes.MeteorImpactMetal, position, 0f);		
            
            BuildingHP component2 = gameObject.GetComponent<BuildingHP>();
            component2.gameObject.Trigger(-794517298, new BuildingHP.DamageSourceInfo
            {
                damage = 9999,
                source = BUILDINGS.DAMAGESOURCES.COMET,
                popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.COMET
            });

            effect.gameObject.SetActive(false);
            LaunchSlicksters(position);

        }

        private void LaunchSlicksters(Vector3 position)
        {

            GameObject prefab = Assets.GetPrefab(OilFloaterConfig.ID);

            for (int i = 0; i < 15; i++)
            {

                var fish = GameUtil.KInstantiate(prefab, position, Grid.SceneLayer.Creatures, null, 0);
                fish.SetActive(true);
                Vector2 vector2 = new Vector3(UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(0f, 3f), 0f).normalized;
                vector2 += new Vector2(0f, UnityEngine.Random.Range(0f, 1f));
                vector2 *= UnityEngine.Random.Range(8f, 14f);
                GameComps.Fallers.Add(fish, vector2);
            }
            PlayImpactSound(position);
        }
        private void PlayImpactSound(Vector3 pos)
        {
            string impactSound = "Meteor_Large_Impact";
            string sound = GlobalAssets.GetSound(impactSound, false);
            if (CameraController.Instance.IsAudibleSound(pos, sound))
            {
                float volume = 3;
                pos.z = 0f;
                EventInstance instance = KFMOD.BeginOneShot(sound, pos, volume);
                instance.setParameterValue("userVolume_SFX", KPlayerPrefs.GetFloat("Volume_SFX"));
                KFMOD.EndOneShot(instance);
            }
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            Debug.Log("effect is being spawned");
            var position = gameObject.transform.GetPosition();
            position.y += 1.25f;
            effect = FXHelpers.CreateEffect("pacu_kanim", position, gameObject.transform, true, Grid.SceneLayer.Front, false);
            effect.destroyOnAnimComplete = false;
            effect.Play("idle_loop", KAnim.PlayMode.Loop);
            effect.animScale *= 0.75f;
            effect.gameObject.SetActive(true);
            Debug.Log("effect spawned at " + gameObject.transform.GetPosition().x + " " + gameObject.transform.GetPosition().y);
            if (effect == null) Debug.Log("effect is null");
        }
    }
}
