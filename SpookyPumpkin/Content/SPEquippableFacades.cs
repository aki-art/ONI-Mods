using Database;

namespace SpookyPumpkinSO.Content
{
    public class SPEquippableFacades
    {
        public const string SKELLINGTON = "SP_JackSkellingtonCostume";

        public static void Register(EquippableFacades parent)
        {
            parent.Add(SKELLINGTON, HalloweenCostumeConfig.ID, "sp_skellingtoncostume_kanim", "shirt_decor02_kanim");
        }
    }
}
