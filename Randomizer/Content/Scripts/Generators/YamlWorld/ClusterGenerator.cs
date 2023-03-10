using Klei;
using ProcGen;
using System.Linq;

namespace Randomizer.Content.Scripts.Generators.YamlWorld
{
    public class ClusterGenerator : ProceduralGenerator<ClusterLayout>
    {
        private ClusterLayout layout;

        public ClusterGenerator(SeededRandom rng, int size) : base(rng, "clusters", "cluster")
        {
            Log.Info("Generating Cluster data...");

            var startWorld = new WorldGenerator(rng, true);

            layout = new ClusterLayout()
            {
                name = "Randomizer.STRINGS.WORLDS.RANDOM_PRESET_DEFAULT.NAME",
                description = "Randomizer.STRINGS.WORLDS.RANDOM_PRESET_DEFAULT.DESCRIPTION",
                requiredDlcId = DlcManager.EXPANSION1_ID,
                menuOrder = 0,
                clusterCategory = ModDb.USERSAVED_CLUSTER_CATEGORY,
                coordinatePrefix = $"RAND-{rng.seed}",
                numRings = 12,
                startWorldIndex = 0,
                worldPlacements = new()
                {
                    // place start world
                    new WorldPlacement()
                    {
                        world = startWorld.FullName(),
                        locationType = WorldPlacement.LocationType.Startworld,
                        allowedRings = new(0, 0)
                    }
                }
            };

            WriteYaml(layout, name);

            var errors = ListPool<YamlIO.Error, ClusterGenerator>.Allocate();
            //SettingsCache.clusterLayouts.LoadFiles(Mod.worldGenFolder, DlcManager.VANILLA_ID, errors);

            foreach (YamlIO.Error error in errors)
            {
                YamlIO.LogError(error, true);
            }

            errors.Recycle();

            var referencedWorlds = layout.worldPlacements.Select(w => w.world).ToHashSet();
            SettingsCache.worlds.LoadReferencedWorlds(Mod.worldGenFolder, DlcManager.VANILLA_ID, referencedWorlds, errors);

            WorldRandomizerGenerator.currentCluster = FullName();
        }
    }
}
