using Database;
using static Database.ArtableStatuses;

namespace FUtility
{
    public class ArtableUtil
    {
        public static string AddStage(ArtableStages stages, string buildingID, string ID, string anim, int decorBonus, ArtableStatusType status, string defaultAnim = "idle")
        {
            var prefix = Log.modName;
            var key = $"{prefix}.STRINGS.BUILDINGS.PREFABS.{buildingID.ToUpperInvariant()}.VARIANT.{ID.ToUpperInvariant()}";
            var id = $"{prefix}_{buildingID}_{ID}";
            var name = Strings.Get(key + ".NAME");
            var description = Strings.Get(key + ".DESCRIPTION");

            PermitResources.PermitIdsToExcludeFromSupplyCloset.Add(id);

            stages.Add(
                $"{prefix}_{buildingID}_{ID}",
                name,
                description,
                PermitRarity.Universal,
                anim,
                defaultAnim,
                decorBonus,
                status == ArtableStatusType.LookingGreat,
                status.ToString(),
                buildingID);

            return id;
        }
    }
}
