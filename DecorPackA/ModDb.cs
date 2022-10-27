using Database;
using DecorPackA.Buildings.GlassSculpture;
using DecorPackA.Buildings.MoodLamp;
using static DecorPackA.STRINGS.BUILDINGS.PREFABS.DECORPACKA_GLASSSCULPTURE;

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
            var config = Mod.Settings.GlassSculpture;
            var anim = "sculpture_glass_kanim";
            var artableStatuses = Db.Get().ArtableStatuses;
            var ID = GlassSculptureConfig.ID;

            stages.Add("DecorPackA_GlassSculpture_Bad", POORQUALITYNAME, anim, "crap_1", config.BadSculptureDecorBonus, false, artableStatuses.Ugly, ID);
            stages.Add("DecorPackA_GlassSculpture_Average", AVERAGEQUALITYNAME, anim, "good_1", config.MediocreSculptureDecorBonus, false, artableStatuses.Okay, ID);
            stages.Add("DecorPackA_GlassSculpture_Good1", EXCELLENTQUALITYNAME, anim, "amazing_1", config.GeniousSculptureDecorBonus, true, artableStatuses.Great, ID);
            stages.Add("DecorPackA_GlassSculpture_Good2", EXCELLENTQUALITYNAME, anim, "amazing_2", config.GeniousSculptureDecorBonus, true, artableStatuses.Great, ID);
            stages.Add("DecorPackA_GlassSculpture_Good3", EXCELLENTQUALITYNAME, anim, "amazing_3", config.GeniousSculptureDecorBonus, true, artableStatuses.Great, ID);
            stages.Add("DecorPackA_GlassSculpture_Good4", EXCELLENTQUALITYNAME, anim, "amazing_4", config.GeniousSculptureDecorBonus, true, artableStatuses.Great, ID);
            stages.Add("DecorPackA_GlassSculpture_Good5", EXCELLENTQUALITYNAME, anim, "amazing_5", config.GeniousSculptureDecorBonus, true, artableStatuses.Great, ID);
            stages.Add("DecorPackA_GlassSculpture_Good6", EXCELLENTQUALITYNAME, anim, "amazing_6", config.GeniousSculptureDecorBonus, true, artableStatuses.Great, ID);
            stages.Add("DecorPackA_GlassSculpture_Good7", EXCELLENTQUALITYNAME, anim, "amazing_7", config.GeniousSculptureDecorBonus, true, artableStatuses.Great, ID);
        }
    }
}
