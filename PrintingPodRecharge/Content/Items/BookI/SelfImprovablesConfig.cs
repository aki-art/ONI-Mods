using System.Collections.Generic;
using UnityEngine;

namespace PrintingPodRecharge.Content.Items.BookI
{
    public class SelfImprovablesConfig : IMultiEntityConfig
    {
        public const string BOOK_VOL1 = "PrintingPodRecharge_BookOfSelfImprovement";
        public const string BOOK_VOL2 = "PrintingPodRecharge_BookOfSelfImprovementVol2";
        public const string D8 = "PrintingPodRecharge_D8";
        public const string D4 = "PrintingPodRecharge_D4";
        public const string D100 = "PrintingPodRecharge_D100";
        public const string DUMBBELL = "PrintingPodRecharge_Dumbbell";
        public const string MANGA = "PrintingPodRecharge_Manga";

        public List<GameObject> CreatePrefabs()
        {
            return new List<GameObject>()
            {
                CreatePrefab<BookVolumeI>(BOOK_VOL1, "rrp_book_of_self_improvement_kanim", STRINGS.ITEMS.BOOK_OF_SELF_IMPROVEMENT.NAME, STRINGS.ITEMS.BOOK_OF_SELF_IMPROVEMENT.DESC),
                CreatePrefab<BookVolumeII>(BOOK_VOL2, "rrp_book_of_self_improvement_vol2_kanim", STRINGS.ITEMS.BOOK_OF_SELF_IMPROVEMENT_VOL2.NAME, STRINGS.ITEMS.BOOK_OF_SELF_IMPROVEMENT_VOL2.DESC),
                CreatePrefab<Manga>(MANGA, "rrp_manga_kanim", STRINGS.ITEMS.MANGA.NAME, STRINGS.ITEMS.MANGA.DESC, 0.75f),
                CreatePrefab<D8>(D8, "rpp_d8_kanim", STRINGS.ITEMS.D8.NAME, STRINGS.ITEMS.D8.DESC, 0.75f),
            };
        }

        public GameObject CreatePrefab<T>(string ID, string anim, string name, string desc, float height = 0.9f, float width = 0.8f) where T : SelfImprovement
        {
            var prefab = EntityTemplates.CreateLooseEntity(
                ID,
                name,
                desc,
                1f,
                false,
                Assets.GetAnim(anim),
                "object",
                Grid.SceneLayer.BuildingBack,
                EntityTemplates.CollisionShape.RECTANGLE,
                width,
                height,
                true,
                additionalTags: new List<Tag>
                {
                    GameTags.MiscPickupable,
                    GameTags.PedestalDisplayable,
                    GameTags.NotRoomAssignable
                });

            prefab.AddComponent<T>();

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
