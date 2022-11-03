using FUtility;
using KSerialization;
using UnityEngine;
using static STRINGS.UI.TOOLS;

namespace SnowSculptures.Content.Buildings
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class SnowMachine : KMonoBehaviour, ISim4000ms
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

        private ParticleSystem particles;
        private Transform colliderPlane;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Subscribe((int)GameHashes.CopySettings, OnCopySettings);
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            particles = Instantiate(ModAssets.Prefabs.snowParticlesPrefab).GetComponent<ParticleSystem>();
            particles.gameObject.SetActive(true);
            var pos = transform.position + new Vector3(0, 0, Grid.GetLayerZ(Grid.SceneLayer.TileFront) - 0.5f);
            particles.transform.position = pos;
            particles.transform.SetParent(transform);

            var main = particles.main;
            main.maxParticles = Mod.Settings.SnowMachineMaxParticles;

            UpdateValues();

            particles.Play();

            colliderPlane = particles.transform.Find("Plane");

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

        public void Sim4000ms(float dt)
        {
            RefreshKillPlane();
        }

        public void RefreshKillPlane()
        {
            var y = -FindSurfaceDistance(transform.position);
            colliderPlane.localPosition = new Vector3(0, y, 0);
        }

        private float FindSurfaceDistance(Vector3 position)
        {
            var cell = Grid.PosToCell(position);
            if (!Grid.IsGas(cell))
            {
                return 0;
            }

            Grid.PosToXY(position, out int x, out int y);
            var yo = 0;

            while (Grid.IsGas(cell))
            {
                yo++;

                if (yo > 16)
                {
                    return 16;
                }

                cell = Grid.XYToCell(x, y - yo);
            }

            yo -= 1; // the top of the tile is needed, so offset by one

            if(Grid.IsLiquid(cell))
            {
                var partialHeight = Grid.Mass[cell] / Grid.Element[cell].maxMass;
                partialHeight = Mathf.Clamp01(partialHeight);

                return yo + partialHeight;
            }
            else
            {
                return yo;
            }
        }
    }
}
