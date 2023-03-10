using ProcGen;
using Randomizer.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using static PointGenerator;

namespace Randomizer.Content.Scripts.Generators.YamlWorld
{
    public class WorldGenerator : ProceduralGenerator<ProcGen.World>
    {
        private readonly bool startWorld;
        public List<WeightedSubworldName> subWorlds;

        public WorldGenerator(SeededRandom rng, bool startWorld) : base(rng, "worlds", "world")
        {
            Generate(rng);
            this.startWorld = startWorld;
        }

        public void Generate(SeededRandom rng)
        {
            var centerSubWorld = new SubWorldGenerator(rng, ElementCollector.DIG_HARDNESS.SOFT, Temperature.Range.Room, true).FullName();
            var ring1a = new SubWorldGenerator(rng, ElementCollector.DIG_HARDNESS.MEDIUM, Temperature.Range.HumanWarm, false).FullName();
            var ring1b = new SubWorldGenerator(rng, ElementCollector.DIG_HARDNESS.MEDIUM, Temperature.Range.Chilly, false).FullName(); ;
            var ring2a = new SubWorldGenerator(rng, ElementCollector.DIG_HARDNESS.HARD, Temperature.Range.HumanHot, false).FullName(); ;
            var ring2b = new SubWorldGenerator(rng, ElementCollector.DIG_HARDNESS.HARD, Temperature.Range.HumanHot, false).FullName(); ;
            var ring2c = new SubWorldGenerator(rng, ElementCollector.DIG_HARDNESS.HARD, Temperature.Range.HumanWarm, false).FullName(); ;
            var core = new SubWorldGenerator(rng, ElementCollector.DIG_HARDNESS.SUPERHARD, Temperature.Range.ExtremelyHot, false).FullName(); ;
            var surface = new SubWorldGenerator(rng, ElementCollector.DIG_HARDNESS.HARD, Temperature.Range.ExtremelyCold, false, new()
            {
                WorldGenTags.ErodePointToBorderInv.ToString()
            }).FullName(); ;

            subWorlds = new()
            {
                new(centerSubWorld, 1f),
                new(ring1a, 1f),
                new(ring1b, 1f),
                new(ring2a, 1f),
                new(ring2b, 1f),
                new(ring2c, 1f),
                new(core, 1f),
                new(surface, 1f),
                new("subworlds/space/Space", 1f)
            };

            foreach (var subworld in subWorlds)
            {
                subworld.maxCount = 999;
            };

            foreach(var subworld in subWorlds)
            {
                subworld.maxCount = 999;
            }

            var world = new ProcGen.World()
            {
                name = "STRINGS.WORLDS.TINYICE.NAME",
                description = "STRINGS.WORLDS.TINYICE.NAME",
                worldTraitScale = 0.4f,
                worldsize = new Vector2I(160, 256),
                layoutMethod = ProcGen.World.LayoutMethod.PowerTree,
                disableWorldTraits = true,
                defaultsOverrides = new DefaultSettings()
                {
                    data = new Dictionary<string, object>()
                    {
                        { "DrawWorldBorder", "true" },
                        { "DrawWorldBorderForce", "false" },
                        { "WorldBorderThickness", "1" },
                        { "WorldBorderRange", "0" },
                        { "OverworldDensityMin", "16" },
                        { "OverworldDensityMax", "16" },
                        { "OverworldAvoidRadius", "5" },
                        { "OverworldSampleBehaviour", "PoissonDisk" },
                        { "OverworldMinNodes", "1" },
                    },
                    startingWorldElements = new()
                },

                startSubworldName = centerSubWorld,
                startingBaseTemplate = "bases/sandstoneBase",
                subworldFiles = subWorlds,

                unknownCellsAllowedSubworlds = new()
                {
                    // starter
                    new ProcGen.World.AllowedCellsFilter()
                    {
                        tagcommand = ProcGen.World.AllowedCellsFilter.TagCommand.Default,
                        subworldNames = new()
                        {
                            centerSubWorld  
                        },
                        command = ProcGen.World.AllowedCellsFilter.Command.Replace
                    },
                    // ring 1
                    new ProcGen.World.AllowedCellsFilter()
                    {
                        tagcommand = ProcGen.World.AllowedCellsFilter.TagCommand.DistanceFromTag,
                        tag = WorldGenTags.AtStart.ToString(),
                        minDistance = 1,
                        maxDistance = 2,
                        subworldNames = new()
                        {
                            ring1a,
                            ring1b
                        },
                        command = ProcGen.World.AllowedCellsFilter.Command.Replace
                    },
                    // ring 2
                    new ProcGen.World.AllowedCellsFilter()
                    {
                        tagcommand = ProcGen.World.AllowedCellsFilter.TagCommand.DistanceFromTag,
                        tag = WorldGenTags.AtStart.ToString(),
                        minDistance = 3,
                        maxDistance = 99,
                        subworldNames = new()
                        {
                            ring2a,
                            ring2b,
                            ring2c,
                        },
                        command = ProcGen.World.AllowedCellsFilter.Command.Replace
                    },
                    // space
                    new ProcGen.World.AllowedCellsFilter()
                    {
                        tagcommand = ProcGen.World.AllowedCellsFilter.TagCommand.DistanceFromTag,
                        tag = WorldGenTags.AtSurface.ToString(),
                        minDistance = 0,
                        maxDistance = 1,
                        subworldNames = new()
                        {
                            "subworlds/space/Space"
                        },
                        command = ProcGen.World.AllowedCellsFilter.Command.Replace
                    },
                    // surface
                    new ProcGen.World.AllowedCellsFilter()
                    {
                        tagcommand = ProcGen.World.AllowedCellsFilter.TagCommand.DistanceFromTag,
                        tag = WorldGenTags.AtSurface.ToString(),
                        minDistance = 2,
                        maxDistance = 2,
                        subworldNames = new()
                        {
                            surface
                        },
                        command = ProcGen.World.AllowedCellsFilter.Command.Replace
                    },
                    // core
                    new ProcGen.World.AllowedCellsFilter()
                    {
                        tagcommand = ProcGen.World.AllowedCellsFilter.TagCommand.DistanceFromTag,
                        tag = WorldGenTags.AtDepths.ToString(),
                        minDistance = 0,
                        maxDistance = 1,
                        subworldNames = new()
                        {
                            core
                        },
                        command = ProcGen.World.AllowedCellsFilter.Command.Replace
                    }
                },

                filePath = FullName()
            };


            SettingsCache.worlds.worldCache[FullName()] = world;

            WriteYaml(world, name);
        }
    }
}
