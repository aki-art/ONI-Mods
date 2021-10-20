using FMOD.Studio;
using FUtility;
using KSerialization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WorldTraitsPlus.WorldEvents
{
    [SerializationConfig(MemberSerialization.OptIn)]
    class EarthQuake : WorldEvent, ISim200ms
    {
        EventInstance soundEventInstance;
        float effectivePower = 0f;
        string soundEffect;
        float maximumTileDamage = 1000f;
        float maximumBuildingDamage = 1000f;
        readonly float breakBlocksTreshold = 0.4f;
        readonly float breakBuildingsTreshold = 0.4f;
        readonly float spawnGeyserTreshold = 0.8f;
        readonly int destructionAttempts = 20;
        bool canSpawnGeyser = true;
        int radius;
        int falloff;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            soundEffect = GlobalAssets.GetSound("meteor_lp");
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            if (power == -1)
                RandomizePower(ModAssets.settings.EarthquakeMagnitude);
            maximumTileDamage *= power;
            maximumBuildingDamage *= power;

            if (duration == -1)
                duration = ModAssets.settings.EarthquakeDuration.Get() * power;

            if (randomizeLocation)
            {
                canSpawnGeyser = SeismicGrid.FindAppropiateEpicenter(power, spawnGeyserTreshold, out int epicenter);
                transform.position = Grid.CellToPos(epicenter);
            }

            radius = 30;
            falloff = 5;
            if (affectedCells == null || affectedCells.Count == 0)
                affectedCells = SeismicGrid.GetCircle(this, radius - falloff, falloff);

            Log.Debuglog($"New earthquake: tremors starting in {WorldEventManager.Instance.TimeUntilNextEvent}s" +
                $", at a magnitude of {power * 10}, affecting {affectedCells.Count} cells");
        }

        public override void Begin()
        {
            base.Begin();
            StartLoopingSound();
            StartCoroutine(ShakeCamera());
        }

        public void Sim200ms(float dt)
        {
            if (effectivePower == 0 || !hasStarted) return;

            BreakBlocks();
            BreakBuildings();
            SpawnGeyser();
        }

        private void SpawnGeyser()
        {
            if (effectivePower >= spawnGeyserTreshold && canSpawnGeyser)
            {
                List<string> geyserPrefabs = Assets.GetPrefabsWithComponent<Geyser>()
                    .Select(p => p.PrefabID().ToString())
                    .ToList();

                geyserPrefabs.Remove(OilWellConfig.ID);
                StampGeyserTemplate(geyserPrefabs.GetRandom());

                Game.Instance.SpawnFX(SpawnFXHashes.MeteorImpactDust, transform.position, 0f);
                PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, "Eruption", transform, 1.5f, false);
                canSpawnGeyser = false;
            }
        }

        private void StampGeyserTemplate(string prefabName)
        {
            TemplateContainer template = TemplateCache.GetTemplate("poi/geyser_basic");
            template.otherEntities.Find(e => e.id == "GeyserGeneric_steam").id = prefabName;
            ClearCellsForTemplate(template);
            TemplateLoader.Stamp(template, transform.position, null);
        }

        private void ClearCellsForTemplate(TemplateContainer template)
        {
            foreach (var templateCell in template.cells)
            {
                Grid.CellToXY(Grid.PosToCell(this), out int x, out int y);
                int cell = Grid.XYToCell(templateCell.location_x + x, templateCell.location_y + y);
                if (Grid.IsValidCell(cell))
                    ClearCell(cell);
            }
        }

        private void ClearCell(int cell)
        {
            DamageTile(cell, 9999);

            GameObject gameObject = Grid.Objects[cell, (int)ObjectLayer.Building];
            if (gameObject != null)
            {
                var deconstructable = gameObject.GetComponent<Deconstructable>();
                if(deconstructable != null)
                { 
                    deconstructable.SpawnItemsFromConstruction();
                    gameObject.DeleteObject();
                }
            }

            SimMessages.ReplaceElement(cell, SimHashes.Vacuum, null, 0);
        }

        private void BreakBuildings()
        {
            if (effectivePower >= breakBuildingsTreshold)
            {

            }
        }

        private void BreakBlocks()
        {
            if (effectivePower >= breakBlocksTreshold)
            {
                int attempts = Mathf.CeilToInt(destructionAttempts * effectivePower);
                for (int i = 0; i < attempts; i++)
                {
                    int targetCell = affectedCells.GetRandomKey();
                    float diff = effectivePower - affectedCells[targetCell];
                    if (Random.value > diff)
                    {
                        DamageTile(targetCell, 0.25f * effectivePower);
                    }
                }
            }
        }


        public override void End(bool triggerEvent = true)
        {
            base.End();
            effectivePower = 0;
            soundEventInstance.stop(STOP_MODE.ALLOWFADEOUT);
            SoundEvent.EndOneShot(soundEventInstance);
            StopAllCoroutines();
        }

        private void StartLoopingSound()
        {
            soundEventInstance = KFMOD.BeginOneShot(soundEffect, gameObject.transform.GetPosition(), 2f);
            soundEventInstance.setParameterValue("userVolume_SFX", KPlayerPrefs.GetFloat("Volume_SFX"));
            soundEventInstance.setPitch(0.7f);
            soundEventInstance.setProperty(EVENT_PROPERTY.MAXIMUM_DISTANCE, 150f);
            if (soundEventInstance.isValid())
            {
                SoundEvent.EndOneShot(soundEventInstance);
            }
        }

        private IEnumerator ShakeCamera()
        {
            PerlinNoise perlin = new PerlinNoise(Random.Range(1, 999));
            while (stage != Stage.Finished)
            {
                elapsedTime += Time.deltaTime;

                if (progress >= 1)
                {
                    End();
                    continue;
                }

                float noiseScale = 0.5f;
                float xScale = 1.5f;
                float frequency = 7;

                float t = progress < 0.5f ? progress * 2 : (1 - progress) * 2;
                float x = (float)perlin.Noise(elapsedTime * frequency * xScale, 0, 0);
                float y = (float)perlin.Noise(0, elapsedTime * frequency, 0);

                Vector3 offset = new Vector3(x, y) * t * noiseScale;
                Vector3 currentPos = CameraController.Instance.transform.GetPosition();

                CameraController.Instance.SetPosition(currentPos + offset);
                soundEventInstance.setVolume(2 * t);
                effectivePower = power * t;
                // vignette.SetIntensity(t);

                yield return new WaitForSeconds(0.005f);
            }

            yield return null;
        }
    }
}
