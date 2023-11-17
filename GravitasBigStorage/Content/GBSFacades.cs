using Database;

namespace GravitasBigStorage.Content
{
	public class GBSFacades
	{
		public const string
			ALIEN = "GravitasBigStorage_Container_Alien",
			PADDED = "GravitasBigStorage_Container_Padded",
			RED = "GravitasBigStorage_Container_Red",
			RETRO = "GravitasBigStorage_Container_Retro",
			RUSTY = "GravitasBigStorage_Container_Rusty",
			STARRY = "GravitasBigStorage_Container_Starry";

		public static void AddSubCategory()
		{
			var sculptures = InventoryOrganization.subcategoryIdToPermitIdsMap[InventoryOrganization.PermitSubcategories.BUILDINGS_STORAGE];

			sculptures.Add(ALIEN);
			sculptures.Add(PADDED);
			sculptures.Add(RED);
			sculptures.Add(RETRO);
			sculptures.Add(RUSTY);
			sculptures.Add(STARRY);
		}

		internal static void RegisterFacades(BuildingFacades facades)
		{
			facades.Add(
					"GravitasBigStorage_Container_Alien",
					STRINGS.BUILDINGS.PREFABS.GRAVITASBIGSTORAGE_CONTAINER.FACADES.ALIEN.NAME,
					STRINGS.BUILDINGS.PREFABS.GRAVITASBIGSTORAGE_CONTAINER.FACADES.ALIEN.DESC,
					PermitRarity.Universal,
					GravitasBigStorageConfig.ID,
					"gravitasbigstorage_container_alien_kanim");

			facades.Add(
				"GravitasBigStorage_Container_Padded",
				STRINGS.BUILDINGS.PREFABS.GRAVITASBIGSTORAGE_CONTAINER.FACADES.PADDED.NAME,
				STRINGS.BUILDINGS.PREFABS.GRAVITASBIGSTORAGE_CONTAINER.FACADES.PADDED.DESC,
				PermitRarity.Universal,
				GravitasBigStorageConfig.ID,
				"gravitasbigstorage_container_padded_kanim");

			facades.Add(
				"GravitasBigStorage_Container_Red",
				STRINGS.BUILDINGS.PREFABS.GRAVITASBIGSTORAGE_CONTAINER.FACADES.RED.NAME,
				STRINGS.BUILDINGS.PREFABS.GRAVITASBIGSTORAGE_CONTAINER.FACADES.RED.DESC,
				PermitRarity.Universal,
				GravitasBigStorageConfig.ID,
				"gravitasbigstorage_container_red_kanim");

			facades.Add(
				"GravitasBigStorage_Container_Retro",
				STRINGS.BUILDINGS.PREFABS.GRAVITASBIGSTORAGE_CONTAINER.FACADES.RETRO.NAME,
				STRINGS.BUILDINGS.PREFABS.GRAVITASBIGSTORAGE_CONTAINER.FACADES.RETRO.DESC,
				PermitRarity.Universal,
				GravitasBigStorageConfig.ID,
				"gravitasbigstorage_container_retro_kanim");

			facades.Add(
				"GravitasBigStorage_Container_Rusty",
				STRINGS.BUILDINGS.PREFABS.GRAVITASBIGSTORAGE_CONTAINER.FACADES.RUSTY.NAME,
				STRINGS.BUILDINGS.PREFABS.GRAVITASBIGSTORAGE_CONTAINER.FACADES.RUSTY.DESC,
				PermitRarity.Universal,
				GravitasBigStorageConfig.ID,
				"gravitasbigstorage_container_rusty_kanim");

			facades.Add(
				"GravitasBigStorage_Container_Starry",
				STRINGS.BUILDINGS.PREFABS.GRAVITASBIGSTORAGE_CONTAINER.FACADES.STARRY.NAME,
				STRINGS.BUILDINGS.PREFABS.GRAVITASBIGSTORAGE_CONTAINER.FACADES.STARRY.DESC,
				PermitRarity.Universal,
				GravitasBigStorageConfig.ID,
				"gravitasbigstorage_container_starry_kanim");
		}
	}
}
