/*using System.Collections.Generic;
using System.Linq;
using TUNING;
using UnityEngine;
using static Slag.SlagStrings.SLAGSTRINGS.ITEMS.INDUSTRIAL_INGREDIENTS;

namespace Slag.Items
{
    public class MiteShellConfig : IMultiEntityConfig
    {
        public static List<string> mitemoltItems = new List<string>();
        public List<GameObject> CreatePrefabs()
        {
            List<GameObject> shells = new List<GameObject>()
            {
                CreateShell("worthless", MITE_MOLT.WORTHLESS.NAME, MITE_MOLT.WORTHLESS.DESC )
            };
            mitemoltItems = shells.Select(m => m.name).ToList();
            return shells;
        }

        private GameObject CreateShell(string id, LocString name, LocString desc)
        {
            GameObject prefab = EntityTemplates.CreateLooseEntity(
                id: "artifact_" + id.ToLower(),
                name: name,
                desc: desc,
                mass: 25f,
                unitMass: false,
                anim: Assets.GetAnim("mitemolt_kanim"),
                initialAnim: "idle_" + id,
                sceneLayer: Grid.SceneLayer.Ore,
                collisionShape: EntityTemplates.CollisionShape.RECTANGLE,
                width: 1f,
                height: 1f,
                isPickupable: true,
                sortOrder: SORTORDER.BUILDINGELEMENTS,
                element: ModAssets.slagSimHash,
                additionalTags: new List<Tag>
                {
                    GameTags.MiscPickupable
                });

            var molt = prefab.AddComponent<MiteMolt>();
            molt.SetUIAnim("ui_" + id);


            return prefab;
        }

        public void OnPrefabInit(GameObject inst)
        {
        }

        public void OnSpawn(GameObject inst)
        {
        }
    }
}
*/