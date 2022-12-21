using PrintingPodRecharge.Content.Cmps;
using System.Collections.Generic;
using UnityEngine;

namespace PrintingPodRecharge.Content.Items
{
    public class BioInkConfig : IMultiEntityConfig
    {
        public const string DEFAULT = "PrintingPodRecharge_BioInk";
        public const string METALLIC = "PrintingPodRecharge_MetallicBioInk";
        public const string VACILLATING = "PrintingPodRecharge_VacillatingBioInk";
        public const string SEEDED = "PrintingPodRecharge_SeededBioInk";
        public const string GERMINATED = "PrintingPodRecharge_GerminatedBioInk";
        public const string FOOD = "PrintingPodRecharge_FoodBioInk";
        public const string SHAKER = "PrintingPodRecharge_ChaosBioInk";
        public const string TWITCH = "PrintingPodRecharge_TwitchBioInk";
        public const string MEDICINAL = "PrintingPodRecharge_Medicinal";

        public static Dictionary<Bundle, string> itemsToBundle = new Dictionary<Bundle, string>();

        public List<GameObject> CreatePrefabs()
        {
            return new List<GameObject>()
            {
                CreateBioInk(DEFAULT, STRINGS.ITEMS.BIO_INK.NAME, STRINGS.ITEMS.BIO_INK.DESC, "rrp_oozing_bioink_kanim", Bundle.None),
                CreateBioInk(METALLIC, STRINGS.ITEMS.METALLIC_BIO_INK.NAME, STRINGS.ITEMS.METALLIC_BIO_INK.DESC, "rrp_metallic_bioink_kanim", Bundle.Metal),
                CreateBioInk(VACILLATING, STRINGS.ITEMS.VACILLATING_BIO_INK.NAME, STRINGS.ITEMS.VACILLATING_BIO_INK.DESC, "rrp_vacillating_bioink_kanim", Bundle.SuperDuplicant),
                CreateBioInk(GERMINATED, STRINGS.ITEMS.GERMINATED_BIO_INK.NAME, STRINGS.ITEMS.GERMINATED_BIO_INK.DESC, "rrp_germinated_bioink_kanim", Bundle.Egg),
                CreateBioInk(SEEDED, STRINGS.ITEMS.SEEDED_BIO_INK.NAME, STRINGS.ITEMS.SEEDED_BIO_INK.DESC, "rrp_seedy_bioink_kanim", Bundle.Seed),
                CreateBioInk(FOOD, STRINGS.ITEMS.FOOD_BIO_INK.NAME, STRINGS.ITEMS.FOOD_BIO_INK.DESC, "rrp_food_bioink_kanim", Bundle.Food),
                CreateBioInk(SHAKER, STRINGS.ITEMS.SHAKER_BIO_INK.NAME, STRINGS.ITEMS.SHAKER_BIO_INK.DESC, "rrp_rando_bioink_kanim", Bundle.Shaker),
                CreateBioInk(TWITCH, STRINGS.ITEMS.TWITCH_BIO_INK.NAME, STRINGS.ITEMS.TWITCH_BIO_INK.DESC, "rrp_twitch_bioink_kanim", Bundle.Twitch),
                CreateBioInk(MEDICINAL, STRINGS.ITEMS.MEDICINAL_BIO_INK.NAME, STRINGS.ITEMS.MEDICINAL_BIO_INK.DESC, "rrp_medicinal_bioink_kanim", Bundle.Medicinal),
            };
        }

        public static GameObject CreateBioInk(string ID, string name, string description, string anim, Bundle bundle)
        {
            var prefab = EntityTemplates.CreateLooseEntity(
                ID,
                name,
                description,
                1f,
                false,
                Assets.GetAnim(anim),
                "object",
                Grid.SceneLayer.BuildingBack,
                EntityTemplates.CollisionShape.RECTANGLE,
                0.66f,
                0.8f,
                true,
                0,
                SimHashes.Creature,
                additionalTags: new List<Tag>
                {
                    GameTags.Organics,
                    ModAssets.Tags.bioInk,
                    GameTags.PedestalDisplayable
                });

            prefab.AddOrGet<EntitySplitter>();
            prefab.AddOrGet<SimpleMassStatusItem>();
            prefab.AddOrGet<BundleModifier>().bundle = bundle;

            itemsToBundle[bundle] = ID;

            return prefab;
        }

        public string[] GetDlcIds()
        {
            return DlcManager.AVAILABLE_ALL_VERSIONS;
        }

        public void OnPrefabInit(GameObject inst)
        {
        }

        public void OnSpawn(GameObject inst)
        {
        }
    }
}