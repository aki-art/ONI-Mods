using FUtility;
using PrintingPodRecharge.Content.Cmps;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PrintingPodRecharge.Content.Items
{
	public class BioInkConfig : IMultiEntityConfig
	{
		public const string
			DEFAULT = "PrintingPodRecharge_BioInk",
			METALLIC = "PrintingPodRecharge_MetallicBioInk",
			VACILLATING = "PrintingPodRecharge_VacillatingBioInk",
			SEEDED = "PrintingPodRecharge_SeededBioInk",
			GERMINATED = "PrintingPodRecharge_GerminatedBioInk",
			FOOD = "PrintingPodRecharge_FoodBioInk",
			SHAKER = "PrintingPodRecharge_ChaosBioInk",
			TWITCH = "PrintingPodRecharge_TwitchBioInk",
			MEDICINAL = "PrintingPodRecharge_Medicinal",
			BIONIC = "PrintingPodRecharge_Bionic";

		public static Dictionary<Bundle, string> itemsToBundle = [];
		public static HashSet<Tag> allInks = [];

		public List<GameObject> CreatePrefabs()
		{
			List<GameObject> inks =
			[
				CreateBioInk(DEFAULT, STRINGS.ITEMS.BIO_INK.NAME, STRINGS.ITEMS.BIO_INK.DESC, "rrp_oozing_bioink_kanim", Bundle.None),
				CreateBioInk(METALLIC, STRINGS.ITEMS.METALLIC_BIO_INK.NAME, STRINGS.ITEMS.METALLIC_BIO_INK.DESC, "rrp_metallic_bioink_kanim", Bundle.Metal),
				CreateBioInk(VACILLATING, STRINGS.ITEMS.VACILLATING_BIO_INK.NAME, STRINGS.ITEMS.VACILLATING_BIO_INK.DESC, "rrp_vacillating_bioink_kanim", Bundle.SuperDuplicant),
				CreateBioInk(GERMINATED, STRINGS.ITEMS.GERMINATED_BIO_INK.NAME, STRINGS.ITEMS.GERMINATED_BIO_INK.DESC, "rrp_germinated_bioink_kanim", Bundle.Egg),
				CreateBioInk(SEEDED, STRINGS.ITEMS.SEEDED_BIO_INK.NAME, STRINGS.ITEMS.SEEDED_BIO_INK.DESC, "rrp_seedy_bioink_kanim", Bundle.Seed),
				CreateBioInk(FOOD, STRINGS.ITEMS.FOOD_BIO_INK.NAME, STRINGS.ITEMS.FOOD_BIO_INK.DESC, "rrp_food_bioink_kanim", Bundle.Food),
				CreateBioInk(SHAKER, STRINGS.ITEMS.SHAKER_BIO_INK.NAME, STRINGS.ITEMS.SHAKER_BIO_INK.DESC, "rrp_rando_bioink_kanim", Bundle.Shaker),
				CreateBioInk(TWITCH, STRINGS.ITEMS.TWITCH_BIO_INK.NAME, STRINGS.ITEMS.TWITCH_BIO_INK.DESC, "rrp_twitch_bioink_kanim", Bundle.Twitch),
				CreateBioInk(MEDICINAL, STRINGS.ITEMS.MEDICINAL_BIO_INK.NAME, STRINGS.ITEMS.MEDICINAL_BIO_INK.DESC, "rrp_medicinal_bioink_kanim", Bundle.Medicinal)
			];

			if (DlcManager.IsContentSubscribed(CONSTS.DLC_BIONIC))
			{
				inks.Add(CreateBioInk(BIONIC, STRINGS.ITEMS.BIONIC_BIO_INK.NAME, STRINGS.ITEMS.BIONIC_BIO_INK.DESC, "rrp_bionic_bioink_kanim", Bundle.Bionic));
			}

			return inks;
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
				additionalTags:
				[
					GameTags.Organics,
					ModAssets.Tags.bioInk,
					GameTags.PedestalDisplayable
				]);

			prefab.AddOrGet<EntitySplitter>();
			prefab.AddOrGet<SimpleMassStatusItem>();
			prefab.AddOrGet<BundleModifier>().bundle = bundle;

			itemsToBundle[bundle] = ID;
			allInks.Add(ID);

			return prefab;
		}

		[Obsolete]
		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}