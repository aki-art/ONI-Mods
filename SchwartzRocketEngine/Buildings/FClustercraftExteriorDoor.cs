using UnityEngine;

namespace SchwartzRocketEngine.Buildings
{
    public class FClustercraftInteriorDoor : ClustercraftInteriorDoor
    {
        KBatchedAnimController kbac;

        [SerializeField]
        public string bgAnim;

        [SerializeField]
        public Vector3 bgOffset;

        protected override void OnSpawn()
        {
            base.OnSpawn();
            kbac = FXHelpers.CreateEffect(bgAnim, transform.position + bgOffset, layer: Grid.SceneLayer.BuildingBack);
            kbac.gameObject.SetActive(true);
            kbac.transform.SetParent(transform);
            //kbac.gameObject.transform.position += new Vector3(0, 0, -0.5f);
            kbac.Play("bg", KAnim.PlayMode.Paused);

            //new KAnimLink(GetComponent<KBatchedAnimController>(), kbac);
        }

        protected override void OnCleanUp()
        {
            Destroy(kbac);
            base.OnCleanUp();
        }
    }
}
