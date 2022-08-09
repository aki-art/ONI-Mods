using FUtility.FUI;
using HarmonyLib;
using System.Collections;
using UnityEngine;

namespace CompactMenus.Cmps
{
    public class BuildMenuSearch : KScreen, IInputHandler
    {
        [MyCmpReq]
        private KInputTextField inputField;

        [SerializeField]
        public FButton resetSearchButton;

        // field accessors
        private static AccessTools.FieldRef<PlanScreen, KIconToggleMenu.ToggleInfo> ref_activeCategoryInfo;

        // method delegates
        private delegate void BuildButtonListDelegate(HashedString planCategory, GameObject parent);
        private delegate void ConfigurePanelSizeDelegate(object data);

        private static System.Action clearButtons;
        private static BuildButtonListDelegate buildButtonList;
        private static ConfigurePanelSizeDelegate configurePanelSize;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            ref_activeCategoryInfo = AccessTools.FieldRefAccess<PlanScreen, KIconToggleMenu.ToggleInfo>("activeCategoryInfo");
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            inputField.onValueChanged.AddListener(SearchFilter);
            inputField.onFocus += OnEditStart;
            inputField.onEndEdit.AddListener(OnEditEnd);

            Activate();

            resetSearchButton.OnClick += OnReset;
        }

        private static void SetMethodDelegates()
        {
            var m_ClearButtons = AccessTools.Method(typeof(PlanScreen), "ClearButtons");
            var m_BuildButtonList = AccessTools.Method(typeof(PlanScreen), "BuildButtonList");
            var m_ConfigurePanelSize = AccessTools.Method(typeof(PlanScreen), "ConfigurePanelSize");

            clearButtons = AccessTools.MethodDelegate<System.Action>(m_ClearButtons, PlanScreen.Instance);
            buildButtonList = AccessTools.MethodDelegate<BuildButtonListDelegate>(m_BuildButtonList, PlanScreen.Instance);
            configurePanelSize = AccessTools.MethodDelegate<ConfigurePanelSizeDelegate>(m_ConfigurePanelSize, PlanScreen.Instance);
        }

        private void OnReset()
        {
            inputField.text = "";
        }

        protected override void OnShow(bool show)
        {
            base.OnShow(show);

            if (show)
            {
                Activate();
                inputField.ActivateInputField();
                SearchFilter(null);
            }
            else
            {
                Deactivate();
                Mod.buildMenuSearch = "";
            }
        }

        protected override void OnDeactivate()
        {
            Mod.buildMenuSearch = "";
        }

        private IEnumerator DelayedRestore()
        {
            yield return new WaitForEndOfFrame();
            inputField.text = Mod.buildMenuSearch;
            SearchFilter(Mod.buildMenuSearch);

            yield break;
        }

        private void OnEditEnd(string input)
        {
            if (gameObject.activeInHierarchy)
            {
                SearchFilter(input);
                return;
            }

            isEditing = false;
            inputField.DeactivateInputField();
        }

        private void OnEditStart()
        {
            isEditing = true;
            inputField.Select();
            inputField.ActivateInputField();

            KScreenManager.Instance.RefreshStack();
        }

        public override void OnKeyDown(KButtonEvent e)
        {
            if (!isEditing)
            {
                base.OnKeyDown(e);
                return;
            }

            if (e.TryConsume(Action.Escape))
            {
                inputField.DeactivateInputField();
                e.Consumed = true;
                isEditing = false;
            }

            if (isEditing)
            {
                e.Consumed = true;
                return;
            }

            if (!e.Consumed)
            {
                base.OnKeyDown(e);
            }
        }

        private void SearchFilter(string searchText)
        {
            Mod.buildMenuSearch = searchText?.ToLowerInvariant();

            if (clearButtons == null)
            {
                SetMethodDelegates();
            }

            var category = (HashedString)ref_activeCategoryInfo(PlanScreen.Instance).userData;

            clearButtons.Invoke();
            buildButtonList.Invoke(category, PlanScreen.Instance.GroupsTransform.gameObject);
            configurePanelSize.Invoke(null);
        }
    }
}
