using FUtility;
using KSerialization;
using Slag.Content.Entities;
using UnityEngine;

namespace Slag.Cmps
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class MiteorSpawner : StateMachineComponent<MiteorSpawner.SMInstance>, ISaveLoadable
    {
        [SerializeField]
        public float duration;

        [SerializeField]
        public string prefabID;

        [SerializeField]
        public int spread;

        private Vector3 origin;

        protected override void OnSpawn()
        {
            base.OnSpawn();

            var bait = GetComponent<Deconstructable>().constructionElements?[1];
            if (bait != "Slag".ToTag())
            {
                enabled = false;
                return;
            }

            var world = ClusterManager.Instance.GetWorld(smi.myWorldId);

            var x = world.Width * Random.value + world.WorldOffset.x;
            x = Mathf.Clamp(x, world.minimumBounds.x + (spread + 1), world.maximumBounds.x - (spread + 1));
            var y = world.Height + world.WorldOffset.y - 1;
            var z = Grid.GetLayerZ(Grid.SceneLayer.FXFront);

            origin = new Vector3(x, y, z);

            smi.StartSM();
        }

        public class States : GameStateMachine<States, SMInstance, MiteorSpawner>
        {
            public State obstructed;
            public State bombarding;

            public override void InitializeStates(out BaseState default_state)
            {
                default_state = obstructed;

                obstructed
                    .ToggleStatusItem(Db.Get().BuildingStatusItems.NoSurfaceSight)
                    .UpdateTransition(bombarding, CanSeeSky, UpdateRate.SIM_4000ms);

                bombarding
                    .Update(DoABombard);
            }

            private void DoABombard(SMInstance smi, float dt)
            {
                if (smi.elapsedTime > smi.master.duration)
                {
                    Util.KDestroyGameObject(smi.gameObject);
                    return;
                }

                if (Random.value < 0.15f)
                {
                    SpawnComet(smi);
                }

                smi.elapsedTime += dt;
            }

            private GameObject SpawnComet(SMInstance smi)
            {
                var position = new Vector3(Random.Range(-smi.master.spread, smi.master.spread), 0) + smi.master.origin;

                var gameObject = Utils.Spawn(smi.master.prefabID, position);

                var egg = gameObject.GetComponent<EggComet>();
                egg.target = smi.transform.position;
                egg.targetAngleMargin = 12f;
                egg.RandomizeVelocity();

                return gameObject;
            }

            private bool CanSeeSky(SMInstance smi, float _)
            {
                var targetCell = Grid.PosToCell(smi);
                var worldContainer = targetCell >= 0 ? ClusterManager.Instance.GetWorld(Grid.WorldIdx[targetCell]) : null;
                var maxY = worldContainer == null ? Grid.HeightInCells : ((int)worldContainer.maximumBounds.y);
                var cell = targetCell;

                while (Grid.CellRow(cell) < maxY)
                {
                    if (!Grid.IsValidCell(cell) || Grid.Solid[cell])
                    {
                        return false;
                    }

                    cell = Grid.CellAbove(cell);
                }

                return true;
            }
        }

        public class SMInstance : GameStateMachine<States, SMInstance, MiteorSpawner, object>.GameInstance
        {
            [Serialize]
            public float elapsedTime;

            public int myWorldId;

            public SMInstance(MiteorSpawner master) : base(master)
            {
                myWorldId = master.GetMyWorldId();
            }
        }
    }
}
