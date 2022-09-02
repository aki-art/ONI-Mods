using FUtility;
using System.Collections.Generic;
using UnityEngine;

namespace ZipLine
{
    public class TetherVisualizer : KMonoBehaviour
    {
        public static TetherVisualizer Instance;

        [SerializeField]
        public Vector3 tetherOffset;

        public List<Tether> tethers = new List<Tether>();


        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Instance = this;
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();

            foreach(var tether in tethers)
            {
                Destroy(tether.line.gameObject);
            }

            tethers.Clear();

            Instance = null;
        }

        public void AddTether(int a, int b)
        {
            if(a == b)
            {
                Log.Warning("Zipline cannot connect into the same cell it originates from.");
                return;
            }

            foreach(var tether in tethers)
            {
                if(tether.a == a && tether.b == b)
                {
                    // already added
                    return;
                }
            }

            tethers.Add(new Tether(b, a, tetherOffset));
        }

        public void RemoveTether(int a, int b)
        {
            for (var i = 0; i < tethers.Count; i++)
            {
                var tether = tethers[i];
                if (tether.a == a && tether.b == b)
                {
                    Destroy(tether.line.gameObject);
                    tethers.RemoveAt(i);

                    return;
                }
            }
        }

        public struct Tether
        {
            public int a;
            public int b;
            public LineRenderer line;

            public Tether(int a, int b, Vector3 offset)
            {
                if(a > b)
                {
                    this.a = a;
                    this.b = b;
                }
                else
                {
                    this.a = b;
                    this.b = a;
                }

                var ropeGo = Instantiate(ModAssets.linePrefab); 

                line = ropeGo.GetComponent<LineRenderer>();

                line.positionCount = 2;
                line.SetPositions(new[]
                {
                    Grid.CellToPos(a) + offset,
                    Grid.CellToPos(b) + offset
                });

                line.gameObject.SetActive(true);
            }
        }
    }
}
