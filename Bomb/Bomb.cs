using FMOD.Studio;
using Harmony;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;
using static ProcGen.SubWorld;

namespace Bomb
{
    public class Bomb : KMonoBehaviour, ISingleSliderControl
    {
        public float radius = 200f;
        public float damage = 10f;

        public string sound = "Meteor_Large_Impact";
        public SpawnFXHashes spawnFXHash = SpawnFXHashes.MeteorImpactMetal;

        public bool damagesBackWall;
        public bool shakesCamera;

        public SimHashes smokeElement = SimHashes.CarbonDioxide;
        public bool leavesSmoke = true;
        public float smokeTemp = 500f;
        public float smokeMass = 30f;

        public bool destroySelf = true;

        public void Explode()
        {
            var area = ProcGen.Util.GetFilledCircle(transform.position, radius);
            area.ForEach(c => DamageTile(c));

            PlayImpactSound(transform.position, sound);
            Game.Instance.SpawnFX(spawnFXHash, transform.position, 0f);

            SpawnSmoke();
            BreakBackWall();
            DestroySelf();
        }

        private void DestroySelf()
        {
            if (destroySelf)
            {
                Util.KDestroyGameObject(this);
            }
        }

        private void SpawnSmoke()
        {
            if (leavesSmoke)
            {
                SimMessages.ReplaceElement(Grid.PosToCell(this), smokeElement, null, smokeMass, smokeTemp);
            }
        }

        private void PlayImpactSound(Vector3 pos, string impactSound)
        {
            string sound = GlobalAssets.GetSound(impactSound, false);
            if (CameraController.Instance.IsAudibleSound(pos, sound))
            {
                pos.z = 0f;
                EventInstance instance = KFMOD.BeginOneShot(sound, pos);
                instance.setParameterValue("userVolume_SFX", KPlayerPrefs.GetFloat("Volume_SFX"));
                KFMOD.EndOneShot(instance);
            }
        }

        internal float DamageTile(Vector2I cellPos)
        {
            int cell = Grid.PosToCell(cellPos);
            if (!Grid.IsValidCell(cell) || Grid.Element[cell].id == SimHashes.Unobtanium)
            {
                return 0;
            }

            GameObject gameObject = Grid.Objects[cell, (int)ObjectLayer.FoundationTile];
            float damageMultiplier = 1f;
            bool replaceElement = false;

            if (gameObject != null)
            {
                damageMultiplier = GetDamageMultiplier(gameObject, damageMultiplier);
                replaceElement = ShouldReplaceElement(gameObject);
            }

            Element element = GetElementForCell(cell, gameObject, replaceElement);
            if (element.strength == 0f)
            {
                return 0;
            }

            float dmg = damage * damageMultiplier / element.strength;
            PlayTileDamageSound(element, Grid.CellToPos(cell));

            if (dmg == 0f) return 0f;

            float dealtdamage = replaceElement ? DamageFoundationTile(gameObject, dmg) : DamageGroundTile(cell, dmg);
            return damage * (1f - dealtdamage / dmg);
        }

        private static float DamageGroundTile(int cell, float damage)
        {
            return WorldDamage.Instance.ApplyDamage(
                cell, damage, cell, STRINGS.BUILDINGS.DAMAGESOURCES.EXPLOSION, STRINGS.UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.EXPLOSION);
        }

        private static float DamageFoundationTile(GameObject gameObject, float damage)
        {
            BuildingHP hp = gameObject.GetComponent<BuildingHP>();

            hp.gameObject.Trigger((int)GameHashes.DoBuildingDamage, new BuildingHP.DamageSourceInfo
            {
                damage = Mathf.RoundToInt(damage * hp.MaxHitPoints),
                source = STRINGS.BUILDINGS.DAMAGESOURCES.EXPLOSION,
                popString = STRINGS.UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.EXPLOSION
            });

            float dealtdamage = Mathf.Min(hp.HitPoints / (float)hp.MaxHitPoints, damage);
            return dealtdamage;
        }

        private static Element GetElementForCell(int cell, GameObject gameObject, bool replaceElement)
        {
            return replaceElement ? gameObject.GetComponent<PrimaryElement>().Element : Grid.Element[cell];
        }

        private static bool ShouldReplaceElement(GameObject gameObject)
        {
            var sco = gameObject.GetComponent<SimCellOccupier>();
            return sco != null && !sco.doReplaceElement;
        }

        private static float GetDamageMultiplier(GameObject gameObject, float damageMultiplier)
        {
            if (gameObject.HasTag(GameTags.Window))
            {
                damageMultiplier = 5f;
            }
            else if (gameObject.HasTag(GameTags.Bunker))
            {
                damageMultiplier = 0.2f;
            }

            return damageMultiplier;
        }

        private void PlayTileDamageSound(Element element, Vector3 pos)
        {
            string sound = Traverse.Create(element.substance).Method("GetMiningBreakSound").GetValue<string>();
            if (sound == null)
            {
                if (element.HasTag(GameTags.RefinedMetal))
                {
                    sound = "RefinedMetal";
                }
                else if (element.HasTag(GameTags.Metal))
                {
                    sound = "RawMetal";
                }
                else
                {
                    sound = "Rock";
                }
            }

            sound = GlobalAssets.GetSound("MeteorDamage_" + sound);
            if (CameraController.Instance && CameraController.Instance.IsAudibleSound(pos, sound))
            {
                KFMOD.PlayOneShot(sound, CameraController.Instance.GetVerticallyScaledPosition(pos), 0.7f);
            }
        }

        private void BreakBackWall()
        {
            if (damagesBackWall)
            {
                int r = Mathf.CeilToInt(radius / 2);
                if (r <= 1)
                {
                    ExposeToSpace(new List<int>() { Grid.PosToCell(this) });
                }
                else
                {
                    var cells = new List<int>();
                    for (int i = 0; i <  r * r; i++)
                    {
                        Vector3 chosenCell = Random.insideUnitCircle * r;
                        int cell = Grid.PosToCell(chosenCell + transform.position);
                        if (Grid.IsValidCell(cell) && World.Instance.zoneRenderData.GetSubWorldZoneType(cell) != ZoneType.Space)
                        {
                            cells.Add(cell);
                        }
                    }

                    if (cells.Count > 0)
                    {
                        ExposeToSpace(cells);
                    }
                }
            }
        }

        public void ExposeToSpace(List<int> cells)
        {
            foreach (int cell in cells)
            {
                SimMessages.ModifyCellWorldZone(cell, byte.MaxValue);
            }

            RegenerateBackwallTexture(cells);
        }

        private void RegenerateBackwallTexture(List<int> cells)
        {
            var zoneRenderData = Traverse.Create(World.Instance.zoneRenderData);
            var colourTexField = zoneRenderData.Field("colourTex");
            var indexTexField = zoneRenderData.Field("indexTex");

            Texture2D colourTex = colourTexField.GetValue<Texture2D>();
            Texture2D indexTex = indexTexField.GetValue<Texture2D>();

            byte[] zoneIndices = colourTex.GetRawTextureData();
            byte[] colors = indexTex.GetRawTextureData();

            foreach (var cell in cells)
            {
                Color32 color = World.Instance.zoneRenderData.zoneColours[(int)ZoneType.Space];
                colors[cell] = byte.MaxValue;

                zoneIndices[cell * 3] = color.r;
                zoneIndices[cell * 3 + 1] = color.g;
                zoneIndices[cell * 3 + 2] = color.b;

                World.Instance.zoneRenderData.worldZoneTypes[cell] = ZoneType.Space;
            }

            colourTex.LoadRawTextureData(zoneIndices);
            indexTex.LoadRawTextureData(colors);

            colourTex.Apply();
            indexTex.Apply();

            colourTexField.SetValue(colourTex);
            indexTexField.SetValue(indexTex);

            zoneRenderData.Method("OnShadersReloaded").GetValue();
            //zoneRenderData.Method("InitSimZones", colors).GetValue();
        }


        public string SliderTitleKey => "title";
        public string SliderUnits => UI.UNITSUFFIXES.UNIT;
        public int SliderDecimalPlaces(int index) => 0;

        public float GetSliderMin(int index) => 1;

        public float GetSliderMax(int index) => 255;

        public float GetSliderValue(int index) => radius;

        public void SetSliderValue(float percent, int index) => radius = percent;

        public string GetSliderTooltipKey(int index) => "tooltipkey";

        public string GetSliderTooltip() => "tooltipkey";

    }
}
