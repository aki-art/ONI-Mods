using System.Collections.Generic;
using UnityEngine;

namespace DecorPackA.DPBuilding.MoodLamp
{
	class MoodLampSideScreen : SideScreenContent
	{
		[SerializeField]
		private RectTransform buttonContainer;
		private GameObject stateButtonPrefab;
		private GameObject debugVictoryButton;
		private GameObject flipButton;
		private readonly List<GameObject> buttons = new List<GameObject>();
		private MoodLamp target;

		public override bool IsValidForTarget(GameObject target) => target.GetComponent<MoodLamp>() != null;

		protected override void OnSpawn()
		{
			base.OnSpawn();
			flipButton.SetActive(false);
			debugVictoryButton.SetActive(false);
		}

		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			titleKey = "STRINGS.UI.UISIDESCREENS.MOODLAMP_SIDE_SCREEN.TITLE";
			stateButtonPrefab = transform.Find("ButtonPrefab").gameObject;
			buttonContainer = transform.Find("Content/Scroll/Grid").GetComponent<RectTransform>();
			debugVictoryButton = transform.Find("Butttons/Button").gameObject;
			flipButton = transform.Find("Butttons/FlipButton").gameObject;
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

			AddRandomizerButton(animFile);

			foreach (var variant in target.variants)
            {
                AddNewButton(animFile, variant.Key);
            }
        }

		private void AddRandomizerButton(KAnimFile animFile)
		{
			var gameObject = Util.KInstantiateUI(stateButtonPrefab, buttonContainer.gameObject, true);

			if (gameObject.TryGetComponent(out KButton button))
			{
				button.onClick += SetRandom;
				button.fgImage.sprite = Def.GetUISpriteFromMultiObjectAnim(animFile, "random_ui");
			}

			buttons.Add(gameObject);
		}

		private void AddNewButton(KAnimFile animFile, string id)
		{
			var gameObject = Util.KInstantiateUI(stateButtonPrefab, buttonContainer.gameObject, true);
			
			if(gameObject.TryGetComponent(out KButton button)) { 
				button.onClick += () => SetVariant(id);
				button.fgImage.sprite = Def.GetUISpriteFromMultiObjectAnim(animFile, id + "_ui");
			}

			buttons.Add(gameObject);
		}

        private void SetVariant(string id)
        {
			target.SetVariant(id);
		}

		private void SetRandom()
        {
			target.SetVariant(target.SelectRandom());
        }

        private void ClearButtons()
		{
			foreach (var button in buttons)
            {
                Util.KDestroyGameObject(button);
            }

            buttons.Clear();
			flipButton.SetActive(false);
		}
	}
}