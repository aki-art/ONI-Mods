using UnityEngine;

namespace DecorPackA.DPBuilding.StainedGlassTile
{
    class ColorShifter : KMonoBehaviour
    {
        [SerializeField]
        public Color a = Color.white;

        [SerializeField]
        public Color b = Color.black;

        [SerializeField]
        public float frequency = 10f;

        protected override void OnSpawn()
        {
            base.OnSpawn();

            Vector2I buildingPos = Grid.PosToXY(transform.position);
            float x = buildingPos.x / frequency;
            float y = buildingPos.y / frequency;
            float t = Mathf.Sin(x) + Mathf.Sin(y);

            Mod.colorOverlays[Grid.PosToCell(transform.position)] = Color.Lerp(a, b, (t + 2f) / 4f);
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();
            Mod.colorOverlays.Remove(Grid.PosToCell(transform.position));
        }
    }
}
