using CrittersDropBones.Content.Items;
using System;
using System.Collections.Generic;

namespace CrittersDropBones.Settings
{
    public class DropsConfig
    {
        // for mods to patch in
        public static void AddDrop(string species, string bone, int babyAmount, int adultAmount)
        {
            bones.Add(species, new BoneDropConfig(bone, babyAmount, adultAmount));
        }

        public static Dictionary<string, BoneDropConfig> bones = new()
        {
            // Bones
            { GameTags.Creatures.Species.PuftSpecies.ToString(), new BoneDropConfig(BoneConfig.ID) },
            { GameTags.Creatures.Species.SquirrelSpecies.ToString(), new BoneDropConfig(BoneConfig.ID) },
            { GameTags.Creatures.Species.MoleSpecies.ToString(), new BoneDropConfig(BoneConfig.ID, 1, 2) },
            { GameTags.Creatures.Species.DreckoSpecies.ToString(), new BoneDropConfig(BoneConfig.ID, 1, 2) },
            { GameTags.Creatures.Species.MooSpecies.ToString(), new BoneDropConfig(BoneConfig.ID, 1, 3) },

            // Fish Bones
            { GameTags.Creatures.Species.PacuSpecies.ToString(), new BoneDropConfig(FishBoneConfig.ID) },

            // Gelatine
            { GameTags.Creatures.Species.OilFloaterSpecies.ToString(), new BoneDropConfig(GelatineConfig.ID) }
        };

        [Serializable]
        public class BoneDropConfig
        {
            public string drop;
            public int amountAdult;
            public int amountBaby;

            public BoneDropConfig(string drop, int amountBaby = 0, int amountAdult = 1)
            {
                this.drop = drop;
                this.amountAdult = amountAdult;
                this.amountBaby = amountBaby;
            }
        }
    }
}
