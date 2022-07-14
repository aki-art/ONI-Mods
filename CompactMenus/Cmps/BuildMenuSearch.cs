using FUtility;
using FUtility.FUI;
using HarmonyLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompactMenus.Cmps
{
    public class BuildMenuSearch : KScreen, IInputHandler
    {
        [MyCmpReq]
        private KInputTextField inputField;

        [SerializeField]
		public FButton resetSearchButton;

		private static Traverse t_PlanScreen;
		private static Traverse t_BuildButtonList;
		private static Traverse t_ClearButtons;
		private static Traverse t_ConfigurePanelSize;
		private static Traverse<KIconToggleMenu.ToggleInfo> t_activeCategoryInfo;

		protected override void OnSpawn()
		{
			base.OnSpawn();

			inputField.onValueChanged.AddListener(SearchFilter);
			inputField.onFocus += OnEditStart;
			inputField.onEndEdit.AddListener(OnEditEnd);

			Activate();

			resetSearchButton.OnClick += OnReset;
		}

        private void OnReset()
		{
			//SearchFilter("");
			inputField.text = "";
		}

        protected override void OnShow(bool show)
        {
            base.OnShow(show);

			if(show)
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

        protected override void OnActivate()
		{
			StartCoroutine(DelayedRestore());
		}

        protected override void OnDeactivate()
		{
			Mod.buildMenuSearch = "";
		}

        private IEnumerator DelayedRestore()
		{
			yield return new WaitForEndOfFrame();
			Mod.buildMenuSearch = inputField.text;

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
			if(!isEditing)
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

			if(!e.Consumed)
			{
				base.OnKeyDown(e);
			}
		}

		private void SearchFilter(string arg0)
		{
			Mod.buildMenuSearch = arg0.ToLowerInvariant();

			if(t_PlanScreen == null)
			{
				t_PlanScreen = Traverse.Create(PlanScreen.Instance);
				t_ClearButtons = t_PlanScreen.Method("ClearButtons");
				t_activeCategoryInfo = t_PlanScreen.Field<KIconToggleMenu.ToggleInfo>("activeCategoryInfo");
				t_BuildButtonList = t_PlanScreen.Method("BuildButtonList", new[] { typeof(HashedString), typeof(GameObject) });
				t_ConfigurePanelSize = t_PlanScreen.Method("ConfigurePanelSize", new[] { typeof(object) });
			}

			t_ClearButtons.GetValue();
			t_BuildButtonList.GetValue((HashedString)t_activeCategoryInfo.Value.userData, PlanScreen.Instance.GroupsTransform.gameObject);
			t_ConfigurePanelSize.GetValue(new object[] { null });
		}
	}
}
