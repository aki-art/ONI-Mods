using System;
using System.Collections.Generic;
using UnityEngine;

namespace DuctTapePipes.Buildings
{
    public abstract class Leech : KMonoBehaviour
    {
        public Storage HostStorage { get; protected set; }

        [SerializeField]
        public Storage storage;

        [MyCmpReq]
        private Building building;

        [MyCmpReq]
        private Operational operational;

        private LeechStages.Instance smi;

        protected override void OnSpawn()
        {
            base.OnSpawn();

            smi = new LeechStages.Instance(this);
            smi.StartSM();
        }

        public virtual void Disconnect()
        {
            smi.sm.disconnect.Trigger(smi);
        }

        protected bool HostExists()
        {
            return HostStorage is object && HostStorage.IsEndOfLife();
        }

        internal bool FindStorage()
        {
            for(int layer = 0; layer < Grid.ObjectLayers.Length; layer++)
            {
                if (layer == (int)building.Def.ObjectLayer) continue; // ignore self

                if (Grid.ObjectLayers[layer].TryGetValue(building.GetCell(), out GameObject go))
                {
                    if (go.TryGetComponent(out Storage storage)) return Connect(storage);
                }
            }

            return false;
        }

        protected virtual bool Connect(Storage storage)
        {
            HostStorage = storage;
            Subscribe(storage.gameObject, (int)GameHashes.OperationalChanged, OnHostOperationalChanged);
            return true;
        }

        public abstract void UpdateStorage();

        public virtual void OnHostOperationalChanged(object obj)
        {
            if(obj is Operational) operational.SetActive((obj as Operational).IsOperational);
        }

        internal bool IsHostOperational()
        {
            return !(HostStorage is null) && HostStorage.GetComponent<Operational>().IsOperational;
        }
    }
}
