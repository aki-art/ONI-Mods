using FUtility;
using FUtility.FUI;
using SpookyPumpkinSO.Content.Plants;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SpookyPumpkinSO.STRINGS.UI.UISIDESCREENS.GHOSTSIDESCREEN;

namespace SpookyPumpkinSO.Content.GhostPip
{
	internal class SpookyPumpkin_GhostSquirrelSideScreen : SideScreenContent
	{
		private SeedTrader seedTrader;
		private Tag treatTag;

		private GameObject selectedItemPanel;
		private LocText selectedItemLabel;
		private LocText seedLabel;
		private LocText treatButtonLabel;
		private Image selectedItemImage;
		private Image seedImage;
		private FButton treatButton;

		public override bool IsValidForTarget(GameObject target)
		{
			return target.GetComponent<SeedTrader>() != null;
		}

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			titleKey = "STRINGS.UI.UISIDESCREENS.GHOSTSIDESCREEN.TITLE";

			var buttonsScroll = transform.Find("Contents/Scroll View").gameObject;
			buttonsScroll.SetActive(false);

			treatButton = transform.Find("Contents/Buttons/TreatButton").gameObject.AddComponent<FButton>();
			treatButtonLabel = treatButton.transform.Find("Text").GetComponent<LocText>();

			selectedItemPanel = transform.Find("Contents/SelectedItem").gameObject;
			selectedItemPanel.SetActive(true);

			selectedItemImage = selectedItemPanel.transform.Find("Panel/Image").GetComponent<Image>();
			selectedItemLabel = selectedItemPanel.transform.Find("Panel/Label").GetComponent<LocText>();

			seedImage = selectedItemPanel.transform.Find("SeedPanel/Image").GetComponent<Image>();
			seedLabel = selectedItemPanel.transform.Find("SeedPanel/Label").GetComponent<LocText>();
			//description = selectedItemPanel.transform.Find("Label").GetComponent<LocText>();
		}

		public override void OnSpawn()
		{
			base.OnSpawn();

			treatButtonLabel.alignment = TextAlignmentOptions.Center;

			selectedItemLabel.color = Color.black;
			selectedItemLabel.alignment = TextAlignmentOptions.Center;
			selectedItemLabel.fontSize = 14;
			selectedItemLabel.SetAllDirty();
			selectedItemLabel.KForceUpdateDirty();

			seedImage.sprite = Def.GetUISprite(Assets.GetPrefab(PumpkinPlantConfig.SEED_ID)).first;
			seedLabel.color = Color.black;
			seedLabel.alignment = TextAlignmentOptions.Center;
			seedLabel.fontSize = 14;
			seedLabel.SetText(STRINGS.CREATURES.SPECIES.SEEDS.SP_PUMPKIN.NAME);
			seedLabel.SetAllDirty();
			seedLabel.KForceUpdateDirty();

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

			treatButtonLabel.SetText(btnText);
			var treat = Assets.TryGetPrefab(treatTag);

			if (treat != null)
			{
				selectedItemImage.sprite = Def.GetUISprite(treat).first;
				var str = seedTrader.TreatRequested ? LABEL2 : LABEL;
				selectedItemLabel.SetText(FormatItem(str, treat.GetProperName()));
			}
		}

		// check if it is the new formatting, to support legacy translations
		private static string FormatItem(string str, string item)
		{
			if (str.Contains("{Item}"))
				return str.Replace("{Item}", item);

			else return $"{str} {item}";
		}
	}
}
