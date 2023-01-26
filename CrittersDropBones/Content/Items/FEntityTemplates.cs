using System.Collections.Generic;
using UnityEngine;

namespace CrittersDropBones.Content.Items
{
    public class FEntityTemplates
    {
        public static GameObject CreateSoup(string ID, string name, string desc, string anim)
        {
            return EntityTemplates.CreateLooseEntity(
                ID,
                name,
                desc,
                1f,
                false,
                Assets.GetAnim(anim),
                "object",
                Grid.SceneLayer.Front,
                EntityTemplates.CollisionShape.RECTANGLE,
                0.8f,
                0.5f,
                true,
                0,
                SimHashes.Creature,
                null);
        }

        public static GameObject CreateBone(string ID, string name, string desc, float mass, string anim, float width, float height)
        {
            var prefab = EntityTemplates.CreateLooseEntity(
                ID,
                name,
                desc,
                mass,
                true,
                Assets.GetAnim(anim),
                "object",
                Grid.SceneLayer.BuildingBack,
                EntityTemplates.CollisionShape.RECTANGLE,
                width,
                height,
                true,
                0,
                SimHashes.Lime,
                additionalTags: new List<Tag>
                {
                    GameTags.Organics,
                    ModAssets.Tags.Bones
                });

            //prefab.GetComponent<KPrefabID>().AddTag(GameTags.Organics, false);
            //prefab.GetComponent<KPrefabID>().AddTag(ModAssets.Tags.Bone, false);
            prefab.AddOrGet<EntitySplitter>();
            prefab.AddOrGet<SimpleMassStatusItem>();
            EntityTemplates.CreateAndRegisterCompostableFromPrefab(prefab);

            return prefab;
        }
    }
}
