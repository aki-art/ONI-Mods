using HarmonyLib;
using KMod;
using System.Collections.Generic;

namespace DecorPackB
{
    public class Mod : UserMod2
    {
        public static Dictionary<SimHashes, TreasureChances> treasureChances;
        public const string PREFIX = "DecorPackB_";

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);

            treasureChances = new Dictionary<SimHashes, TreasureChances>();

            treasureChances.Add(SimHashes.Fossil, new TreasureChances()
            {
                treasures = new List<TreasureChances.Treasure>()
                {
                    new TreasureChances.Treasure()
                    {
                        weight = 1,
                        tag = CrabShellConfig.ID
                    }
                }
            });

            treasureChances.Add(SimHashes.SandStone, new TreasureChances()
            {
                treasures = new List<TreasureChances.Treasure>()
                {
                    new TreasureChances.Treasure()
                    {
                        weight = 1,
                        tag = PrickleGrassConfig.SEED_ID
                    },

                    new TreasureChances.Treasure()
                    {
                        weight = 1,
                        tag = HatchConfig.EGG_ID
                    },
                }
            });
        }
    }
}
