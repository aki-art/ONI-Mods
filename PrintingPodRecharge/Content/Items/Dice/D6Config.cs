using PrintingPodRecharge.Content.Cmps;
using System.Collections.Generic;
using UnityEngine;

namespace PrintingPodRecharge.Content.Items.Dice
{
    public class D6Config : IEntityConfig
    {
        public const string ID = "PrintingPodRecharge_D6";

        public GameObject CreatePrefab()
        {
            var prefab = EntityTemplates.CreateLooseEntity(
                ID,
                STRINGS.ITEMS.D6.NAME,
                STRINGS.ITEMS.D6.DESC,
                1f,
                false,
                Assets.GetAnim("rpp_d6_kanim"),
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
            prefab.AddOrGet<DecorProvider>().SetValues(TUNING.DECOR.BONUS.TIER1);

            return prefab;
        }

        public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

        public void OnPrefabInit(GameObject inst)
        {
        }

        public void OnSpawn(GameObject inst)
        {
            BioInksD6Manager.Instance.AddDie(1);
            PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, STRINGS.ITEMS.D6.NAME, null, inst.transform.position);
            Util.KDestroyGameObject(inst);
        }
    }
}
