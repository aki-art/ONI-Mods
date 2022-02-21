using FUtility;
using FUtility.FUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SpookyPumpkin.STRINGS.UI.UISIDESCREENS.GHOSTSIDESCREEN;

namespace SpookyPumpkin.GhostPip
{
    internal class GhostSquirrelSideScreen : SideScreenContent
    {
        private SeedTrader seedTrader;
        private Tag treatTag;

        private GameObject selectedItemPanel;
        private TextMeshProUGUI description;
        private TextMeshProUGUI selectedItemLabel;
        private TextMeshProUGUI seedLabel;
        private TextMeshProUGUI treatButtonLabel;
        private Image selectedItemImage;
        private Image seedImage;
        private FButton treatButton;

        public override bool IsValidForTarget(GameObject target)
        {
            return target.GetComponent<SeedTrader>() != null;
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            titleKey = "SpookyPumpkinSO.STRINGS.UI.UISIDESCREENS.GHOSTSIDESCREEN.TITLE";

            GameObject buttonsScroll = transform.Find("Contents/Scroll View").gameObject;
            buttonsScroll.SetActive(false);

            treatButton = transform.Find("Contents/Buttons/TreatButton").gameObject.AddComponent<FButton>();
            treatButtonLabel = treatButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();

            selectedItemPanel = transform.Find("Contents/SelectedItem").gameObject;
            selectedItemPanel.SetActive(true);

            selectedItemImage = selectedItemPanel.transform.Find("Panel/Image").GetComponent<Image>();
            selectedItemLabel = selectedItemPanel.transform.Find("Panel/Label").GetComponent<TextMeshProUGUI>();

            seedImage = selectedItemPanel.transform.Find("SeedPanel/Image").GetComponent<Image>();
            seedLabel = selectedItemPanel.transform.Find("SeedPanel/Label").GetComponent<TextMeshProUGUI>();
            description = selectedItemPanel.transform.Find("Label").GetComponent<TextMeshProUGUI>();
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            //var treat = treatButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            treatButtonLabel.alignment = TextAlignmentOptions.Center;

            selectedItemLabel.color = Color.black;
            selectedItemLabel.alignment = TextAlignmentOptions.Center;
            selectedItemLabel.fontSize = 14;

            seedImage.sprite = Def.GetUISprite(Assets.GetPrefab(PumpkinPlantConfig.SEED_ID)).first;
            seedLabel.color = Color.black;
            seedLabel.alignment = TextAlignmentOptions.Center;
            seedLabel.fontSize = 14;
            seedLabel.SetText(STRINGS.CREATURES.SPECIES.SEEDS.SP_PUMPKIN.NAME);

            treatButton.OnClick += OnButtonClick;

            gameObject.SetActive(true);
        }

        public override void SetTarget(GameObject target)
        {
            seedTrader = target.GetComponent<SeedTrader>();
            treatTag = seedTrader.treatTag;
            RefreshUI();
        }

        private void OnButtonClick()
        {
            if (seedTrader != null && seedTrader.IsConsumed)
            {
                seedTrader.RequestTreat(!seedTrader.TreatRequested);
                RefreshUI();
            }
        }

        private void RefreshUI()
        {
            string btnText = seedTrader.TreatRequested ? CANCELBUTTON : TREATBUTTON;

            Log.Debuglog("treatButtonText");
            Log.Debuglog(treatButtonLabel == null);

            treatButtonLabel.SetText(btnText);
            GameObject treat = Assets.TryGetPrefab(treatTag);

            if (treat != null)
            {
                selectedItemImage.sprite = Def.GetUISprite(treat).first;
                string str = seedTrader.TreatRequested ? LABEL2 : LABEL;
                str += " ";
                str += treat.GetProperName();
                selectedItemLabel.SetText(str);
            }
        }
    }
}