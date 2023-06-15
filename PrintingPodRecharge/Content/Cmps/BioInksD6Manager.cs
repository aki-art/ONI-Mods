using HarmonyLib;
using KSerialization;
using System.Collections.Generic;
using UnityEngine;

namespace PrintingPodRecharge.Content.Cmps
{
	[SerializationConfig(MemberSerialization.OptIn)]
    public class BioInksD6Manager : KMonoBehaviour
    {
        public static BioInksD6Manager Instance;
        public KButton button;
        public LocText label;
        public ToolTip tooltip;

        [Serialize] public int diceCount;

        private Traverse<List<ITelepadDeliverableContainer>> containersTraverse;
        private Traverse InitializeContainersMethod;

        private static AccessTools.FieldRef<CharacterSelectionController, List<ITelepadDeliverableContainer>> ref_container;

        // used for debugging
        public bool forceDie;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Instance = this;

            ref_container = AccessTools.FieldRefAccess<CharacterSelectionController, List<ITelepadDeliverableContainer>>("containers");
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();
            Instance = null;
        }

        public void SetButton(GameObject gameObject)
        {
            button = gameObject.GetComponent<KButton>();
            button.onClick += () => UseDie();
            label = gameObject.GetComponentInChildren<LocText>();

            var red = Util.ColorFromHex("8d332c");

            button.bgImage.colorStyleSetting = new ColorStyleSetting()
            {
                activeColor = red,
                inactiveColor = red,
                disabledColor = button.bgImage.colorStyleSetting.disabledColor,
                disabledActiveColor = button.bgImage.colorStyleSetting.disabledActiveColor,
                hoverColor = Util.ColorFromHex("a33e36"),
                disabledhoverColor = button.bgImage.colorStyleSetting.disabledhoverColor
            };

            button.bgImage.ApplyColorStyleSetting();
            gameObject.SetActive(false);
            UpdateDiceButton();
        }

        public bool HasDice()
        {
            return diceCount > 0;
        }

        private void UpdateDiceButton()
        {
            if (diceCount > 0)
            {
                button.gameObject.SetActive(true);
                label.SetText(string.Format(STRINGS.UI.D6BUTTON.LABEL, diceCount));
            }
            else
            {
                button.gameObject.SetActive(false);
            }
        }

        public bool UseDie()
        {
            if (diceCount <= 0)
            {
                return false;
            }

            if (ModAssets.Sounds.diceRolls != null && ModAssets.Sounds.diceRolls.Count > 0)
            {
                var sound = ModAssets.Sounds.diceRolls.GetRandom();
                AudioUtil.PlaySound(sound, KPlayerPrefs.GetFloat("Volume_UI"));
            }

            if (containersTraverse == null)
            {
                containersTraverse = Traverse.Create(ImmigrantScreen.instance).Field<List<ITelepadDeliverableContainer>>("containers");
                InitializeContainersMethod = Traverse.Create(ImmigrantScreen.instance).Method("InitializeContainers");
            }

            var containers = containersTraverse.Value;

            foreach (var telepadDeliverableContainer in ref_container(ImmigrantScreen.instance))
            {
                Destroy(telepadDeliverableContainer.GetGameObject());
            }

            containers.Clear();

            InitializeContainersMethod.GetValue();

            foreach (var container in ref_container(ImmigrantScreen.instance))
            {
                if (container is CharacterContainer characterContainer)
                {
                    characterContainer.SetReshufflingState(false);
                }
            }

            diceCount--;
            UpdateDiceButton();

            return true;
        }

        public void AddDie(int count)
        {
            diceCount += count;
            UpdateDiceButton();
        }
    }
}
