using DecorPackA.Buildings.MoodLamp;
using FUtility.FUI;
using System.Collections.Generic;
using UnityEngine;
using static DecorPackA.STRINGS.BUILDINGS.PREFABS.DECORPACKA_MOODLAMP;

namespace DecorPackA.UI
{
	public class DecorPackA_MoodLampSideScreen : SideScreenContent
	{
		[SerializeField] private RectTransform buttonContainer;

		public override int GetSideScreenSortOrder() => 20;

		private GameObject stateButtonPrefab;
		private GameObject debugVictoryButton;
		private KButton flipButton;
		private readonly List<GameObject> buttons = new();
		private MoodLamp target;
		private bool initialized;

		public override bool IsValidForTarget(GameObject target) => target.GetComponent<MoodLamp>() != null;

		public override void OnSpawn()
		{
			base.OnSpawn();

			flipButton.gameObject.SetActive(true);
			debugVictoryButton.SetActive(false);

			flipButton.onClick += Flip;
		}

		private void Flip()
		{
			if (target == null)
				return;

			target.Rotate();
		}

		public override void SetTarget(GameObject target)
		{
			base.SetTarget(target);

			if (target == null)
				return;

			this.target = target.GetComponent<MoodLamp>();
			gameObject.SetActive(true);
			GenerateStateButtons();
		}

		private void GenerateStateButtons()
		{
			if (!initialized)
			{
				titleKey = "STRINGS.UI.UISIDESCREENS.MOODLAMP_SIDE_SCREEN.TITLE";
				stateButtonPrefab = transform.Find("ButtonPrefab").gameObject;
				buttonContainer = transform.Find("Content/Scroll/Grid").GetComponent<RectTransform>();
				debugVictoryButton = transform.Find("Butttons/Button").gameObject;
				flipButton = transform.Find("Butttons/FlipButton").GetComponent<KButton>();

				initialized = true;
			}

			ClearButtons();
			var animFile = target.GetComponent<KBatchedAnimController>().AnimFiles[0];

			AddRandomizerButton("");

			foreach (var lamp in ModDb.lampVariants.resources)
			{
				if (!lamp.hidden)
					AddButton(lamp, lamp.uiName ?? lamp.Name, () => target.SetVariant(lamp.Id));
			}
		}

		private void AddRandomizerButton(string animName)
		{
			var gameObject = Util.KInstantiateUI(stateButtonPrefab, buttonContainer.gameObject, true);

			if (gameObject.TryGetComponent(out KButton button))
			{
				button.onClick += () => target.SetRandom();

				button.fgImage.sprite = Assets.GetSprite("unknown");
			}

			Helper.AddSimpleToolTip(gameObject, VARIANT.RANDOM, true);

			buttons.Add(gameObject);
		}

		private void AddButton(LampVariant variant, LocString tooltip, System.Action onClick)
		{
			var gameObject = Util.KInstantiateUI(stateButtonPrefab, buttonContainer.gameObject, true);

			if (gameObject.TryGetComponent(out KButton button))
			{
				button.onClick += onClick;

				if (Assets.TryGetAnim(variant.kAnimFile, out var anim))
					button.fgImage.sprite = Def.GetUISpriteFromMultiObjectAnim(anim);

				if (variant.showCustomizableIcon)
					Util.KInstantiateUI(ModAssets.Prefabs.brushIcon, gameObject, true);
			}

			Helper.AddSimpleToolTip(gameObject, tooltip, true);

			buttons.Add(gameObject);
		}

		private void ClearButtons()
		{
			foreach (var button in buttons)
				Util.KDestroyGameObject(button);

			buttons.Clear();
			debugVictoryButton.SetActive(false);
		}
	}
}
