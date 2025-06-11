using Database;
using SpookyPumpkinSO.Content.Equipment;

namespace SpookyPumpkinSO.Content
{
	public class SPEquippableFacades
	{
		public const string SKELLINGTON = "SP_JackSkellingtonCostume";
		public const string SCARECROW = "SP_ScarecrowCostume";
		public const string VAMPIRE = "SP_VampireCostume";

		public static void Register(EquippableFacades parent)
		{
			parent.Add(
				SKELLINGTON,
				STRINGS.EQUIPMENT.PREFABS.SP_HALLOWEENCOSTUME.FACADES.SP_JACKSKELLINGTONCOSTUME,
				"",
				PermitRarity.Universal,
				HalloweenCostumeConfig.ID,
				"sp_skellingtoncostume_kanim",
				"sp_skellington_item_kanim",
				null,
				null);

			parent.Add(
				SCARECROW,
				STRINGS.EQUIPMENT.PREFABS.SP_HALLOWEENCOSTUME.FACADES.SP_SCARECROWCOSTUME,
				"",
				PermitRarity.Universal,
				HalloweenCostumeConfig.ID,
				"sp_scarecrow_costume_kanim",
				"sp_scarecrow_item_kanim",
				null,
				null);

			parent.Add(
				VAMPIRE,
				STRINGS.EQUIPMENT.PREFABS.SP_HALLOWEENCOSTUME.FACADES.SP_VAMPIRECOSTUME,
				"",
				PermitRarity.Universal,
				HalloweenCostumeConfig.ID,
				"sp_dracula_costume_kanim",
				"sp_dracula_item_kanim",
				null,
				null);

			InventoryOrganization.subcategoryIdToPermitIdsMap["YAML"].Add(SKELLINGTON);
			InventoryOrganization.subcategoryIdToPermitIdsMap["YAML"].Add(SCARECROW);
			InventoryOrganization.subcategoryIdToPermitIdsMap["YAML"].Add(VAMPIRE);
		}
	}
}
