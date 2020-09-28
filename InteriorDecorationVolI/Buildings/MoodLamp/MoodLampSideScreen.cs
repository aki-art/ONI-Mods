using System.Collections.Generic;
using UnityEngine;

namespace InteriorDecorationv1.Buildings.MoodLamp
{
	class MoodLampSideScreen : SideScreenContent
	{
		[SerializeField]
		private RectTransform buttonContainer;
		public GameObject stateButtonPrefab;
		private List<GameObject> buttons = new List<GameObject>();
		private MoodLamp target;
		public override bool IsValidForTarget(GameObject target) => target.GetComponent<MoodLamp>() != null;

		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			titleKey = "STRINGS.UI.UISIDESCREENS.MOODLAMP_SIDE_SCREEN.TITLE";
			stateButtonPrefab = transform.Find("ButtonPrefab").gameObject;
			buttonContainer = transform.Find("Content/Scroll/Grid").GetComponent<RectTransform>();
		}
		public override void SetTarget(GameObject target)
		{
			base.SetTarget(target);
			this.target = target.GetComponent<MoodLamp>();
			gameObject.SetActive(true);
			GenerateStateButtons();
		}

		private void GenerateStateButtons()
		{
			ClearButtons();
			KAnimFile animFile = target.GetComponent<KBatchedAnimController>().AnimFiles[0];

			foreach (var variant in target.variants)
				AddNewButton(animFile, variant);
		}

		private void AddNewButton(KAnimFile animFile, KeyValuePair<string, Color> variant)
		{
			var gameObject = Util.KInstantiateUI(stateButtonPrefab, buttonContainer.gameObject, true);
			var button = gameObject.GetComponent<KButton>();
			button.onClick += delegate { target.SetVariant(variant.Key); };
			buttons.Add(gameObject);
			button.fgImage.sprite = Def.GetUISpriteFromMultiObjectAnim(animFile, variant.Key + "_ui");
		}

		private void ClearButtons()
		{
			foreach (var button in buttons)
				Util.KDestroyGameObject(button);
			buttons.Clear();
		}
	}
}
