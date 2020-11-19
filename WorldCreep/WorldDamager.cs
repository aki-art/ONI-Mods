using System.Collections.Generic;

namespace WorldCreep
{
    // Various utilities for wreaking havoc
    public class WorldDamager : KMonoBehaviour
    {
        public WorldDamager Instance;
        protected override void OnPrefabInit() => Instance = this;
        public void DestroyInstance() => Instance = null;

        public void DamageTile(int cell, float damage, float maxHardness = 255, bool ignoreSpecial = true)
        {

        }

        public void TryCrush(int cell)
        {
            if (IsCrushable(Grid.Element[cell], out SimHashes crushed))
            {
                SimMessages.ReplaceElement(cell, crushed, null, Grid.Mass[cell], Grid.Temperature[cell], Grid.DiseaseIdx[cell], Grid.DiseaseCount[cell]);
                Game.Instance.SpawnFX(SpawnFXHashes.BuildingLeakGas, Grid.CellToPos(cell), 0f);
            }
        }

        public void DamageBuilding(Building building, float damage, bool ignoreProtected = false)
        {

        }

        public void DamageEntity(Pickupable entity, float damage)
        {

        }

        public void TearBackwall(IEnumerable<int> cells)
        {

        }

        public void SplashLiquid(int cell)
        {

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
    }
}
