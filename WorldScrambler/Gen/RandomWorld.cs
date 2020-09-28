using FUtility;
using ProcGen;
using System.Collections.Generic;
using static WorldScrambler.Gen.Mirrors.World;

namespace WorldScrambler.Gen
{
    public class RandomWorld
    {
        internal Mirrors.World generatedWorld;
        internal static int worldId;
        internal Dictionary<string, RandomSubworld> subWorlds;

        public static string Path => "worlds/Entropea_" + worldId;

        // Generates a basic empty world for the asteroid selection screen to pick up
        public void GenerateDefault()
        {
            worldId = ModAssets.EpochTime();
            generatedWorld = new Mirrors.World
            {
                name = "Entropea.STRINGS.WORLDS.ENTROPEA.NAME",
                description = "Entropea.STRINGS.WORLDS.ENTROPEA.DESCRIPTION",
                spriteName = "asteroid_entropea",
                coordinatePrefix = "ENTRO",
                difficulty = 2,
                tier = 2,
                disableWorldTraits = true,
                noStart = true,
                worldsize = new serializableVector2I(256, 384),
                layoutMethod = ProcGen.World.LayoutMethod.PowerTree
            };

            ModAssets.SaveYAML(generatedWorld, $"worldgen/{Path}.yaml");
        }

        public void Populate()
        {

        }

        public void Print()
        {
            Log.Info($"World info of " + Path);
            Log.Info($"Subworld count: {subWorlds.Count}");
        }
    }
}
