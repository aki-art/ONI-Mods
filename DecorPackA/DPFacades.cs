using Database;
using System.Collections.Generic;
using UnityEngine;

namespace DecorPackA
{
	public class DPFacades
	{
		public static Dictionary<string, string> categories = new();

		public class SUB_CATEGORIES
		{
			public const string
				DOORS = "BUILDING_DOOR";
		}

		public class AERO_POTS
		{
			public const string
				COLORFUL = "DecorPackA_FlowerVaseHangingFancy_Colorful",
				BLUE_YELLOW = "DecorPackA_FlowerVaseHangingFancy_BlueYellow",
				SHOVEVOLE = "DecorPackA_FlowerVaseHangingFancy_ShoveVole",
				HONEY = "DecorPackA_FlowerVaseHangingFancy_Honey",
				URANIUM = "DecorPackA_FlowerVaseHangingFancy_Uranium";
		}

		public class PNEUMATIC_DOORS
		{
			public const string
				GLASS = "DecorPackA_PneumaticDoor_Glass",
				GREEN = "DecorPackA_PneumaticDoor_StainedGreen",
				PURPLE = "DecorPackA_PneumaticDoor_StainedPurple",
				VERY_PURPLE = "DecorPackA_PneumaticDoor_StainedVeryPurple",
				RED = "DecorPackA_PneumaticDoor_StainedRed";
		}

		public static void ConfigureSubCategories()
		{
			var sculptures = InventoryOrganization.subcategoryIdToPermitIdsMap[InventoryOrganization.PermitSubcategories.BUILDING_SCULPTURE];

			foreach (var sculpture in ModDb.mySculptures)
				sculptures.Add(sculpture);

			var pots = InventoryOrganization.subcategoryIdToPermitIdsMap[InventoryOrganization.PermitSubcategories.BUILDINGS_FLOWER_VASE];
			pots.Add(AERO_POTS.COLORFUL);
			pots.Add(AERO_POTS.BLUE_YELLOW);
			pots.Add(AERO_POTS.SHOVEVOLE);
			pots.Add(AERO_POTS.HONEY);
			pots.Add(AERO_POTS.URANIUM);

			var doors = GetOrCreateSubCategory(
				SUB_CATEGORIES.DOORS,
				InventoryOrganization.InventoryPermitCategories.BUILDINGS,
				Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim("door_internal_kanim")));

			doors.Add(PNEUMATIC_DOORS.GLASS);
			doors.Add(PNEUMATIC_DOORS.GREEN);
			doors.Add(PNEUMATIC_DOORS.RED);
			doors.Add(PNEUMATIC_DOORS.PURPLE);
			doors.Add(PNEUMATIC_DOORS.VERY_PURPLE);
		}

		private static HashSet<string> GetOrCreateSubCategory(string subCategory, string mainCategory, Sprite icon)
		{
			if (!InventoryOrganization.subcategoryIdToPermitIdsMap.ContainsKey(subCategory))
			{
				InventoryOrganization.AddSubcategory(
					subCategory,
					icon,
					900,
					new string[] { });

				InventoryOrganization.categoryIdToSubcategoryIdsMap[mainCategory].Add(subCategory);
			}

			return InventoryOrganization.subcategoryIdToPermitIdsMap[subCategory];
		}

		public static void Register(ResourceSet<BuildingFacadeResource> resource)
		{
			AddFacade(
				resource,
				FlowerVaseHangingFancyConfig.ID,
				AERO_POTS.COLORFUL,
				STRINGS.BUILDINGS.PREFABS.FLOWERVASEHANGINGFANCY.FACADES.DECORPACKA_COLORFUL.NAME,
				STRINGS.BUILDINGS.PREFABS.FLOWERVASEHANGINGFANCY.FACADES.DECORPACKA_COLORFUL.DESC,
				"decorpacka_hangingvase_colorful_kanim");

			AddFacade(
				resource,
				FlowerVaseHangingFancyConfig.ID,
				AERO_POTS.BLUE_YELLOW,
				STRINGS.BUILDINGS.PREFABS.FLOWERVASEHANGINGFANCY.FACADES.DECORPACKA_BLUEYELLOW.NAME,
				STRINGS.BUILDINGS.PREFABS.FLOWERVASEHANGINGFANCY.FACADES.DECORPACKA_BLUEYELLOW.DESC,
				"decorpacka_hangingvase_blueyellow_kanim");

			AddFacade(
				resource,
				FlowerVaseHangingFancyConfig.ID,
				AERO_POTS.SHOVEVOLE,
				STRINGS.BUILDINGS.PREFABS.FLOWERVASEHANGINGFANCY.FACADES.DECORPACKA_SHOVEVOLE.NAME,
				STRINGS.BUILDINGS.PREFABS.FLOWERVASEHANGINGFANCY.FACADES.DECORPACKA_SHOVEVOLE.DESC,
				"decorpacka_hangingvase_shovevoleb_kanim");

			AddFacade(
				resource,
				FlowerVaseHangingFancyConfig.ID,
				AERO_POTS.HONEY,
				STRINGS.BUILDINGS.PREFABS.FLOWERVASEHANGINGFANCY.FACADES.DECORPACKA_HONEY.NAME,
				STRINGS.BUILDINGS.PREFABS.FLOWERVASEHANGINGFANCY.FACADES.DECORPACKA_HONEY.DESC,
				"decorpacka_hangingvase_honey_kanim");

			AddFacade(
				resource,
				FlowerVaseHangingFancyConfig.ID,
				AERO_POTS.URANIUM,
				STRINGS.BUILDINGS.PREFABS.FLOWERVASEHANGINGFANCY.FACADES.DECORPACKA_URANIUM.NAME,
				STRINGS.BUILDINGS.PREFABS.FLOWERVASEHANGINGFANCY.FACADES.DECORPACKA_URANIUM.DESC,
				"decorpacka_hangingvase_uranium_kanim");

			AddFacade(
				resource,
				DoorConfig.ID,
				PNEUMATIC_DOORS.GLASS,
				STRINGS.BUILDINGS.PREFABS.DOOR.FACADES.DECORPACKA_PNEUMATICDOOR_GLASS.NAME,
				STRINGS.BUILDINGS.PREFABS.DOOR.FACADES.DECORPACKA_PNEUMATICDOOR_GLASS.DESC,
				"decorpacka_pneumaticdoor_glass_kanim");

			AddFacade(
				resource,
				DoorConfig.ID,
				PNEUMATIC_DOORS.GREEN,
				STRINGS.BUILDINGS.PREFABS.DOOR.FACADES.DECORPACKA_PNEUMATICDOOR_STAINEDGREEN.NAME,
				STRINGS.BUILDINGS.PREFABS.DOOR.FACADES.DECORPACKA_PNEUMATICDOOR_STAINEDGREEN.DESC,
				"decorpacka_pneumaticdoor_stained_blue_kanim");

			AddFacade(
				resource,
				DoorConfig.ID,
				PNEUMATIC_DOORS.PURPLE,
				STRINGS.BUILDINGS.PREFABS.DOOR.FACADES.DECORPACKA_PNEUMATICDOOR_STAINEDPURPLE.NAME,
				STRINGS.BUILDINGS.PREFABS.DOOR.FACADES.DECORPACKA_PNEUMATICDOOR_STAINEDPURPLE.DESC,
				"decorpacka_pneumaticdoor_stained_purple_kanim");

			AddFacade(
				resource,
				DoorConfig.ID,
				PNEUMATIC_DOORS.VERY_PURPLE,
				STRINGS.BUILDINGS.PREFABS.DOOR.FACADES.DECORPACKA_PNEUMATICDOOR_STAINEDVERYPURPLE.NAME,
				STRINGS.BUILDINGS.PREFABS.DOOR.FACADES.DECORPACKA_PNEUMATICDOOR_STAINEDVERYPURPLE.DESC,
				"decorpacka_pneumaticdoor_stained_purpler_kanim");

			AddFacade(
				resource,
				DoorConfig.ID,
				PNEUMATIC_DOORS.RED,
				STRINGS.BUILDINGS.PREFABS.DOOR.FACADES.DECORPACKA_PNEUMATICDOOR_STAINEDRED.NAME,
				STRINGS.BUILDINGS.PREFABS.DOOR.FACADES.DECORPACKA_PNEUMATICDOOR_STAINEDRED.DESC,
				"decorpacka_pneumaticdoor_stained_red_kanim");
		}

		private static void AddFacade(ResourceSet<BuildingFacadeResource> resource, string buildingId, string id, string name, string desc, string anim)
		{
			Add(resource,
				id,
				name,
				desc + STRINGS.DESIGN_BY_DECORPACKA,
				PermitRarity.Universal,
				buildingId,
				anim);

			ModDb.myFacades.Add(id);
		}

		public static void Add(
				ResourceSet<BuildingFacadeResource> set,
				string id,
				LocString name,
				LocString description,
				PermitRarity rarity,
				string prefabId,
				string animFile,
				Dictionary<string, string> workables = null)
		{
			set.resources.Add(new BuildingFacadeResource(id, name, description, rarity, prefabId, animFile, workables));
		}
	}
}
