using System.Collections.Generic;
using UnityEngine;

namespace ModularStorage
{
    public class StorageModule : KMonoBehaviour, ISim4000ms
    {
        [MyCmpAdd]
        Storage storage;
        public int cell;


        public MSController Controller { get; private set; }

        public Storage Storage => storage;

        public bool IsPartOfNetwork { get; private set; } = false;

        protected override void OnSpawn()
        {
            base.OnSpawn();
            cell = Grid.PosToCell(this);
            GetComponent<KSelectable>().AddStatusItem(ModAssets.NotConnectedModuleStatus, this);
            if (FindController(out MSController controller))
            {
                ConnectToNetWork(controller);
            }
        }

        protected override void OnCleanUp()
        {
            Disconnect();
            base.OnCleanUp();
        }

        public void ConnectToNetWork(MSController controller)
        {
            controller.ConnectModule(this, cell);
            SetController(controller);
        }

        public void SetController(MSController controller)
        {
            Disconnect();
            IsPartOfNetwork = true;
            Controller = controller;
            RefreshStatusItems();
            MSGrid.networks[cell] = Controller.ID;

            Debug.Log($"connected {cell} to {controller.Name}");
        }

        private void RefreshStatusItems()
        {
            if(IsPartOfNetwork)
            {
                GetComponent<KSelectable>().RemoveStatusItem(ModAssets.NotConnectedModuleStatus);
                GetComponent<KSelectable>().AddStatusItem(ModAssets.ConnectionStatus, this);
            }
            else
            {
                GetComponent<KSelectable>().AddStatusItem(ModAssets.NotConnectedModuleStatus);
                GetComponent<KSelectable>().RemoveStatusItem(ModAssets.ConnectionStatus);
            }
        }

        private bool FindController(out MSController controller)
        {
            List<int> cellsToCheck = new List<int>()
            {
                Grid.CellAbove(cell),
                Grid.CellRight(cell),
                Grid.CellBelow(cell),
                Grid.CellLeft(cell)
            };
           
            foreach (var c in cellsToCheck)
            {
                if(MSGrid.GetController(c, out controller))
                    return true;
            }

            controller = null;
            return false;
        }


        public void RemoveController()
        {
            Controller = null;
            IsPartOfNetwork = false;
        }

        public void Disconnect()
        {
            if (IsPartOfNetwork)
            {
                if (Controller != null)
                {
                    Controller.DisconnectModule(cell);
                    Debug.Log($"disconnected {cell} from {Controller.Name}");
                }

                GetComponent<KSelectable>().AddStatusItem(ModAssets.NotConnectedModuleStatus, this);
            }

            RemoveController();
        }

        public void Sim4000ms(float dt)
        {
            if (IsPartOfNetwork && Controller == null)
            {
                Disconnect();
            }
        }
    }
}
