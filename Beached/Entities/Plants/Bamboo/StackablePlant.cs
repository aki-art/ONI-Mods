using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Beached.Entities.Plants.Bamboo
{
    public class StackablePlant : KMonoBehaviour
    {
        [SerializeField] 
        public Tag prefabTag;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

            Subscribe((int)GameHashes.NewGameSpawn, OnNewGameSpawn);
        }

        private void OnNewGameSpawn(object obj)
        {
            int maxHeight = GetHeightToCeiling(32);
            int height = UnityEngine.Random.Range(1, maxHeight);

            for(int i = 0; i < height; i++)
            {
                FUtility.Utils.Spawn(prefabTag, transform.position + i * Vector3.up, Grid.SceneLayer.Building);
            }
        }

        private bool CanGrowInto(int cell)
        {
            return Grid.IsValidCell(cell) && !Grid.IsSolidCell(cell);
        }

        private int GetHeightToCeiling(int max)
        {
            int y;
            for (y = 0; y <= max && CanGrowInto(Grid.OffsetCell(Grid.PosToCell(this), 0, y)); y++) { }

            return y;
        }
    }
}
