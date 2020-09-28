/*using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Dowsing
{
    class DowsingSideScreen : SideScreenContent, IRender1000ms
    {
        private DowsingRod target;

        [SerializeField]
        private GameObject entityToggle;
        [SerializeField]
        private GameObject requestObjectList;
        private List<ReceptacleToggle> entityToggles = new List<ReceptacleToggle>();
        ReceptacleToggle selectedEntityToggle;
        Material desaturatedMaterial;
        Material defaultMaterial;

        public override bool IsValidForTarget(GameObject target) => target.GetComponent<DowsingRod>() != null;
        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

            entityToggle = transform.Find("Contents/ScrollViewport/GridLayout/Toggle").gameObject;
            requestObjectList = transform.Find("Contents/ScrollViewport/GridLayout").gameObject;
            defaultMaterial = Assets.GetMaterial("animMatUI");
            desaturatedMaterial = Assets.GetMaterial("animMatUIDesat");
        }

        private DowsingToggle ConvertToDowsingToggle(GameObject original)
        {
            ReceptacleToggle origComponent = original.GetComponent<ReceptacleToggle>();
            DowsingToggle destComponent = original.AddComponent<DowsingToggle>();
            System.Type type = typeof(ReceptacleToggle);

            foreach (var field in type.GetFields())
            {
                if (!field.IsStatic)
                    field.SetValue(destComponent, field.GetValue(origComponent));
            }

            Destroy(origComponent);
            return destComponent;
        }

        public override void SetTarget(GameObject target)
        {
            DowsingRod component = target.GetComponent<DowsingRod>();
            Initialize(component);

            entityToggles.ForEach(r => Destroy(r.gameObject));
            entityToggles.Clear();

            foreach (GameObject geyser in Assets.GetPrefabsWithComponent<Geyser>())
            {
                GameObject toggle = Util.KInstantiateUI(entityToggle, requestObjectList);
                toggle.SetActive(true);

                DowsingToggle newToggle = ConvertToDowsingToggle(toggle);

                newToggle.prefabID = geyser.PrefabID();
                newToggle.title.text = geyser.GetProperName();
                newToggle.image.sprite = Def.GetUISprite(Assets.GetPrefab(geyser.PrefabID()), "ui", false).first;
                newToggle.toggle.onClick += () => ToggleClicked(newToggle);
                newToggle.toggle.onPointerEnter += CheckAndUpdate;

                entityToggles.Add(newToggle);
            }

            selectedEntityToggle = null;
        }

        private void CheckAndUpdate()
        {

        }

        private void ToggleClicked(DowsingToggle newToggle)
        {
            Debug.Log("Geyser selected" + newToggle.prefabID);
        }

        private void Initialize(DowsingRod target)
        {
            this.target = target;
            gameObject.SetActive(true);
        }
        private void SetImageToggleState(KToggle toggle, ImageToggleState.State state)
        {
            switch (state)
            {
                case ImageToggleState.State.Disabled:
                    toggle.GetComponent<ImageToggleState>().SetDisabled();
                    toggle.gameObject.GetComponentInChildrenOnly<Image>().material = desaturatedMaterial;
                    return;
                case ImageToggleState.State.Inactive:
                    toggle.GetComponent<ImageToggleState>().SetInactive();
                    toggle.gameObject.GetComponentInChildrenOnly<Image>().material = defaultMaterial;
                    return;
                case ImageToggleState.State.Active:
                    toggle.GetComponent<ImageToggleState>().SetActive();
                    toggle.gameObject.GetComponentInChildrenOnly<Image>().material = defaultMaterial;
                    return;
                case ImageToggleState.State.DisabledActive:
                    toggle.GetComponent<ImageToggleState>().SetDisabledActive();
                    toggle.gameObject.GetComponentInChildrenOnly<Image>().material = desaturatedMaterial;
                    return;
                default:
                    return;
            }
        }

        public void Render1000ms(float dt)
        {
            //throw new System.NotImplementedException();
        }

        class DowsingToggle : ReceptacleToggle//, IPointerEnterHandler
        {
            public Tag prefabID;

            public void OnPointerEnter(PointerEventData eventData)
            {
                Debug.Log("it works?");
            }
        }
    }
}
*/