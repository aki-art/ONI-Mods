using FUtility;
using FUtility.FUI;
using KSerialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SpookyPumpkin.STRINGS.UI.UISIDESCREENS.GHOSTSIDESCREEN;

namespace SpookyPumpkin
{
	class GhostSquirrelSideScreen : SideScreenContent
	{
		private SeedTrader seedTrader;
		private GhostSquirrel ghostSquirrel;
		private Tag treatTag = GrilledPrickleFruitConfig.ID;

		private GameObject selectedItemPanel;
		private TextMeshProUGUI description;
		private TextMeshProUGUI selectedItemLabel;
		private TextMeshProUGUI seedLabel;
		private Image selectedItemImage;
		private Image seedImage;
		private FButton treatButton;

		public override bool IsValidForTarget(GameObject target) => target.GetComponent<SeedTrader>() != null;

		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			titleKey = "SpookyPumpkin.STRINGS.UI.UISIDESCREENS.GHOSTSIDESCREEN.TITLE";

			GameObject buttonsScroll = transform.Find("Contents/Scroll View").gameObject;
			buttonsScroll.SetActive(false);

			treatButton = transform.Find("Contents/Buttons/TreatButton").gameObject.AddComponent<FButton>();

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

			var treat = treatButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();
			treat.alignment = TextAlignmentOptions.Center;

			selectedItemImage.sprite = Def.GetUISprite(Assets.GetPrefab(treatTag)).first;
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
			ghostSquirrel = target.GetComponent<GhostSquirrel>();

			RefreshUI();
		}

		//private void OnShoo() => ghostSquirrel.DisAppear(true);

		private void OnButtonClick()
		{
			if (seedTrader.IsConsumed)
			{
				seedTrader.RequestTreat(!seedTrader.TreatRequested);
				RefreshUI();
			}
		}

		private void RefreshUI()
		{
			string btnText = seedTrader.TreatRequested ? CANCELBUTTON : TREATBUTTON;
			treatButton.transform.Find("Text").GetComponent<TextMeshProUGUI>().SetText(btnText);
			selectedItemLabel.SetText(seedTrader.TreatRequested ? LABEL2 : LABEL);
		}
	}
}