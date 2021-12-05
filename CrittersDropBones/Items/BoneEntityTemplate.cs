using System.Collections.Generic;
using UnityEngine;

namespace CrittersDropBones.Items
{
    public class BoneEntityTemplate
    {
        public static GameObject CreateBone(string ID, string name, string desc, float mass, string anim, float width, float height)
        {
            GameObject prefab = EntityTemplates.CreateLooseEntity(
                ID,
                name,
                desc,
                mass,
                false,
                Assets.GetAnim(anim),
                "object",
                Grid.SceneLayer.BuildingBack,
                EntityTemplates.CollisionShape.RECTANGLE,
                width,
                height,
                true,
                0,
                SimHashes.Lime);

            prefab.GetComponent<KPrefabID>().AddTag(GameTags.Organics, false);
            //prefab.GetComponent<KPrefabID>().AddTag(ModAssets.Tags.Bone, false);
            prefab.AddOrGet<EntitySplitter>();
            prefab.AddOrGet<SimpleMassStatusItem>();
            EntityTemplates.CreateAndRegisterCompostableFromPrefab(prefab);

            return prefab;
        }
    }
}
