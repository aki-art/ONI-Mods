using UnityEngine;
using WorldCreep.WorldEvents;

namespace WorldCreep.Buildings
{
    public class SeismicStabilizer : KMonoBehaviour
    {
        [SerializeField]
        public int radius;

        protected override void OnSpawn() => SeismicGrid.AddNullifier(transform.position, radius);

        protected override void OnCleanUp() => SeismicGrid.RemoveNullifier(transform.position, radius);
    }
}
