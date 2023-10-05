using FUtility.FUI;
using System.Collections.Generic;
using UnityEngine;

namespace SpookyPumpkinSO.Content.Buildings
{
	public class SpookyPumpkin_CarvedPumpkinSideScreen : SideScreenContent
	{
		[SerializeField] private RectTransform buttonContainer;

		public override int GetSideScreenSortOrder() => 20;

		private GameObject stateButtonPrefab;
		private GameObject debugVictoryButton;
		private KButton flipButton;
		private readonly List<GameObject> buttons = new();
		private CarvedPumpkin target;
		private bool initialized;

		public override bool IsValidForTarget(GameObject target) => target.GetComponent<CarvedPumpkin>() != null;

		public override void OnSpawn()
		{
			base.OnSpawn();

			flipButton.gameObject.SetActive(true);
			debugVictoryButton.SetActive(false);

			flipButton.onClick += Flip;
		}

		private void Flip()
		{
			if (target != null && target.TryGetComponent(out Rotatable rotatable))
				rotatable.Rotate();
		}

		public override void SetTarget(GameObject target)
		{
			base.SetTarget(target);
			this.target = target.GetComponent<CarvedPumpkin>();
			gameObject.SetActive(true);
			GenerateStateButtons();
		}

		private void GenerateStateButtons()
		{
			if (!initialized)
			{
				titleKey = "STRINGS.UI.UISIDESCREENS.FRIENDLYPUMPKIN_SIDE_SCREEN.TITLE";
				stateButtonPrefab = transform.Find("ButtonPrefab").gameObject;
				buttonContainer = transform.Find("Content/Scroll/Grid").GetComponent<RectTransform>();
				debugVictoryButton = transform.Find("Butttons/Button").gameObject;
				flipButton = transform.Find("Butttons/FlipButton").GetComponent<KButton>();

				initialized = true;
			}

			ClearButtons();

			foreach (var face in ModDb.pumpkinFaces.resources)
				AddButton(face, face.Name, () => target.Carve(face.Id));
		}

		private void AddButton(FriendlyPumpkinFace variant, LocString tooltip, System.Action onClick)
		{
			var gameObject = Util.KInstantiateUI(stateButtonPrefab, buttonContainer.gameObject, true);

			if (gameObject.TryGetComponent(out KButton button))
			{
				button.onClick += onClick;
				button.fgImage.sprite = Def.GetUISpriteFromMultiObjectAnim(variant.animFile);
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
