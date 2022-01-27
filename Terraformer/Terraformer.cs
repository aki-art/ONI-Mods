using FUtility;
using ProcGenGame;
using System;
using System.Collections.Generic;

namespace Terraformer
{
    public class Terraformer : KMonoBehaviour
    {
        int worldId;

        /*
            public WorldContainer CreateNewWorld(AxialI location)
            {
                WorldGen worldGen = new WorldGen("", new List<string>(), false);

                Vector2I worldsize = worldGen.Settings.world.worldsize;
                worldGen.SetWorldSize(worldsize.x, worldsize.y);

                Grid.GetFreeGridSpace(worldsize, out Vector2I worldPlacement);

                worldGen.SetPosition(new Vector2I(worldPlacement.x, worldPlacement.y));
                SaveLoader.Instance.ClusterLayout.worlds.Add(worldGen);

                worldId = Traverse.Create(ClusterManager.Instance).Method("CreateAsteroidWorldContainer", new Type[] { typeof(WorldGen) }).GetValue<int>(worldGen);

                Vector2I position = worldGen.GetPosition();
                Vector2I vector2I = position + worldGen.GetSize();

                for (int i = position.y; i < vector2I.y; i++)
                {
                    for (int j = position.x; j < vector2I.x; j++)
                    {
                        int num = Grid.XYToCell(j, i);
                        Grid.WorldIdx[num] = (byte)worldId;
                        Pathfinding.Instance.AddDirtyNavGridCell(num);
                    }
                }

                Sim.Cell[] arg = null;
                Sim.DiseaseCell[] arg2 = null;

                //GridSettings.Reset(worldGen.GetSize().x, worldGen.GetSize().y);
                worldGen.GenerateNoiseData(SuccessCB);
                worldGen.GenerateOffline();
                worldGen.FinalizeStartLocation();

                if (!worldGen.RenderOffline(true, ref arg, ref arg2, worldId, false))
                {
                   // thread = null;
                    //return;
                }

                //ClusterManager.Instance.get

                //SaveLoader.Instance.ClusterLayout.PerWorldGenCompleteCallback.


                return ClusterManager.Instance.GetWorld(worldId);
            }


        */
        private bool SuccessCB(StringKey stringKeyRoot, float completePercent, WorldGenProgressStages.Stages stage)
        {
            return true;
        }

        public void Generate(WorldContainer world, string worldName)
        {
            WorldGen worldGen = new WorldGen(worldName, new List<string>(), false);
            worldGen.SetWorldSize(world.WorldSize.x, world.WorldSize.y);
            //worldGen.SetPosition(world.WorldOffset);
            worldGen.SetPosition(Vector2I.zero);

            Log.Debuglog($"SET SIZE TO {world.WorldSize}");

            Sim.Cell[] arg = null;
            Sim.DiseaseCell[] arg2 = null;
            worldGen.isRunningDebugGen = true;

            Log.Debuglog("made worldgen");
            Log.Assert("stats", worldGen.stats);

            //worldGen.GenerateWorldData();
            //worldGen.DontGenerateNoiseData();
            //worldGen.succ
            //Log.Debuglog("GenerateNoiseData");
            worldGen.Initialise(SuccessCB, ErrorCB);
            worldGen.GenerateOffline();
            Log.Debuglog("GenerateOffline");

            worldGen.FinalizeStartLocation();
            Log.Debuglog("FinalizeStartLocation");

            if (!worldGen.RenderOffline(true, ref arg, ref arg2, worldId, false))
            {
                // thread = null;
                //return;
            }
            Log.Debuglog("RenderOffline");
        }

        private void ErrorCB(OfflineWorldGen.ErrorInfo obj)
        {
            Log.Debuglog("error cb");
            Log.Debuglog(obj.errorDesc);
        }
    }
}
