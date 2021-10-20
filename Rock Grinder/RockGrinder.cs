using Harmony;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RockGrinder
{
    public class RockGrinder : KMonoBehaviour
    {
        public bool grindCritters;
        public bool grindDupes;
        private Extents pickupableExtents;
        private HandleVector<int>.Handle pickupablesChangedEntry;
        [MyCmpReq] Building building;
        [MyCmpReq] ComplexFabricator fabricator;
        Storage fabricatorStorage;
        protected override void OnSpawn()
        {
            base.OnSpawn();
            fabricatorStorage = fabricator.inStorage;
            StartPartitioner();
        }
        private void StartPartitioner()
        {
            int topLeftCell = Grid.CellAbove(building.NaturalBuildingCell());
            pickupableExtents = new Extents(topLeftCell, new CellOffset[] {
              new CellOffset(0, 0),
              new CellOffset(1, 0),
              new CellOffset(2, 0)});

            pickupablesChangedEntry = GameScenePartitioner.Instance.Add(
                "RockGrinder.PickupablesChanged", 
                gameObject, pickupableExtents, 
                GameScenePartitioner.Instance.pickupablesChangedLayer, 
                OnPickupablesChanged);
        }

        private void OnPickupablesChanged(object obj)
        {
            Pickupable p = obj as Pickupable;
            if ((IsFallingObject(p) || IsFallingCritter(p)) && !IsDupe(p))
            {
                foreach(ComplexRecipe recipe in fabricator.GetRecipes())
                {
                    if(recipe.ingredients[0].material == p.PrefabID())
                    {
                        int count = fabricator.GetRecipeQueueCount(recipe);
                        fabricator.SetRecipeQueueCount(recipe, Mathf.FloorToInt(p.TotalAmount / recipe.ingredients[0].amount + count));
                        fabricatorStorage.Store(p.gameObject, true);

                        ForceRefreshFetchOrders();
                        break;
                    }
                }
            }
        }

        private void ForceRefreshFetchOrders()
        {
            foreach (FetchList2 fetchList in fabricator.DebugFetchLists)
                fetchList.Cancel("cancel all orders");
            fabricator.DebugFetchLists.Clear();

            Traverse.Create(fabricator).Method("RefreshQueue").GetValue();
            fabricator.SetQueueDirty();
        }

        private bool IsFallingObject(Pickupable p) => GameComps.Fallers.Has(p.gameObject);
        private bool IsDupe(Pickupable p) => p.KPrefabID.HasTag(GameTags.DupeBrain);
        private bool IsFallingCritter(Pickupable p)
        {
            return p.HasTag(GameTags.CreatureBrain) && GameComps.Gravities.Has(p.gameObject);
        }
    }
}
