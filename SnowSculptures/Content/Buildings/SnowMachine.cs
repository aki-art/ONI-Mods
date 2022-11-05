using KSerialization;
using TUNING;
using UnityEngine;

namespace SnowSculptures.Content.Buildings
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class SnowMachine : StateMachineComponent<SnowMachine.SMInstance>
    {
        [Serialize]
        [SerializeField]
        public float speed;

        [Serialize]
        [SerializeField]
        public float density;

        [Serialize]
        [SerializeField]
        public float turbulence;

        [Serialize]
        [SerializeField]
        public float lifeTime;

        [Serialize]
        public bool active;

        private ParticleSystem particles;
        private Transform colliderPlane;
        private const float Y_OFFSET = 0.73f;

        private EffectorValues decor;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Subscribe((int)GameHashes.CopySettings, OnCopySettings);
        }

        protected override void OnSpawn()
        {
            decor = new EffectorValues(Mod.Settings.SnowMachineDecor.Amount, Mod.Settings.SnowMachineDecor.Range);

            particles = Instantiate(ModAssets.Prefabs.snowParticlesPrefab).GetComponent<ParticleSystem>();
            particles.gameObject.SetActive(true);

            var pos = transform.position + new Vector3(0, Y_OFFSET, Grid.GetLayerZ(Grid.SceneLayer.TileFront) - 0.5f);
            particles.transform.position = pos;
            particles.transform.SetParent(transform);

            colliderPlane = particles.transform.Find("Plane");

            var main = particles.main;
            main.maxParticles = Mod.Settings.SnowMachineMaxParticles;

            UpdateValues();

            smi.StartSM();
        }

        private void OnCopySettings(object obj)
        {
            if (((GameObject)obj).TryGetComponent(out SnowMachine snowMachine))
            {
                speed = snowMachine.speed;
                turbulence = snowMachine.turbulence;
                lifeTime = snowMachine.lifeTime;
                density = snowMachine.density;

                UpdateValues();
            }
        }

        public void UpdateValues()
        {
            var main = particles.main;
            main.startLifetime = lifeTime;

            var emission = particles.emission;
            emission.rateOverTime = density;

            var noise = particles.noise;
            noise.strength = turbulence;

            main.simulationSpeed = speed;
        }

        public class States : GameStateMachine<States, SMInstance, SnowMachine>
        {
            public State off;
            public State on;

            public override void InitializeStates(out BaseState default_state)
            {
                default_state = off;

                off
                    .Enter(StopSnowing)
                    .EventTransition(GameHashes.OperationalChanged, on, smi => smi.operational.IsOperational);

                on
                    .Enter(StartSnowing)
                    .ToggleTag(GameTags.Decoration)
                    .Update(RefreshCollider, UpdateRate.SIM_1000ms)
                    .EventTransition(GameHashes.OperationalChanged, off, smi => !smi.operational.IsOperational);
            }

            private void StartSnowing(SMInstance smi)
            {
                smi.decorProvider.SetValues(smi.master.decor);
                smi.decorProvider.Refresh();

                smi.master.particles.gameObject.SetActive(true);
                smi.master.particles.Play();
            }

            private void StopSnowing(SMInstance smi)
            {
                smi.decorProvider.SetValues(DECOR.NONE);
                smi.decorProvider.Refresh();

                smi.master.particles.Stop();
                smi.master.particles.gameObject.SetActive(false);
            }

            private void RefreshCollider(SMInstance smi, float _)
            {
                var y = -FindSurfaceDistance(smi.transform.position);
                smi.master.colliderPlane.localPosition = new Vector3(0, y, 0);
            }

            private bool CanPassCell(int cell)
            {
                return Grid.IsGas(cell) || Mathf.Approximately(Grid.Mass[cell], 0);
            }

            private float FindSurfaceDistance(Vector3 position)
            {
                var cell = Grid.PosToCell(position);
                if (!CanPassCell(cell))
                {
                    return 0;
                }

                Grid.PosToXY(position, out var x, out var y);
                var yo = 0;

                while (CanPassCell(cell))
                {
                    yo++;

                    if (yo > 16)
                    {
                        return 16;
                    }

                    cell = Grid.XYToCell(x, y - yo);
                }

                var offset = (yo + Y_OFFSET) - 1f;

                if (Grid.IsLiquid(cell))
                {
                    var partialHeight = Grid.Mass[cell] / Grid.Element[cell].maxMass;
                    partialHeight = Mathf.Clamp01(partialHeight);

                    return offset + partialHeight;
                }
                else
                {
                    return offset;
                }
            }
        }

        public class SMInstance : GameStateMachine<States, SMInstance, SnowMachine, object>.GameInstance
        {
            public DecorProvider decorProvider;
            public Operational operational;

            public SMInstance(SnowMachine master) : base(master)
            {
                decorProvider = master.GetComponent<DecorProvider>();
                operational = master.GetComponent<Operational>();

            }
        }
    }
}
