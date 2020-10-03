using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModularStorage
{
    public class ModularStorageManager : KMonoBehaviour
    {
        public static ModularStorageManager Instance { get; private set; }
        public Dictionary<int, MSController> controllers;

        protected override void OnPrefabInit()
        {
            Instance = this;
            controllers = new Dictionary<int, MSController>();
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            MSGrid.networks = Enumerable.Repeat(-1, Grid.CellCount).ToArray();
        }

        protected override void OnCleanUp()
        {
            Instance = null;
        }

        public enum StorageType
        {
            None,
            Gas,
            Liquid,
            Solid
        }
    }
}
