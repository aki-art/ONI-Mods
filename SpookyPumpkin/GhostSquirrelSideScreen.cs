using FUtility.FUI;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SpookyPumpkin.STRINGS.UI.UISIDESCREENS.GHOSTSIDESCREEN;

namespace SpookyPumpkin
{
	class GhostSquirrelSideScreen : SideScreenContent
	{
		private GameObject buttonContainer;
		public GameObject buttonsScroll;
		public FToggle cardPrefab;
		private Dictionary<Tag, FToggle> toggles = new Dictionary<Tag, FToggle>();
		private SeedTrader seedTrader;
		private GameObject selectedItemPanel;
		private Image selectedItemImage;
		private TextMeshProUGUI selectedItemLabel;
		private FButton treatButton;
		private FButton shooButton;
		private Tag selectedTag = Tag.Invalid;
		bool HasSelection = false;

		public override bool IsValidForTarget(GameObject target) => target.GetComponent<SeedTrader>() != null;

		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			titleKey = "SpookyPumpkin.UI.UISIDESCREENS.GHOSTSIDESCREEN.TITLE";

			buttonsScroll = transform.Find("Contents/Scroll View").gameObject;
			buttonContainer = transform.Find("Contents/Scroll View/Viewport/Content").gameObject;

			treatButton = transform.Find("Contents/Buttons/TreatButton").gameObject.AddComponent<FButton>();
			shooButton = transform.Find("Contents/Buttons/ShooButton").gameObject.AddComponent<FButton>();

			selectedItemPanel = transform.Find("Contents/SelectedItem").gameObject;
			selectedItemImage = selectedItemPanel.transform.Find("Panel/Image").GetComponent<Image>();
			selectedItemLabel = selectedItemPanel.transform.Find("Panel/Label").GetComponent<TextMeshProUGUI>();
			selectedItemPanel.SetActive(false);

			cardPrefab = buttonContainer.transform.Find("Panel").gameObject.AddComponent<FToggle>();
			//Helper.AddSimpleToolTip(shooButton.gameObject, MESSAGE);
		}

		protected override void OnSpawn()
		{
			base.OnSpawn();

			var shoo = shooButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();
			shoo.SetText(SHOOBUTTON);
			shoo.alignment = TextAlignmentOptions.Center;

			var treat = treatButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();
			treat.alignment = TextAlignmentOptions.Center;
		}

		public override void SetTarget(GameObject target)
		{
			base.SetTarget(target);
			seedTrader = target.GetComponent<SeedTrader>();
			gameObject.SetActive(true);
			CreateCards();
			treatButton.OnClick += OnTreat;
			if (seedTrader.requestedEntityTag != null && seedTrader.requestedEntityTag.IsValid)
			{
				if (toggles.TryGetValue(seedTrader.requestedEntityTag, out FToggle toggle))
				{
					selectedTag = seedTrader.requestedEntityTag;
					HasSelection = true;
					RefreshFetch();
				}
				else
				{
					seedTrader.CancelActiveRequest();
				}
			}

			RefreshUI();
		}

		private void RefreshUI()
		{
			string btnText = TREATBUTTON;

			if (HasSelection && selectedTag.IsValid)
			{
				btnText = CANCELBUTTON;
				selectedItemPanel.SetActive(true);
				buttonsScroll.SetActive(false);
				selectedItemImage.sprite = Def.GetUISprite(Assets.GetPrefab(selectedTag)).first;
				selectedItemLabel.SetText(selectedTag.ProperName());
			}
			else
			{
				selectedItemPanel.SetActive(false);
				buttonsScroll.SetActive(true);
			}

			treatButton.transform.Find("Text").GetComponent<TextMeshProUGUI>().SetText(btnText);
		}

		private void OnTreat()
		{
			HasSelection = !HasSelection;
			if(HasSelection)
				selectedTag = GetSelectedItem();
			RefreshFetch();
			RefreshUI();
		}

		private void RefreshFetch()
		{
			if (HasSelection)
			{
				seedTrader.InitiateTrade(selectedTag);
			}
			else
			{
				selectedTag = Tag.Invalid;
				seedTrader.CancelActiveRequest();
			}
		}

		private Tag GetSelectedItem()
		{
			return toggles.FirstOrDefault(t => t.Value.toggle.isOn).Key;
		}

		private void CreateCards()
		{
			ClearCards();
			var tags = new List<Tag>()
			{
				GrilledPrickleFruitConfig.ID,
				FruitCakeConfig.ID,
				SalsaConfig.ID, // (Stuffed Berry)
				PumkinPieConfig.ID, 
				// Slag
				"S_CottonCandy", 
				// Vanilla and Dessert
				"Candy", 
				"IceCream", 
				// Dupes Cuisine
				"KakawaCookie", 
				"KakawaBar",
				"Nut_cake",
				"KakawaSeed",
				// Palmera Tree
				"SteamedPalmeraBerry"
			};

			tags.ForEach(tag => AddNewButton(tag));
		}

		private void AddNewButton(Tag tag)
		{
			var prefab = Assets.TryGetPrefab(tag);
			if(prefab != null)
			{
				FToggle card = Util.KInstantiateUI(cardPrefab.gameObject, buttonContainer, true).GetComponent<FToggle>();
				toggles.Add(tag, card);

				card.transform.Find("Image").GetComponent<Image>().sprite = Def.GetUISprite(prefab).first;

				var tmp = card.transform.Find("Label").GetComponent<TextMeshProUGUI>();
				tmp.SetText(prefab.GetProperName());
				tmp.alignment = TextAlignmentOptions.Top;
				tmp.fontSize = 12;
			}
		}

		private void ClearCards()
		{
			foreach (var toggle in toggles)
				Util.KDestroyGameObject(toggle.Value.gameObject);
			toggles.Clear();
		}
	}
}