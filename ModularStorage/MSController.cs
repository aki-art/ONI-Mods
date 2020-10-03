using System.Collections.Generic;
using UnityEngine;

namespace ModularStorage
{
    public class MSController : KMonoBehaviour, ISidescreenButtonControl
    {
        [MyCmpReq]
        Storage mainStorage;
        Dictionary<int, StorageModule> connectedModules;
        HashSet<int> connectedCells;
        [SerializeField]
        public Tag moduleTag;
        private Color debugColor;
        private List<KBatchedAnimController> debugOverlays;
        private bool isDebugVisible = false;


        public string Name => GetComponent<UserNameable>().savedName;
        public int ID;
        public string SidescreenTitleKey => "-";

        public string SidescreenStatusMessage => "-";

        public string SidescreenButtonText => isDebugVisible ? "debug Hide" : "debug Show";

        public void OnSidescreenButtonPressed()
        {
            if (isDebugVisible)
                HideDebugOverlay();
            else 
                DrawDebugOverlay();
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            debugOverlays = new List<KBatchedAnimController>();
            debugColor = Random.ColorHSV(0, 1, 1, 1, 0.5f, 1);

            connectedCells = new HashSet<int>();
            connectedModules = new Dictionary<int, StorageModule>();

            RefreshConnections();
            ID = Grid.PosToCell(this);

            ModularStorageManager.Instance.controllers.Add(ID, this);
        }

        protected override void OnCleanUp()
        {
            DisconnectAll();
            ModularStorageManager.Instance.controllers.Remove(ID);

            base.OnCleanUp();
        }

        public void DrawDebugOverlay()
        {
            if(debugOverlays != null)
            {
                HideDebugOverlay();
                debugOverlays.Clear();
            }

            foreach (int cell in connectedCells)
            {
                KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect(
                    "transferarmgrid_kanim",
                    Grid.CellToPosCCC(cell, Grid.SceneLayer.FXFront),
                    null, false,
                    Grid.SceneLayer.FXFront);

                kbatchedAnimController.destroyOnAnimComplete = false;
                kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.Always;
                kbatchedAnimController.gameObject.SetActive(true);
                kbatchedAnimController.TintColour = debugColor;
                kbatchedAnimController.Play("grid_loop", KAnim.PlayMode.Loop);

                debugOverlays.Add(kbatchedAnimController);
            }

            isDebugVisible = true;
        }

        public void HideDebugOverlay()
        {
            if(debugOverlays != null)
            {
                foreach (var kbac in debugOverlays)
                {
                    kbac.Stop();
                    Util.KDestroyGameObject(kbac);
                }
            }

            debugOverlays.Clear();

            isDebugVisible = false;
        }

        public void RefreshConnections()
        {
            DisconnectAll();
            connectedCells = GetConnectedCells();

            foreach (int cell in connectedCells)
            {
                GameObject go = Grid.Objects[cell, (int)ObjectLayer.Building];
                if(go != null)
                    ConnectModule(go.GetComponent<StorageModule>(), cell);
            }
            Debug.Log(connectedCells.Count);
        }

        public HashSet<int> GetConnectedCells()
        {
            var result = new HashSet<int>();
            GameUtil.FloodFillConditional(Grid.PosToCell(this), IsPartOfNetwork, new HashSet<int>(), result);
            return result;
        }

        protected virtual bool IsPartOfNetwork(int cell)
        {
            var go = Grid.Objects[cell, (int)ObjectLayer.Building];
            return go != null && go.HasTag(moduleTag);
        }

        public void ConnectModule(StorageModule module, int id)
        {
            Debug.Log($"Connecting module {id}");
            if (!IsConnected(id))
            {
                Debug.Log("It was not part of the system yet.");
                connectedModules.Add(id, module);
                connectedCells.Add(id);
                module.SetController(this);
            }
            Debug.Log("It was already added.");
        }

        private void DisconnectAll()
        {
            foreach (var module in connectedModules)
            {
                module.Value.RemoveController();
            }

            connectedModules.Clear();
            connectedCells.Clear();
        }

        public void DisconnectModule(int id)
        {
            if(connectedModules.TryGetValue(id, out StorageModule module))
            {
                module.RemoveController();
            }

            connectedModules.Remove(id);
            connectedCells.Remove(id);
        }

        public bool IsConnected(int id)
        {
            return connectedCells.Contains(id) || id == ID;
        }
    }
}
