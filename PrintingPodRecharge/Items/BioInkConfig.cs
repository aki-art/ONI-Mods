using PrintingPodRecharge.Cmps;
using System.Collections.Generic;
using UnityEngine;

namespace PrintingPodRecharge.Items
{
    public class BioInkConfig : IMultiEntityConfig
    {
        public const string DEFAULT = Mod.PREFIX + "BioInk";
        public const string OOZING = Mod.PREFIX + "OozingBioInk";
        public const string METALLIC = Mod.PREFIX + "MetallicBioInk";
        public const string FIBROUS = Mod.PREFIX + "FibrousBioInk";
        public const string VACILLATING = Mod.PREFIX + "VacillatingBioInk";
        public const string SEEDED = Mod.PREFIX + "SeededBioInk";
        public const string GERMINATED = Mod.PREFIX + "GerminatedBioInk";
        public const string FOOD = Mod.PREFIX + "FoodBioInk";

        public delegate void PostInitFn(GameObject gameObject);

        public List<GameObject> CreatePrefabs()
        {
            return new List<GameObject>()
            {
                CreateBioInk(DEFAULT, STRINGS.ITEMS.BIO_INK.NAME, STRINGS.ITEMS.BIO_INK.DESC, "rrp_bioink_kanim", ImmigrationModifier.Bundle.None),
                CreateBioInk(OOZING, STRINGS.ITEMS.OOZING_BIO_INK.NAME, STRINGS.ITEMS.OOZING_BIO_INK.DESC, "rrp_oozing_bioink_kanim", ImmigrationModifier.Bundle.Duplicant),
                CreateBioInk(METALLIC, STRINGS.ITEMS.METALLIC_BIO_INK.NAME, STRINGS.ITEMS.METALLIC_BIO_INK.DESC, "rrp_metallic_bioink_kanim", ImmigrationModifier.Bundle.Metal),
                CreateBioInk(VACILLATING, STRINGS.ITEMS.VACILLATING_BIO_INK.NAME, STRINGS.ITEMS.VACILLATING_BIO_INK.DESC, "rrp_vacillating_bioink_kanim", ImmigrationModifier.Bundle.SuperDuplicant),
                CreateBioInk(GERMINATED, STRINGS.ITEMS.GERMINATED_BIO_INK.NAME, STRINGS.ITEMS.GERMINATED_BIO_INK.DESC, "rrp_germinated_bioink_kanim", ImmigrationModifier.Bundle.Egg),
                CreateBioInk(SEEDED, STRINGS.ITEMS.SEEDED_BIO_INK.NAME, STRINGS.ITEMS.SEEDED_BIO_INK.DESC, "rrp_seedy_bioink_kanim", ImmigrationModifier.Bundle.Seed),
                CreateBioInk(FOOD, STRINGS.ITEMS.FOOD_BIO_INK.NAME, STRINGS.ITEMS.FOOD_BIO_INK.DESC, "rrp_seedy_bioink_kanim", ImmigrationModifier.Bundle.Food),
            };
        }

        public static GameObject CreateBioInk(string ID, string name, string description, string anim, ImmigrationModifier.Bundle bundle)
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
                    ModAssets.Tags.bioInk
                });

            prefab.AddOrGet<EntitySplitter>();
            prefab.AddOrGet<SimpleMassStatusItem>();
            prefab.AddOrGet<BundleModifier>().bundle = bundle;

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