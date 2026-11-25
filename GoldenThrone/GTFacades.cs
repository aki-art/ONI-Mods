using Database;

namespace GoldenThrone
{
    public class GTFacades
    {
        public const string GOLDEN_TOILET = "GoldenThroneToilet";

        public static void AddFacade(ResourceSet<BuildingFacadeResource> resource)
        {
            resource.resources.Add(new BuildingFacadeResource(
                GOLDEN_TOILET,
                STRINGS.BUILDINGS.PREFABS.FLUSHTOILET.FACADES.GOLDEN_THRONE_TOILET.NAME,
                STRINGS.BUILDINGS.PREFABS.FLUSHTOILET.FACADES.GOLDEN_THRONE_TOILET.DESC,
                PermitRarity.Universal,
                FlushToiletConfig.ID,
                "gold_flush_kanim",
                null,
                null,
                null,
                null
                ));
        }
    }
}
