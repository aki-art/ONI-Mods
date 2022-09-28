using CrittersDropBones.Items;
using System.Collections.Generic;

namespace CrittersDropBones.Settings
{
    public class DropsConfig
    {
        public static Dictionary<string, BoneDropConfig> bones = new Dictionary<string, BoneDropConfig>()
        {
            // Bones
            { GameTags.Creatures.Species.PuftSpecies.ToString(), SimpleTag(BoneConfig.ID) },
            { GameTags.Creatures.Species.SquirrelSpecies.ToString(), SimpleTag(BoneConfig.ID) },
            { GameTags.Creatures.Species.MoleSpecies.ToString(), SimpleTag(BoneConfig.ID, 2, 1) },
            { GameTags.Creatures.Species.DreckoSpecies.ToString(), SimpleTag(BoneConfig.ID, 2, 1) },
            { GameTags.Creatures.Species.MooSpecies.ToString(), SimpleTag(BoneConfig.ID, 3, 1) },

            // Fish Bones
            { GameTags.Creatures.Species.PacuSpecies.ToString(), SimpleTag(FishBoneConfig.ID) },

            // Gelatine
            { GameTags.Creatures.Species.OilFloaterSpecies.ToString(), SimpleTag(GelatineConfig.ID) }
        };

        private static BoneDropConfig SimpleTag(string tag, int adult = 1, int baby = 0)
        {
            return new BoneDropConfig(tag, adult, baby);
        }

        public class BoneDropConfig
        {
            public string drop;
            public int amountAdult;
            public int amountBaby;

            public BoneDropConfig(string drop, int amountAdult = 1, int amountBaby = 0)
            {
                this.drop = drop;
                this.amountAdult = amountAdult;
                this.amountBaby = amountBaby;
            }
        }
    }
}
