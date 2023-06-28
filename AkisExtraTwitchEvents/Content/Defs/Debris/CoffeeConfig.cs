using UnityEngine;

namespace Twitchery.Content.Defs.Debris
{
    public class CoffeeConfig : IOreConfig
    {
        public SimHashes ElementID => Elements.Coffee;

        public GameObject CreatePrefab()
        {
            var info = new MedicineInfo(
                ElementID.ToString(),
                TEffects.CAFFEINATED,
                MedicineInfo.MedicineType.Booster,
                null); // TODO sleepy curse

            var prefab = EntityTemplates.CreateLiquidOreEntity(ElementID);
            EntityTemplates.ExtendEntityToMedicine(prefab, info);

            return prefab;
        }
    }
}
