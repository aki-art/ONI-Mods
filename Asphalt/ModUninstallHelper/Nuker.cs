using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asphalt
{
    public class Nuker
    {
        private static readonly BuildingDef sandstoneDef = Assets.GetBuildingDef(TileConfig.ID);
        private static Notifier notifier;

        private static void SpawnSandstoneTile(int cell, Orientation buildingOrientation, Storage _, IList<Tag> selectedElements, float temperature)
        {
            sandstoneDef.Build(cell, buildingOrientation, null, selectedElements, temperature, false, GameClock.Instance.GetTime() + 1);
            World.Instance.blockTileRenderer.Rebuild(ObjectLayer.FoundationTile, cell);
        }

        private static void ChangeTileToSandstone(BuildingComplete building)
        {
            // Save the asphalt tile-s information
            Orientation buildingOrientation = building.Orientation;
            var selectedElements = new List<Tag>() { "SandStone".ToTag() };
            float temperature = building.primaryElement.Temperature;
            int cell = Grid.PosToCell(building.transform.GetPosition());

            // Replacing SimCell Occupier
            SimCellOccupier simCellOccupier = building.GetComponent<SimCellOccupier>();
            simCellOccupier.DestroySelf(delegate { SpawnSandstoneTile(cell, buildingOrientation, null, selectedElements, temperature); });

            // Destroy Asphalt
            HashSetPool<GameObject, SandboxDestroyerTool>.PooledHashSet objects_to_destroy = HashSetPool<GameObject, SandboxDestroyerTool>.Allocate();
            objects_to_destroy.Add(building.gameObject);
            Util.KDestroyGameObject(building.gameObject);
            objects_to_destroy.Recycle();
        }

        public static void ChangeAllAsphaltToSandstoneTiles()
        {
            Log.Info("Changing tiles");

            try
            {
                foreach (BuildingComplete building in Components.BuildingCompletes)
                {
                    if (building.name == AsphaltConfig.ID + "Complete")
                        ChangeTileToSandstone(building);
                }

            }
            catch (Exception e)
            {
                Log.Warning(e);
            }

            RebuildTiles();
        }

        // Stops bitumen production for already spawned refineries
        public static void StopBitumenProductionRuntime()
        {
            foreach(BuildingComplete building in Components.BuildingCompletes)
            if (building.name == OilRefineryConfig.ID + "Complete")
            {
                Log.Debuglog("Halting bitumen production");
                ElementConverter elementConverter = building.GetComponent<ElementConverter>();

                int keyIndex = Array.FindIndex(elementConverter.outputElements, w => w.elementHash == SimHashes.Bitumen);
                var outPutElementList = new List<ElementConverter.OutputElement>(elementConverter.outputElements);
                outPutElementList.RemoveAt(keyIndex);
                elementConverter.outputElements = outPutElementList.ToArray();
            }
        }
        public static void RemoveAllBitumenFromWorld(bool refund)
        {
            if (Components.Pickupables == null) return;
            float amount = 0;

            foreach (Pickupable pickupable in Components.Pickupables)
            {
                if (pickupable.PrimaryElement.Element == ElementLoader.FindElementByHash(SimHashes.Bitumen))
                {
                        amount += pickupable.TotalAmount;
                        PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, UI.SANDBOXTOOLS.CLEARFLOOR.DELETED, pickupable.transform);
                        Util.KDestroyGameObject(pickupable.gameObject);            
                }
            }

            Log.Info($"Removed {amount}kg of Bitumen from the world.");

            if (refund && amount > 0)
            {
                var location = Vector3.zero;

                // Primary location: at the printing pod
                var printingPod = Components.BuildingCompletes.Items.Find(x => x.name == HeadquartersConfig.ID + "Complete");
                if (printingPod != null)
                    location = Grid.CellToPosCBC(printingPod.GetCell(), Grid.SceneLayer.Ore);
                else
                {
                    // Secondary location: at a random dupe-s feet
                    var randomDupe = Components.LiveMinionIdentities.Items[0];
                    if (randomDupe != null)
                        location = randomDupe.gameObject.transform.position;
                    else
                    {
                        // Tertiary location: at any random building
                        for (int i = Grid.CellCount; i > 0; i--)
                        {
                            var randomBuilding = Components.BuildingCompletes.Items[0];
                            if (randomBuilding != null)
                                location = Grid.CellToPosCBC(randomBuilding.GetCell(), Grid.SceneLayer.Ore);
                            else Log.Info("No place found to spawn the refunded oil.");
                        }
                    }
                }

                // Spawn Oil
                var element = ElementLoader.FindElementByHash(SimHashes.CrudeOil);
                var result = element.substance.SpawnResource(location, amount / 4, 300, 0, 0, false, false, false);
                PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, ELEMENTS.CRUDEOIL.NAME, result.transform);

                // Give a notification
                var notification = new Notification(
                    title: "Oil Refunded!",
                    type: NotificationType.Good,
                    group: HashedString.Invalid,
                    tooltip: null,
                    tooltip_data: null,
                    expires: true,
                    delay: 0f,
                    custom_click_callback: null,
                    custom_click_data: null,
                    click_focus: result.transform);

                notifier = result.gameObject.AddComponent<Notifier>();
                notifier.Add(notification);
            }
        }

        private static void RebuildTiles()
        {
            for (int i = 0; i < Grid.CellCount; i++)
            {
                World.Instance.blockTileRenderer.Rebuild(ObjectLayer.FoundationTile, i);
            }
            Log.Info("Rebuilt Tiles.");
        }

    }
}
