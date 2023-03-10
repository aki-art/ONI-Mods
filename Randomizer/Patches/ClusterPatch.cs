using HarmonyLib;
using Klei;
using ProcGen;
using ProcGenGame;
using Randomizer.Content.Scripts;
using Randomizer.Content.Scripts.Generators.YamlWorld;
using System.Collections.Generic;
using System.Linq;

namespace Randomizer.Patches
{
    public class ClusterPatch
    {
        [HarmonyPatch(typeof(Cluster), MethodType.Constructor, 
            typeof(string), 
            typeof(int), 
            typeof(List<string>), 
            typeof(bool),
            typeof(bool))]
        public class Cluster_Ctor_Patch
        {
            public static void Prefix(ref string name)
            {
                if(name.StartsWith("clusters/Randomizer_PlaceHolder"))
                {
                    name = WorldRandomizerGenerator.currentCluster;
                }

                var errors = ListPool<YamlIO.Error, ClusterGenerator>.Allocate();

                //SettingsCache.LoadFiles(Mod.worldGenFolder, "", errors);

                SettingsCache.LoadFiles(errors);
                foreach (YamlIO.Error error in errors)
                {
                    YamlIO.LogError(error, true);
                }

                errors.Recycle();

                var cluster = SettingsCache.clusterLayouts.clusterCache[name];
                if (cluster == null)
                {
                    Log.Debuglog($"cluster {name} is not loaded");
                }
                else
                {
                    Log.Debuglog($"Cluser {name} loaded. {cluster.disableStoryTraits}");

                    var referencedWorlds = new HashSet<string>(from worldPlacment in SettingsCache.clusterLayouts.clusterCache.Values.SelectMany((ClusterLayout clusterLayout) => clusterLayout.worldPlacements)
                                                                           select worldPlacment.world);

                    foreach(var referencedWorld in referencedWorlds)
                    {
                        Log.Debuglog(" -   " + referencedWorld);

                        var world = SettingsCache.worlds.worldCache[referencedWorld];
                        if(world == null)
                        {
                            Log.Debuglog($"world {world} is not loaded");
                        }
                        else
                        {
                            Log.Debuglog($"  world: {world.filePath} ok");
                            foreach(var filter in world.unknownCellsAllowedSubworlds)
                            {
                                Log.Debuglog($"{filter.tag} - {filter.subworldNames.Join()}");

                                foreach(var subworld in filter.subworldNames)
                                {
                                    if (!SettingsCache.subworlds.ContainsKey(subworld))
                                    {
                                        Log.Debuglog("NO SUBWORLD " + subworld);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
