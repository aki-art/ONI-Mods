using FUtility;
using FUtility.FUI;
using System.Collections.Generic;
using UnityEngine;

namespace DecorPackA.Buildings.MoodLamp
{
    internal class MoodLampSideScreen : SideScreenContent
    {
        [SerializeField]
        private RectTransform buttonContainer;

        private GameObject stateButtonPrefab;
        private GameObject debugVictoryButton;
        private GameObject flipButton;
        private readonly List<GameObject> buttons = new List<GameObject>();
        private MoodLamp target;
        private bool initialized;

        public override bool IsValidForTarget(GameObject target)
        {
            return target.GetComponent<MoodLamp>() != null;
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            // the monument screen used here has 2 extra buttons that are not needed, disabling them
            flipButton.SetActive(false);
            debugVictoryButton.SetActive(false);
        }

        public override void SetTarget(GameObject target)
        {
            base.SetTarget(target);
            this.target = target.GetComponent<MoodLamp>();
            gameObject.SetActive(true);
            GenerateStateButtons();
        }

        // Creates clickable card buttons for all the lamp types + a randomizer button
        private void GenerateStateButtons()
        {
            if(!initialized)
            {
                Helper.ListChildren(transform);

                titleKey = "STRINGS.UI.UISIDESCREENS.MOODLAMP_SIDE_SCREEN.TITLE";
                stateButtonPrefab = transform.Find("ButtonPrefab").gameObject;
                buttonContainer = transform.Find("Content/Scroll/Grid").GetComponent<RectTransform>();
                debugVictoryButton = transform.Find("Butttons/Button").gameObject;
                flipButton = transform.Find("Butttons/FlipButton").gameObject;
            }

            ClearButtons();
            var animFile = target.GetComponent<KBatchedAnimController>().AnimFiles[0];

            // random button
            AddButton(animFile, "random_ui", STRINGS.BUILDINGS.PREFABS.DECORPACKA_MOODLAMP.VARIANT.RANDOM, () => target.SetRandom());

            foreach(var lamp in ModDb.lampVariants.resources)
            {
                Log.Debuglog("added button: " + lamp.Id);
                AddButton(animFile, lamp.Id + "_ui", lamp.Name, () => target.SetVariant(lamp.Id));
            }
        }

        private void AddButton(KAnimFile animFile, string animName, LocString tooltip, System.Action onClick)
        {
            var gameObject = Util.KInstantiateUI(stateButtonPrefab, buttonContainer.gameObject, true);

            if (gameObject.TryGetComponent(out KButton button))
            {
                button.onClick += onClick;
                button.fgImage.sprite = Def.GetUISpriteFromMultiObjectAnim(animFile, animName);
            }

            FUtility.FUI.Helper.AddSimpleToolTip(gameObject, tooltip, true);
            buttons.Add(gameObject);
        }

        private void ClearButtons()
        {
            Log.Assert("buttons", buttons);

            foreach (var button in buttons)
            {
                Util.KDestroyGameObject(button);
            }

            buttons.Clear();

            Log.Assert("flipButton", flipButton);
            Log.Assert("debugVictoryButton", debugVictoryButton);

            flipButton.SetActive(false);
            debugVictoryButton.SetActive(false);
        }
    }
}
