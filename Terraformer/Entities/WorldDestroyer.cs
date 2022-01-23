using Delaunay.Geo;
using FUtility;
using HarmonyLib;
using Klei;
using KSerialization;
using ProcGen;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Terraformer.Entities
{
    public class WorldDestroyer : KMonoBehaviour
    {
        const int MAX_DELETE_ITERATION = 10;

        private WorldContainer worldContainer;

        public Action<AxialI> onWorldDestroyed;

        int worldId;

        private bool success;
        private int deleteAttempts = 0;
        private Traverse CancelTool_OnDragTool;
        private Traverse CancelTool_OnDragComplete;
        private Traverse World_zoneRenderData_OnShadersReloaded;

        int minX;
        int maxX;
        int minY;
        int maxY;

        [Serialize]
        private bool completeDeletion;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            CancelTool_OnDragTool = Traverse.Create(CancelTool.Instance).Method("OnDragTool", new Type[] { typeof(int), typeof(int) });
            CancelTool_OnDragComplete = Traverse.Create(CancelTool.Instance).Method("OnDragComplete", new Type[] { typeof(Vector3), typeof(Vector3) });
            World_zoneRenderData_OnShadersReloaded = Traverse.Create(World.Instance.zoneRenderData).Method("OnShadersReloaded");
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            worldId = this.GetMyWorldId();
            Mod.WorldDestroyers.Add(this);
        }

        public void Detonate(bool destroyCluster)
        {
            if (this is null || this.GetMyWorld() is null)
            {
                return;
            }

            worldContainer = this.GetMyWorld();

            if(worldContainer is null || worldContainer.IsStartWorld || SpaceCraftsPresent(worldContainer))
            {
                return;
            }

            minX = (int)worldContainer.minimumBounds.x;
            maxX = (int)worldContainer.maximumBounds.x;
            minY = (int)worldContainer.minimumBounds.y;
            maxY = (int)worldContainer.maximumBounds.y;

            DeleteWarpPortalLeadingHere();
            CancelOrders();
            DeleteBuildings(worldContainer);
            //DeletePrioritizables();
            RemoveCells(minX, maxX, minY, maxY);

            if (destroyCluster)
            {
                GameScheduler.Instance.ScheduleNextFrame("delete cluster", obj => DestroyClusterEntity());
            }
            else
            {
                RemoveWorldZones();
                StartCoroutine(KeepTryingToDeletePickupables());
            }
        }

        private void DestroyClusterEntity()
        {
            // change active world to a safe one
            if (ClusterManager.Instance.activeWorldId == worldId)
            {
                ClusterManager.Instance.SetActiveWorld(ClusterManager.Instance.GetStartWorld().id);
            }

            //WorldSelector.Instance.worldRows.Remove(worldId);

            // don't Grid.FreeGridSpace(worldContainer.WorldOffset, worldContainer.WorldSize);

            GameScheduler.Instance.ScheduleNextFrame("delete cluster", obj => DeleteCluster());
            
        }

        private void DeleteCluster()
        {
            if (ClusterManager.Instance.activeWorldId == worldId || ClusterManager.Instance.activeWorld == worldContainer)
            {
                var startWorld = ClusterManager.Instance.GetStartWorld();
                int start = startWorld.id;

                ClusterManager.Instance.SetActiveWorld(start);

                var center = startWorld.maximumBounds / 2f; //(startWorld.maximumBounds - startWorld.minimumBounds) / 2f + startWorld.minimumBounds;
                // Have to do this manually, or the game renderer will cry
                CameraController.Instance.SetTargetPos(center, 8f, false);
                CameraController.Instance.SetPosition(center);
            }

            GetComponent<KSelectable>()?.Unselect();

            AxialI location = worldContainer.GetMyWorldLocation();

            if (worldContainer.TryGetComponent(out ClusterGridEntity clusterEntity))
            {
                //ClusterGrid.Instance.UnregisterEntity(clusterEntity);
                clusterEntity.Trigger((int)GameHashes.QueueDestroyObject, clusterEntity.gameObject);
                clusterEntity.DeleteObject();
            }

            worldContainer.GetComponent<WorldInventory>()?.DeleteObject();
            //ClusterManager.Instance.Trigger((int)GameHashes.WorldRemoved, worldId);

            Destroy(worldContainer);
            Destroy(this);

            onWorldDestroyed?.Invoke(location);
        }

        // Keeps attempting to delete all contents of a world until there is nothing left (or we ran out of max attempts)
        private IEnumerator KeepTryingToDeletePickupables()
        {
            while(!success && deleteAttempts++ < MAX_DELETE_ITERATION)
            {
                if(Components.Pickupables.GetWorldItems(worldId).Count == 0)
                {
                    Log.Info($"World {worldContainer.worldName} was destroyed. It took {deleteAttempts} iterations.");

                    success = true;
                    yield break;
                }

                DeletePickupables(worldContainer);
                yield return new WaitForSeconds(0.2f);
            }

            if(!success)
            {
                Log.Warning($"World {worldContainer.worldName} could not be fully destroyed, giving up. (is something creating an infinitely respawning item?).");
                Log.Warning(string.Join(", ", Components.Pickupables.GetWorldItems(worldId)));
            }



            yield return null;
        }

        private void CancelOrders()
        {
            CancelTool_OnDragComplete.GetValue((Vector3)worldContainer.minimumBounds, (Vector3)worldContainer.maximumBounds);
        }

        private void RemoveCells(int minX, int maxX, int minY, int maxY)
        {
            var entombedItemVis = Game.Instance.GetComponent<EntombedItemVisualizer>();

            for (int x = minX + 1; x < maxX; x++)
            {
                for (int y = minY + 1; y < maxY; y++)
                {
                    int cell = Grid.XYToCell(x, y);
                    if (Grid.IsValidCellInWorld(cell, worldId))
                    {
                        CancelTool_OnDragTool.GetValue(cell, 0);

                        if (entombedItemVis.IsEntombedItem(cell))
                        {
                            entombedItemVis.RemoveItem(cell);
                        }

                        DestroyCell(cell);
                    }
                }
            }
        }

        private void RemoveWorldZones()
        {
            var overworldCell = new WorldDetailSave.OverworldCell
            {
                poly = new Polygon(new Rect(worldContainer.WorldOffset, worldContainer.WorldSize)),
                zoneType = SubWorld.ZoneType.Space
            };

            SaveLoader.Instance.clusterDetailSave.overworldCells.Add(overworldCell);

            worldContainer.ClearWorldZones();

            World_zoneRenderData_OnShadersReloaded.GetValue();
        }

        public void DestroyCell(int cell)
        {
            List<GameObject> objects = new List<GameObject>
            {
                Grid.Objects[cell, (int)ObjectLayer.Backwall],
                Grid.Objects[cell, (int)ObjectLayer.Wire],
                Grid.Objects[cell, (int)ObjectLayer.Building],
                Grid.Objects[cell, (int)ObjectLayer.GasConduit],
                Grid.Objects[cell, (int)ObjectLayer.LiquidConduit],
                Grid.Objects[cell, (int)ObjectLayer.Minion],

                // additional layers
                Grid.Objects[cell, (int)ObjectLayer.SolidConduit],
                Grid.Objects[cell, (int)ObjectLayer.FoundationTile],
                Grid.Objects[cell, (int)ObjectLayer.LogicWire],
                Grid.Objects[cell, (int)ObjectLayer.Pickupables]
            };

            foreach (GameObject gameObject in objects)
            {
                if (gameObject != null)
                {
                    Destroy(gameObject);
                }
            }

            DebugTool.Instance.ClearCell(cell);

            if (ElementLoader.elements[Grid.ElementIdx[cell]].id == SimHashes.Void)
            {
                SimMessages.ReplaceElement(cell, SimHashes.Void, CellEventLogger.Instance.DebugTool, 0f, 0f, byte.MaxValue, 0, -1);
                return;
            }
            SimMessages.ReplaceElement(cell, SimHashes.Vacuum, CellEventLogger.Instance.DebugTool, 0f, 0f, byte.MaxValue, 0, -1);
        }

        private void DeleteWarpPortalLeadingHere()
        {
            foreach (WarpPortal portal in FindObjectsOfType<WarpPortal>())
            {
                int targetWorldId = Traverse.Create(portal).Method("GetTargetWorldID").GetValue<int>();
                if (targetWorldId == worldId)
                {
                    portal.CancelAssignment();
                    Destroy(portal);
                }
            }
        }

        private bool SpaceCraftsPresent(WorldContainer worldContainer)
        {
            AxialI location = worldContainer.GetMyWorldLocation();
            foreach (Clustercraft clustercraft in Components.Clustercrafts)
            {
                if(clustercraft.Location == location)
                {
                    Log.Warning("Cannot destroy world: Rocket is landed here.");
                    return true;
                }

                if (clustercraft.Destination == location)
                {
                    Log.Warning("Cannot destroy world: Rocket is headed here.");
                    return true;
                }
            }

            return false;
        }

        private void DeletePrioritizables()
        {
            foreach (var p in Components.Prioritizables.GetWorldItems(worldId))
            {
                p.RemoveRef();
            }
        }

        private void DeleteBuildings(WorldContainer world)
        {
            ListPool<ScenePartitionerEntry, ClusterManager>.PooledList pooledList = ListPool<ScenePartitionerEntry, ClusterManager>.Allocate();
            GameScenePartitioner.Instance.GatherEntries((int)world.minimumBounds.x, (int)world.minimumBounds.y, world.Width, world.Height, GameScenePartitioner.Instance.completeBuildings, pooledList);

            foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
            {
                if (scenePartitionerEntry.obj is BuildingComplete buildingComplete)
                {
                    buildingComplete.DeleteObject();
                }
            }
            pooledList.Clear();
        }

        private void DeletePickupables(WorldContainer world)
        {
            ListPool<ScenePartitionerEntry, ClusterManager>.PooledList pooledList = ListPool<ScenePartitionerEntry, ClusterManager>.Allocate();
            GameScenePartitioner.Instance.GatherEntries((int)world.minimumBounds.x, (int)world.minimumBounds.y, world.Width, world.Height, GameScenePartitioner.Instance.pickupablesLayer, pooledList);

            foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
            {
                if (scenePartitionerEntry.obj is Pickupable pickupable)
                {
                    pickupable.DeleteObject();
                }
            }
            pooledList.Clear();
        }
    }
}
