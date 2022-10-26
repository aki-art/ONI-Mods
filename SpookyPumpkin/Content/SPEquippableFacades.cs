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
            parent.Add(SKELLINGTON, HalloweenCostumeConfig.ID, "sp_skellingtoncostume_kanim", "sp_skellington_item_kanim");
            parent.Add(SCARECROW, HalloweenCostumeConfig.ID, "sp_scarecrow_costume_kanim", "sp_scarecrow_item_kanim");
            parent.Add(VAMPIRE, HalloweenCostumeConfig.ID, "sp_dracula_costume_kanim", "sp_dracula_item_kanim");
        }
    }
}
