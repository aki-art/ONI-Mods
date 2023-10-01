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
				HalloweenCostumeConfig.ID,
				"sp_skellingtoncostume_kanim",
				"sp_skellington_item_kanim");

			parent.Add(
				SCARECROW,
				STRINGS.EQUIPMENT.PREFABS.SP_HALLOWEENCOSTUME.FACADES.SP_SCARECROWCOSTUME,
				HalloweenCostumeConfig.ID,
				"sp_scarecrow_costume_kanim",
				"sp_scarecrow_item_kanim");

			parent.Add(
				VAMPIRE,
				STRINGS.EQUIPMENT.PREFABS.SP_HALLOWEENCOSTUME.FACADES.SP_VAMPIRECOSTUME,
				HalloweenCostumeConfig.ID,
				"sp_dracula_costume_kanim",
				"sp_dracula_item_kanim");

			InventoryOrganization.subcategoryIdToPermitIdsMap["YAML"].Add(SKELLINGTON);
			InventoryOrganization.subcategoryIdToPermitIdsMap["YAML"].Add(SCARECROW);
			InventoryOrganization.subcategoryIdToPermitIdsMap["YAML"].Add(VAMPIRE);
		}
	}
}
