using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Bomb
{
    class Fuse : KMonoBehaviour, ISim1000ms, ISidescreenButtonControl
    {
        [MyCmpReq]
        KBatchedAnimController animController;
        State state = State.Laid;
        State queuedState = State.Laid;

        private List<Fuse> FindNeighbours()
        {
            if (this == null) return null;

            var list = new List<GameObject>();
            int layer = (int)ObjectLayer.LogicWireTile;
            int cell = Grid.PosToCell(this);

            list.Add(Grid.Objects[Grid.CellAbove(cell), layer]);
            list.Add(Grid.Objects[Grid.CellBelow(cell), layer]);
            list.Add(Grid.Objects[Grid.CellLeft(cell), layer]);
            list.Add(Grid.Objects[Grid.CellRight(cell), layer]);

            if (list.Count == 0) return null;
            var fuses = new List<Fuse>();
            foreach (var item in list)
            {
                if (item == null) continue;
                Fuse fuse = item.GetComponent<Fuse>();
                if (fuse != null)
                    fuses.Add(fuse);
            }

            return fuses;
        }


        public void Sim1000ms(float dt)
        {
            if(state == State.Lit)
            {
                animController.TintColour = Color.red;

                List<Fuse> neighbours = FindNeighbours();
                if (neighbours == null || neighbours.Count == 0) return;

                foreach (var fuse in neighbours)
                {
                    if (fuse == null) return;
                    if(fuse.state == State.Laid)
                        fuse.Light();
                }
            }

            state = queuedState;
        }

        public string SidescreenTitleKey => "title";

        public string SidescreenStatusMessage => "";

        public string SidescreenButtonText => "Light";

        public void OnSidescreenButtonPressed()
        {
            Light();
        }

        private void Light()
        {
            queuedState = State.Lit;
        }
        public enum State
        {
            Laid,
            Lit,
            Burnt
        }
    }
}
