using FUtility.FUI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DecorPackA.STRINGS.BUILDINGS.PREFABS.DECORPACKA_MOODLAMP;

namespace DecorPackA.Buildings.MoodLamp
{
	internal class MoodLampSideScreen : SideScreenContent
    {
        [SerializeField]
        private RectTransform buttonContainer;

        private GameObject stateButtonPrefab;
        private GameObject debugVictoryButton;
        //private CategoryHeader categoryHeaderPrefab;
        private KButton flipButton;
		private readonly List<GameObject> buttons = new();
        //private readonly Dictionary<string, CategoryHeader> categories = new();
        private MoodLamp target;
        private bool initialized;
        private Image swatchPrefab;

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

            target.GetComponent<Rotatable>().Rotate();
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
			if (!initialized)
			{
				Helper.ListChildren(transform);

				titleKey = "STRINGS.UI.UISIDESCREENS.MOODLAMP_SIDE_SCREEN.TITLE";
				stateButtonPrefab = transform.Find("ButtonPrefab").gameObject;
				buttonContainer = transform.Find("Content/Scroll/Grid").GetComponent<RectTransform>();
				debugVictoryButton = transform.Find("Butttons/Button").gameObject;
				flipButton = transform.Find("Butttons/FlipButton").GetComponent<KButton>();

                var go = new GameObject("swatch");
                var rect = go.AddOrGet<RectTransform>();
                rect.sizeDelta = new(16, 16);
                var image = go.AddComponent<Image>();
                image.sprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, Texture2D.whiteTexture.width, Texture2D.whiteTexture.height), Vector2.zero);
                image.color = Color.white;

                var outline = go.AddComponent<Outline>();
                outline.effectDistance = new(1f, 1f);
                outline.effectColor = Color.black;

                swatchPrefab = image;
                rect.localPosition = new(-10, -56, -0.01f);

                go.SetActive(false);

				initialized = true;
			}

			ClearButtons();
			var animFile = target.GetComponent<KBatchedAnimController>().AnimFiles[0];

			AddRandomizerButton("");

			foreach (var lamp in ModDb.lampVariants.resources)
			{
				if (!lamp.hidden)
					AddButton(lamp, lamp.Name, () => target.SetVariant(lamp.Id));
			}
		}

		private void AddRandomizerButton(string animName)
		{
			var gameObject = Util.KInstantiateUI(stateButtonPrefab, buttonContainer.gameObject, true);

			if (gameObject.TryGetComponent(out KButton button))
			{
				button.onClick += () => target.SetRandom();

                //if (Assets.TryGetAnim(variant.kAnimFile, out var anim))
                button.fgImage.sprite = Assets.GetSprite("unknown"); // Def.GetUISpriteFromMultiObjectAnim(anim);
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

                if(Assets.TryGetAnim(variant.kAnimFile, out var anim))
					button.fgImage.sprite = Def.GetUISpriteFromMultiObjectAnim(anim);
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
