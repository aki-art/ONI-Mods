using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Bomb
{
    public class Bomb2 : KMonoBehaviour
    {
        public int radius = 20;
        public float damage = 1f;
        public bool destroySelf = false;
        Dictionary<int, float> affectedCells;
        HashSet<int> safeCells;
        List<int> destroyedCells;

        private float innerRadius => Mathf.Floor(radius / 2);

        protected override void OnSpawn()
        {
            base.OnSpawn();
            safeCells = new HashSet<int>();
            destroyedCells = new List<int>();
            SetArea();
        }

        private void SetArea()
        {
            affectedCells = new Dictionary<int, float>();
            List<Vector2I> cells = ProcGen.Util.GetFilledCircle(transform.position, radius);

            ExplosionRayCaster.Test(cells);
            foreach (Vector2I cellPos in cells)
            {
                int cell = Grid.PosToCell(cellPos);
                float dist = Vector2.Distance(transform.position, cellPos);
                affectedCells[cell] = 1f - Mathf.Pow(dist / radius, 8);
            }
        }

        public void Explode()
        {
            SetArea();
/*            foreach(var cell in affectedCells)
            {
                DamageTile(cell.Key, cell.Value * damage);
                CastShadow(cell.Key);
            }*/
        }

        private bool isBlocking(int cell)
        {
            bool tileStanding = false;

            GameObject gameObject = Grid.Objects[cell, (int)ObjectLayer.FoundationTile];
            if(gameObject != null)
            {
                BuildingHP hp = gameObject.GetComponent<BuildingHP>();
                tileStanding = hp.HitPoints > 0;
            }

            return Grid.Damage[cell] >= 1f || tileStanding;
        }
        private void CastShadow(int cell)
        {
            if(safeCells.Contains(cell) || !isBlocking(cell))
            {
                return;
            }

            var o = transform.position;
            Vector2 t = Grid.CellToPos(cell, CellAlignment.Top, Grid.SceneLayer.Background);
            Vector2 r = Grid.CellToPos(cell, CellAlignment.Right, Grid.SceneLayer.Background);
            Vector2 b = Grid.CellToPos(cell, CellAlignment.Bottom, Grid.SceneLayer.Background);
            Vector2 l = Grid.CellToPos(cell, CellAlignment.Left, Grid.SceneLayer.Background);



            ListPool<int, LightGridManager.LightGridEmitter>.PooledList pooledList = ListPool<int, LightGridManager.LightGridEmitter>.Allocate();
            pooledList.Add(cell);


        }

        internal float DamageTile(int cell, float damage)
        {
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

            float dealtdamage = replaceElement ? 
                DamageFoundationTile(gameObject, dmg) : DamageGroundTile(cell, dmg);

            return damage * (1f - dealtdamage / dmg);
        }

        private static float DamageGroundTile(int cell, float damage)
        {
            return WorldDamage.Instance.ApplyDamage(
                cell,
                damage,
                cell,
                STRINGS.BUILDINGS.DAMAGESOURCES.EXPLOSION,
                STRINGS.UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.EXPLOSION);
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
    }
}
