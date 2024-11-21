using Database;
using DecorPackA.Buildings.MoodLamp;
using System.Collections.Generic;
using UnityEngine;

namespace DecorPackA
{
	public class DPFacades
	{
		public static Dictionary<string, string> categories = new();

		public static HashSet<string> myFacades = new();

		public class SUB_CATEGORIES
		{
			public const string
				DOORS = "BUILDING_DOOR",
				DINING_TABLES = "DINING_TABLE",
				DESKS = "DECORPACKA_BUILDING_MOODLAMPDESK";
		}

		public class DINING_TABLES
		{
			public const string GLASS = "DecorPackA_DiningTable_Glass";
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

		public class DESKS
		{
			public const string
				GLASS = "DecorPackA_Moodlamp_Glass",
				ROBOTICS = "DecorPackA_Moodlamp_Robotics",
				BOILER = "DecorPackA_Moodlamp_Boiler",
				MODERNORANGE = "DecorPackA_Moodlamp_ModernOrange",
				MODERNBLUE = "DecorPackA_Moodlamp_ModernBlue",
				MODERNPURPLE = "DecorPackA_Moodlamp_ModernPurple",
				THULECITE = "DecorPackA_Moodlamp_Thulecite",
				TOUCHSTONE = "DecorPackA_Moodlamp_Touchstone",
				TREETRUNK = "DecorPackA_Moodlamp_TreeTrunk",
				INDUSTRIAL = "DecorPackA_Moodlamp_Industrial";
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

			var diningTables = GetOrCreateSubCategory(
				SUB_CATEGORIES.DINING_TABLES,
				InventoryOrganization.InventoryPermitCategories.BUILDINGS,
				Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim("diningtable_kanim")));

			diningTables.Add(DINING_TABLES.GLASS);

			var doors = GetOrCreateSubCategory(
				SUB_CATEGORIES.DOORS,
				InventoryOrganization.InventoryPermitCategories.BUILDINGS,
				Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim("door_internal_kanim")));

			doors.Add(PNEUMATIC_DOORS.GLASS);
			doors.Add(PNEUMATIC_DOORS.GREEN);
			doors.Add(PNEUMATIC_DOORS.RED);
			doors.Add(PNEUMATIC_DOORS.PURPLE);
			doors.Add(PNEUMATIC_DOORS.VERY_PURPLE);

			var desks = GetOrCreateSubCategory(
				SUB_CATEGORIES.DESKS,
				InventoryOrganization.InventoryPermitCategories.BUILDINGS,
				Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim("dpi_desk_kanim")));

			desks.Add(DESKS.GLASS);
			desks.Add(DESKS.ROBOTICS);
			desks.Add(DESKS.BOILER);
			desks.Add(DESKS.MODERNORANGE);
			desks.Add(DESKS.MODERNBLUE);
			desks.Add(DESKS.MODERNPURPLE);
			desks.Add(DESKS.TOUCHSTONE);
			desks.Add(DESKS.THULECITE);
			desks.Add(DESKS.TREETRUNK);
			desks.Add(DESKS.INDUSTRIAL);
		}

		private static HashSet<string> GetOrCreateSubCategory(string subCategory, string mainCategory, Sprite icon)
		{
			if (!InventoryOrganization.subcategoryIdToPermitIdsMap.ContainsKey(subCategory))
			{
				InventoryOrganization.AddSubcategory(
					subCategory,
					icon,
					900,
					[]);

				InventoryOrganization.categoryIdToSubcategoryIdsMap[mainCategory].Add(subCategory);
			}

			return InventoryOrganization.subcategoryIdToPermitIdsMap[subCategory];
		}

		public static void Register(ResourceSet<BuildingFacadeResource> resource)
		{
			AddFacade(resource, FlowerVaseHangingFancyConfig.ID, AERO_POTS.COLORFUL, "decorpacka_hangingvase_colorful_kanim");
			AddFacade(resource, FlowerVaseHangingFancyConfig.ID, AERO_POTS.BLUE_YELLOW, "decorpacka_hangingvase_blueyellow_kanim");
			AddFacade(resource, FlowerVaseHangingFancyConfig.ID, AERO_POTS.SHOVEVOLE, "decorpacka_hangingvase_shovevoleb_kanim");
			AddFacade(resource, FlowerVaseHangingFancyConfig.ID, AERO_POTS.HONEY, "decorpacka_hangingvase_honey_kanim");
			AddFacade(resource, FlowerVaseHangingFancyConfig.ID, AERO_POTS.URANIUM, "decorpacka_hangingvase_uranium_kanim");

			AddFacade(resource, DoorConfig.ID, PNEUMATIC_DOORS.GLASS, "decorpacka_pneumaticdoor_glass_kanim");
			AddFacade(resource, DoorConfig.ID, PNEUMATIC_DOORS.GREEN, "decorpacka_pneumaticdoor_stained_blue_kanim");
			AddFacade(resource, DoorConfig.ID, PNEUMATIC_DOORS.PURPLE, "decorpacka_pneumaticdoor_stained_purple_kanim");
			AddFacade(resource, DoorConfig.ID, PNEUMATIC_DOORS.VERY_PURPLE, "decorpacka_pneumaticdoor_stained_purpler_kanim");
			AddFacade(resource, DoorConfig.ID, PNEUMATIC_DOORS.RED, "decorpacka_pneumaticdoor_stained_red_kanim");

			AddFacade(resource, MoodLampConfig.ID, DESKS.GLASS, "decorpacki_desk_glass_kanim");
			AddFacade(resource, MoodLampConfig.ID, DESKS.ROBOTICS, "decorpacki_desk_robotics_kanim");
			AddFacade(resource, MoodLampConfig.ID, DESKS.BOILER, "decorpacki_desk_boiler_kanim");
			AddFacade(resource, MoodLampConfig.ID, DESKS.MODERNORANGE, "decorpacki_desk_modern_orange_kanim");
			AddFacade(resource, MoodLampConfig.ID, DESKS.MODERNBLUE, "decorpacki_desk_modern_blue_kanim");
			AddFacade(resource, MoodLampConfig.ID, DESKS.MODERNPURPLE, "decorpacki_desk_modern_purple_kanim");
			AddFacade(resource, MoodLampConfig.ID, DESKS.TOUCHSTONE, "decorpacki_desk_touchstone_kanim");
			AddFacade(resource, MoodLampConfig.ID, DESKS.THULECITE, "decorpacki_desk_thulecite_kanim");
			AddFacade(resource, MoodLampConfig.ID, DESKS.TREETRUNK, "decorpacki_desk_treetrunk_kanim");
			AddFacade(resource, MoodLampConfig.ID, DESKS.INDUSTRIAL, "decorpacki_desk_industrial_kanim");

			AddFacade(resource, DiningTableConfig.ID, DINING_TABLES.GLASS, "decorpacka_diningtable_glass_kanim");
		}

		private static void AddFacade(ResourceSet<BuildingFacadeResource> resource, string buildingId, string id, string anim)
		{
			var name = Strings.Get($"STRINGS.BUILDINGS.PREFABS.{buildingId.ToUpperInvariant()}.FACADES.{id.ToUpperInvariant()}.NAME")?.String;
			var desc = Strings.Get($"STRINGS.BUILDINGS.PREFABS.{buildingId.ToUpperInvariant()}.FACADES.{id.ToUpperInvariant()}.DESC")?.String;

			Add(resource,
				id,
				name,
				desc + STRINGS.DESIGN_BY_DECORPACKA,
				PermitRarity.Universal,
				buildingId,
				anim);

			myFacades.Add(id);
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
			set.resources.Add(new BuildingFacadeResource(id, name, description, rarity, prefabId, animFile, DlcManager.AVAILABLE_ALL_VERSIONS, workables));
		}
	}
}
