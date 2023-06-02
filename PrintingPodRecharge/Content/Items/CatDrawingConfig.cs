using System.Collections.Generic;
using UnityEngine;
using TUNING;

namespace PrintingPodRecharge.Content.Items
{
    public class CatDrawingConfig : IEntityConfig
    {
        public const string ID = "PrintingPodRecharge_CatDrawing";

        public GameObject CreatePrefab()
        {
            var prefab = EntityTemplates.CreateLooseEntity(
                ID,
                STRINGS.ITEMS.CAT_DRAWING.NAME,
                STRINGS.ITEMS.CAT_DRAWING.DESC,
                1f,
                false,
                Assets.GetAnim("rrp_catdrawing_kanim"),
                "object",
                Grid.SceneLayer.BuildingBack,
                EntityTemplates.CollisionShape.RECTANGLE,
                0.8f,
                0.9f,
                true,
                0,
                SimHashes.Creature,
                additionalTags: new List<Tag>
                {
                    GameTags.MiscPickupable,
                    GameTags.PedestalDisplayable
                });

            prefab.AddOrGet<OccupyArea>().SetCellOffsets(EntityTemplates.GenerateOffsets(1, 1));
            prefab.AddOrGet<DecorProvider>().SetValues(DECOR.BONUS.TIER1);

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
