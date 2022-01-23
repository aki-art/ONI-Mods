using HarmonyLib;
using ProcGenGame;
using System;
using System.Collections.Generic;

namespace Terraformer
{
    /*
    public class Terraformer : KMonoBehaviour
    {
        int worldId;

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

        private bool SuccessCB(StringKey stringKeyRoot, float completePercent, WorldGenProgressStages.Stages stage)
        {
            return true;
        }

        public void PopulateWorld(WorldContainer world)
        {

        }
    }
    */
    }
