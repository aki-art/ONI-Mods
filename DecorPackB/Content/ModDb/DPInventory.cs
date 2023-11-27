using Database;
using DecorPackB.Content.Defs.Buildings;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DecorPackB.Content.ModDb
{
	public class DPInventory
	{
		public static PermitCategory fossil = (PermitCategory)Hash.SDBMLower("DecorPackB_Fossil");

		public static HashSet<string> myFacades = new();
		public static HashSet<string> useMuseumDefs = new()
		{
			FossilDisplayConfig.ID,
			GiantFossilDisplayConfig.ID,
			PotConfig.ID,
			// fountain
			// tile
		};

		public class SUB_CATEGORIES
		{
			public const string
				FOSSILS = "DECORPACKB_FOSSILS",
				POTS = "DECORPACKB_POTS",
				FOUNTAINS = "DECORPACKB_FOUNTAINS";
		}

		public static void ConfigureSubCategories()
		{
			GetOrCreateSubCategory(
				SUB_CATEGORIES.FOSSILS,
				InventoryOrganization.InventoryPermitCategories.ARTWORK,
				Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim("decorpackb_fossildisplay_minipara_kanim")),
				DPArtableStages.fossils.ToArray());

			GetOrCreateSubCategory(
				SUB_CATEGORIES.POTS,
				InventoryOrganization.InventoryPermitCategories.ARTWORK,
				Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim("decorpackb_pot_muckroot_kanim")),
				DPArtableStages.pots.ToArray());

			GetOrCreateSubCategory(
				SUB_CATEGORIES.FOUNTAINS,
				InventoryOrganization.InventoryPermitCategories.ARTWORK,
				Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim("dp_fountain_kanim")),
				DPArtableStages.fountains.ToArray());
		}

		private static HashSet<string> GetOrCreateSubCategory(string subCategory, string mainCategory, Sprite icon, string[] items = null, int sortKey = 900)
		{
			items ??= new string[] { };
			if (!InventoryOrganization.subcategoryIdToPermitIdsMap.ContainsKey(subCategory))
			{
				InventoryOrganization.AddSubcategory(
					subCategory,
					icon,
					sortKey,
					items);

				InventoryOrganization.categoryIdToSubcategoryIdsMap[mainCategory].Add(subCategory);
			}

			foreach (var item in items)
				myFacades.Add(item);

			return InventoryOrganization.subcategoryIdToPermitIdsMap[subCategory];
		}

		private static void AddFacade(ResourceSet<BuildingFacadeResource> resource, string buildingId, string id, string anim)
		{
			var name = Strings.Get($"STRINGS.BUILDINGS.PREFABS.{buildingId.ToUpperInvariant()}.FACADES.{id.ToUpperInvariant()}.NAME")?.String;
			var desc = Strings.Get($"STRINGS.BUILDINGS.PREFABS.{buildingId.ToUpperInvariant()}.FACADES.{id.ToUpperInvariant()}.DESC")?.String;

			Add(resource,
				id,
				name,
				desc + STRINGS.DESIGN_BY_DECORPACKB,
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
