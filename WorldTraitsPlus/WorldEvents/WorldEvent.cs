using Harmony;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;
using WorldTraitsPlus.Traits.WorldEvents;

namespace WorldTraitsPlus.WorldEvents
{
    public class WorldEvent : KMonoBehaviour
    {
        [SerializeField]
        public bool randomizeLocation = true;
        [SerializeField]
        public bool immediateStart = true;
        [SerializeField]
        public float power = -1;
        [SerializeField]
        public float duration = -1;
        [SerializeField]
        public Dictionary<int, float> affectedCells;

        public float elapsedTime = 0;
        public float progress => Math.Min(elapsedTime / duration, 1f);
        public Stage stage;
        public bool hasStarted = false;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            affectedCells = new Dictionary<int, float>();
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            stage = Stage.Waiting;
            WorldEventManager.Instance.activeEvents.Add(this);
            Trigger((int)WorldEventHashes.WorldEventScheduled, this);

            if (immediateStart)
            {
                Begin();
            }
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();
            WorldEventManager.Instance.activeEvents.Remove(this);
        }

        protected void RandomizePower(MathUtil.MinMax range)
        {
            float max = Math.Min(range.max, SeismicGrid.highestActivity);
            float min = range.min;

            power = Mathf.Abs(Util.GaussianRandom(0, (max - min) / 2.5f) + min);
            power = Mathf.Clamp(power, min, max);
        }

        public void SetPower(float value)
        {
            value = Mathf.Clamp(value, 0, 100);
            power = value;
        }

        public virtual void SetCells() { }

        public virtual void Begin() 
        {
            stage = Stage.Ongoing;
            hasStarted = true;
            Trigger((int)WorldEventHashes.WorldEventStarted, this);
        }

        public virtual void End(bool triggerEvent = true)
        {
            stage = Stage.Finished;
            if(triggerEvent)
            {
                Trigger((int)WorldEventHashes.WorldEventEnded, this);
            }
        }


        private bool IsCrushable(Element element, out SimHashes crushed)
        {
            crushed = SimHashes.CrushedRock;

            switch (element.id)
            {
                case SimHashes.Ice:
                    crushed = SimHashes.CrushedIce;
                    return true;
                case SimHashes.CrushedIce:
                    crushed = SimHashes.Snow;
                    return true;
                case SimHashes.CrushedRock:
                    crushed = SimHashes.Sand;
                    return true;
                default:
                    return element.HasTag(GameTags.Crushable) && element.hardness < 100;
            }
        }

        internal float DamageTile(int cell, float inputDamage, bool crushing = false, float crushChance = 1f, bool spawnFX = true)
        {
            if (!Grid.IsValidCell(cell) || Grid.Element[cell].id == SimHashes.Unobtanium) return 0;
            GameObject gameObject = Grid.Objects[cell, (int)ObjectLayer.FoundationTile];
            float damageMultiplier = 1f;
            bool replaceElement = false;

            if (gameObject != null)
            {
                if (gameObject.GetComponent<KPrefabID>().HasTag(GameTags.Window))
                {
                    damageMultiplier = 2f;
                }

                var sco = gameObject.GetComponent<SimCellOccupier>();
                replaceElement = sco != null && !sco.doReplaceElement;
            }

            Element element;
            if (replaceElement)
            {
                element = gameObject.GetComponent<PrimaryElement>().Element;
            }
            else
            {
                element = Grid.Element[cell];
                if (crushing && IsCrushable(element, out SimHashes crushed) && UnityEngine.Random.value <= crushChance)
                {
                    SimMessages.ReplaceElement(cell, crushed, null, Grid.Mass[cell], Grid.Temperature[cell], Grid.DiseaseIdx[cell], Grid.DiseaseCount[cell]);
                    Game.Instance.SpawnFX(SpawnFXHashes.BuildingLeakGas, Grid.CellToPos(cell), 0f);
                    return 0;
                }
            }

            if (element.strength == 0f) return 0f;

            float damage = inputDamage * damageMultiplier / element.strength;
            PlayTileDamageSound(element, Grid.CellToPos(cell));
            Game.Instance.SpawnFX(SpawnFXHashes.BuildingLeakGas, Grid.CellToPos(cell), 0f);

            if (damage == 0f) return 0f;

            float dealtdamage;
            if (replaceElement)
            {
                BuildingHP hp = gameObject.GetComponent<BuildingHP>();
                float a = hp.HitPoints / (float)hp.MaxHitPoints;
                float f = damage * hp.MaxHitPoints;
                hp.gameObject.Trigger((int)GameHashes.DoBuildingDamage, new BuildingHP.DamageSourceInfo
                {
                    damage = Mathf.RoundToInt(f),
                    source = BUILDINGS.DAMAGESOURCES.COMET,
                    popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.COMET
                });
                dealtdamage = Mathf.Min(a, damage);
            }
            else
            {
                dealtdamage = WorldDamage.Instance.ApplyDamage(
                    cell, damage, cell, STRINGS.BUILDINGS.DAMAGESOURCES.EARTHQUAKE, STRINGS.UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.EARTHQUAKE);
            }

            //destroyedCells.Add(cell);
            float what = dealtdamage / damage;
            return inputDamage * (1f - what);
        }

        private void PlayTileDamageSound(Element element, Vector3 pos)
        {
            string text = Traverse.Create(element.substance).Method("GetMiningBreakSound").GetValue<string>();
            if (text == null)
            {
                if (element.HasTag(GameTags.RefinedMetal))
                {
                    text = "RefinedMetal";
                }
                else if (element.HasTag(GameTags.Metal))
                {
                    text = "RawMetal";
                }
                else
                {
                    text = "Rock";
                }
            }

            text = GlobalAssets.GetSound("MeteorDamage_" + text);
            if (CameraController.Instance && CameraController.Instance.IsAudibleSound(pos, text))
            {
                KFMOD.PlayOneShot(text, CameraController.Instance.GetVerticallyScaledPosition(pos), 0.7f);
            }
        }


        public enum Stage
        {
            Waiting,
            Ongoing,
            Finished
        }
    }
}
