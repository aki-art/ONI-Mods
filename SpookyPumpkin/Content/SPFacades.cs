using Database;
using System.Collections.Generic;

namespace SpookyPumpkinSO.Content
{
	public class SPFacades
	{
		public static HashSet<string> myFacades = new();

		public static string PUMPKINBED = "SpookyPumpking_ElegantBed_Pumpkin";

		public static void Register(ResourceSet<BuildingFacadeResource> set)
		{
			Add(
				set,
				PUMPKINBED,
				STRINGS.BUILDINGS.PREFABS.LUXURYBED.FACADES.SPOOKYPUMPKING_LUXURYBED_PUMPKIN.NAME,
				STRINGS.BUILDINGS.PREFABS.LUXURYBED.FACADES.SPOOKYPUMPKING_LUXURYBED_PUMPKIN.DESC,
				LuxuryBedConfig.ID,
				"sp_luxurybed_pumpkin_kanim");
		}

		public static void Add(
				ResourceSet<BuildingFacadeResource> set,
				string id,
				LocString name,
				LocString description,
				string prefabId,
				string animFile,
				Dictionary<string, string> workables = null)
		{
			myFacades.Add(id);

			set.resources.Add(new BuildingFacadeResource(id, name, description, PermitRarity.Universal, prefabId, animFile, DlcManager.AVAILABLE_ALL_VERSIONS, workables));
		}

		public static void ConfigureSubCategories()
		{
			InventoryOrganization.subcategoryIdToPermitIdsMap["BUILDINGS_BED_LUXURY"].Add(PUMPKINBED);
		}
	}
}
