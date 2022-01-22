using System.Collections.Generic;
using UnityEngine;

namespace DecorPackB.Items
{
    public class FossilNoduleConfig : IEntityConfig
    {
        public static string ID = Mod.PREFIX + "FossilNodule";

        public GameObject CreatePrefab()
        {
            return EntityTemplates.CreateLooseEntity(
                   ID,
                   STRINGS.ITEMS.DECORPACKB_FOSSILNODULE.NAME,
                   STRINGS.ITEMS.DECORPACKB_FOSSILNODULE.DESC,
                   1f,
                   false,
                   Assets.GetAnim("dp_fossil_nodule_kanim"),
                   "object",
                   Grid.SceneLayer.Front,
                   EntityTemplates.CollisionShape.RECTANGLE,
                   0.8f,
                   0.33f,
                   true,
                   0,
                   SimHashes.Lime,
                   new List<Tag>
                   {
                       ModAssets.Tags.FossilNodule,
                       GameTags.PedestalDisplayable
                   });
        }

        public string[] GetDlcIds()
        {
            return DlcManager.AVAILABLE_ALL_VERSIONS;
        }

        public void OnPrefabInit(GameObject inst) { }

        public void OnSpawn(GameObject inst) { }
    }
}
