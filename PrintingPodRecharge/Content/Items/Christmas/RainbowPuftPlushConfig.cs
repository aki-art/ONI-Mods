using System.Collections.Generic;
using UnityEngine;

namespace PrintingPodRecharge.Content.Items.Christmas
{
    public class RainbowPuftPlushConfig : IEntityConfig
    {
        public const string ID = "PrintingPodRecharge_RainbowPuftPlush";

        public GameObject CreatePrefab()
        {
            var prefab = EntityTemplates.CreateLooseEntity(
                ID, 
                "Glitter Puft Plushie", 
                "It farts glitter!", 
                25f, 
                true,
                Assets.GetAnim("rpp_rainbowpufplush_kanim"), 
                "object", 
                Grid.SceneLayer.Ore,
                EntityTemplates.CollisionShape.RECTANGLE, 
                1f, 
                1f,
                true, 0, 
                SimHashes.Unobtanium, 
                new List<Tag>
                {
                    GameTags.MiscPickupable,
                    GameTags.PedestalDisplayable
                });

            var occupyArea = prefab.AddOrGet<OccupyArea>();
            occupyArea.OccupiedCellsOffsets = EntityTemplates.GenerateOffsets(1, 1);

            var decorProvider = prefab.AddOrGet<DecorProvider>();
            decorProvider.SetValues(TUNING.DECOR.BONUS.TIER4);
            decorProvider.overrideName = prefab.name;

            prefab.AddOrGet<GlitterToot>();

            return prefab;
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

        public void OnPrefabInit(GameObject inst)
        {
        }

        public void OnSpawn(GameObject inst)
        {
        }
    }
}
