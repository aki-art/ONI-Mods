using UnityEngine;

namespace MoreMarbleSculptures.Components
{
    public class TintableOverlay : KMonoBehaviour
    {
        [SerializeField]
        public Vector3 offset;

        [SerializeField]
        public string anim;

        public KBatchedAnimController Kbac {  get; private set; }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            Create();
        }

        public void Create()
        {
            transform.parent = transform;

            gameObject.AddComponent<KPrefabID>().PrefabTag = new Tag(name);

            Kbac = gameObject.AddComponent<KBatchedAnimController>();
            Kbac.AnimFiles = new KAnimFile[]
            {
                Assets.GetAnim(anim)
            };

            Kbac.initialAnim = "slab";
            Kbac.isMovable = true;
            Kbac.sceneLayer = Grid.SceneLayer.BuildingFront;

            transform.SetPosition(transform.position + offset);
            Kbac.gameObject.SetActive(true);
        }
    }
}
