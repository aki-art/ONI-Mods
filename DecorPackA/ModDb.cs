using Database;
using DecorPackA.Buildings.GlassSculpture;
using DecorPackA.Buildings.MoodLamp;
using FUtility;
using static Database.ArtableStatuses;

namespace DecorPackA
{
    public class ModDb
    {
        public static LampVariants lampVariants;

        public static void Initialize()
        {
            lampVariants = new LampVariants();
        }

        public static void RegisterArtableStages(ArtableStages stages)
        {
            AddBadSculpture(stages, "Bad", "decorpacka_glasssculpture_broken_kanim");
            AddMedSculpture(stages, "Average", "decorpacka_glasssculpture_muckroot_kanim");
            AddGreatSculpture(stages, "Good1", "decorpacka_glasssculpture_meep_kanim");
            AddGreatSculpture(stages, "Good2", "decorpacka_glasssculpture_hatch_kanim");
            AddGreatSculpture(stages, "Good3", "decorpacka_glasssculpture_posh_crab_kanim");
            AddGreatSculpture(stages, "Good4", "decorpacka_glasssculpture_pip_kanim");
            AddGreatSculpture(stages, "Good5", "decorpacka_glasssculpture_unicorn_kanim");
            AddGreatSculpture(stages, "Good6", "decorpacka_glasssculpture_swan_kanim");
            AddGreatSculpture(stages, "Good7", "decorpacka_glasssculpture_golem_kanim");
        }

        private static void AddGreatSculpture(ArtableStages stages, string id, string animFile)
        {
            AddStage(
                stages,
                GlassSculptureConfig.ID,
                $"DecorPackA_GlassSculpture_{id}",
                animFile,
                Mod.Settings.GlassSculpture.GeniousSculptureDecorBonus,
                ArtableStatusType.LookingGreat);
        }

        private static void AddMedSculpture(ArtableStages stages, string id, string animFile)
        {
            AddStage(
                stages,
                GlassSculptureConfig.ID,
                $"DecorPackA_GlassSculpture_{id}",
                animFile,
                Mod.Settings.GlassSculpture.MediocreSculptureDecorBonus,
                ArtableStatusType.LookingOkay);
        }

        private static void AddBadSculpture(ArtableStages stages, string id, string animFile)
        {
            AddStage(
                stages,
                GlassSculptureConfig.ID,
                $"DecorPackA_GlassSculpture_{id}",
                animFile,
                Mod.Settings.GlassSculpture.BadSculptureDecorBonus,
                ArtableStatusType.LookingUgly);
        }

        public static string AddStage(ArtableStages stages, string buildingID, string ID, string anim, int decorBonus, ArtableStatusType status, string defaultAnim = "idle")
        {
            var prefix = Log.modName;
            var key = $"{prefix}.STRINGS.BUILDINGS.PREFABS.{buildingID.ToUpperInvariant()}.FACADES.{ID.ToUpperInvariant()}";
            var id = $"{prefix}_{buildingID}_{ID}";
            var name = Strings.Get(key + ".NAME");
            var description = Strings.Get(key + ".DESCRIPTION");

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
