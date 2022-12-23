using Database;
using DecorPackB.Content.Buildings.FossilDisplays;
using static Database.ArtableStatuses;

namespace DecorPackB.Content
{
    public class DPArtableStages
{
        public static void Register(ArtableStages stages)
        {
            // ugly
            AddStage(stages, FossilDisplayConfig.ID, "human", "decorpackb_human_kanim", 5, ArtableStatusType.LookingUgly);
            
            // mediocre
            AddStage(stages, FossilDisplayConfig.ID, "pacu", "decorpackb_pacu_kanim", 10, ArtableStatusType.LookingOkay);

            // great
            AddStage(stages, FossilDisplayConfig.ID, "dodo", "decorpackb_dodo_kanim", 15, ArtableStatusType.LookingGreat);
            AddStage(stages, FossilDisplayConfig.ID, "beefalo", "decorpackb_beefalo_kanim", 15, ArtableStatusType.LookingGreat);
            AddStage(stages, FossilDisplayConfig.ID, "minipara", "decorpackb_minipara_kanim", 15, ArtableStatusType.LookingGreat);
            AddStage(stages, FossilDisplayConfig.ID, "trilobite", "decorpackb_trilobite_kanim", 15, ArtableStatusType.LookingGreat);
        }

        private static void AddStage(ArtableStages stages, string buildingID, string ID, string anim, int decorBonus, ArtableStatusType status)
        {
            var key = $"DecorPackB.STRINGS.BUILDINGS.PREFABS.{buildingID.ToUpperInvariant()}.VARIANT.{ID.ToUpperInvariant()}";

            var name = Strings.Get(key + ".NAME");
            var description = Strings.Get(key + ".DESCRIPTION");

            stages.Add(
                $"DecorPackB_{buildingID}_{ID}",
                name,
                description,
                PermitRarity.Universal,
                anim,
                "idle",
                decorBonus,
                status == ArtableStatusType.LookingGreat,
                status.ToString(),
                buildingID);
        }
    }
}
