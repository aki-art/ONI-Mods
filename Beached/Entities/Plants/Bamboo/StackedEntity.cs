using KSerialization;
using UnityEngine;

namespace Beached.Entities.Plants.Bamboo
{
    public class StackedEntity : KMonoBehaviour
    {
        [Serialize]
        public bool isBase; 

        private bool IsSolidBlock(int cell)
        {
            return Grid.IsSolidCell(cell);
        }

        public void Topple(bool uproot)
        {
            var cellAbove = Grid.CellAbove(Grid.PosToCell(this));

            if (Grid.ObjectLayers[(int)ObjectLayer.Plants].TryGetValue(cellAbove, out GameObject go))
            {
                if (go.TryGetComponent(out StackedEntity stackedEntityOnTop))
                {
                    stackedEntityOnTop.Topple(true);
                }
            }

            if(uproot)
            {
                GetComponent<Uprootable>().Uproot();
            }
        }

        protected override void OnSpawn()
        {
            if(IsSolidBlock(Grid.CellBelow(Grid.PosToCell(this))))
            {
                isBase = true;
                // check for solid changed
            }

            base.OnSpawn();
        }

        protected override void OnCleanUp()
        {
            Topple(false);
            base.OnCleanUp();
        }
    }
}
