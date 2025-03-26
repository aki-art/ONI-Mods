using FUtility.FUI;
using PrintingPodRecharge.Content.Cmps;
using UnityEngine;
using UnityEngine.UI;

namespace PrintingPodRecharge.UI
{
	public class BioInkSidescreen : SideScreenContent
	{
		private BioPrinter printer;
		private LocText descriptionLabel;
		private TextFitter descriptionBoxFitter;

		private BioInkDropdown dropdown;

		private FButton actionButton;
		private FButton cancelButton;
		private LocText actionButtonLabel;
		private LocText cancelButtonLabel;

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();

			dropdown = transform.Find("Contents/BeakerSelector/Dropdown").gameObject.AddComponent<BioInkDropdown>();

			var descriptionBox = transform.Find("Contents/Description").GetComponent<LayoutElement>();
			descriptionLabel = descriptionBox.transform.Find("Label").GetComponent<LocText>();
			descriptionBoxFitter = descriptionBox.FindOrAddComponent<TextFitter>();
			descriptionBoxFitter.targetText = descriptionLabel;

			actionButton = transform.Find("Contents/Buttons/Deliver").FindOrAddComponent<FButton>();
			actionButtonLabel = actionButton.transform.Find("Text").FindOrAddComponent<LocText>();
			actionButtonLabel.SetText(STRINGS.UI.BIOINKSIDESCREEN.CONTENTS.BUTTONS.DELIVER.TEXT);

			cancelButton = transform.Find("Contents/Buttons/Activate").FindOrAddComponent<FButton>();
			cancelButtonLabel = cancelButton.transform.Find("Text").FindOrAddComponent<LocText>();

			dropdown.RefreshOptions();
		}


		public override void OnSpawn()
		{
			base.OnSpawn();

			cancelButtonLabel.SetText(global::STRINGS.UI.CANCELPLACEINRECEPTACLE);

			dropdown.onValueChanged += OnDropdownChanged;
			actionButton.OnClick += OnButtonClicked;
			cancelButton.OnClick += OnCancelClicked;
		}

		private void OnCancelClicked()
		{
			if (printer == null)
			{
				return;
			}

			printer.CancelDelivery();

			RefreshButtons();
		}

		private void OnButtonClicked()
		{
			if (printer == null)
			{
				return;
			}

			if (printer.CanStartPrint())
			{
				printer.StartBonusPrint();
			}
			else
			{
				printer.SetDelivery(dropdown.SelectedTag);
			}

			RefreshButtons();
		}

		private void OnDropdownChanged(int index)
		{
			if (printer != null)
			{
				printer.inkTag = dropdown.SelectedTag;
				SetDescription(dropdown.Selected.description);
				actionButton.SetInteractable(true);
			}
		}

		private void RefreshButtons()
		{
			if (printer.inkTag != Tag.Invalid)
			{
				if (printer.CanStartPrint())
				{
					dropdown.dropdown.interactable = false;

					actionButtonLabel.SetText(STRINGS.UI.BIOINKSIDESCREEN.CONTENTS.BUTTONS.ACTIVATE.TEXT);

					cancelButton.SetInteractable(false);
					actionButton.SetInteractable(!printer.isPrinterBusy);
				}
				else
				{
					actionButtonLabel.SetText(STRINGS.UI.BIOINKSIDESCREEN.CONTENTS.BUTTONS.DELIVER.TEXT);

					if (printer.isDeliveryActive)
					{
						dropdown.dropdown.interactable = false;
						actionButton.SetInteractable(false);
						cancelButton.SetInteractable(true);
					}
					else
					{
						dropdown.dropdown.interactable = true;
						actionButton.SetInteractable(true);
						cancelButton.SetInteractable(false);
					}
				}
			}
			else
			{
				actionButtonLabel.SetText(STRINGS.UI.BIOINKSIDESCREEN.CONTENTS.BUTTONS.DELIVER.TEXT);

				dropdown.dropdown.interactable = true;
				actionButton.SetInteractable(true);
				cancelButton.SetInteractable(false);
			}
		}

		public override bool IsValidForTarget(GameObject target)
		{
			return target.GetComponent<BioPrinter>() != null;
		}

		public override void SetTarget(GameObject target)
		{
			base.SetTarget(target);

			printer = target.GetComponent<BioPrinter>();

			if (printer == null)
			{
				return;
			}

			dropdown.RefreshOptions();

			SetInk(printer.lastInkTag);
			RefreshButtons();

			SetDescription(dropdown.Selected.description);
		}


		private void SetInk(Tag ink)
		{
			dropdown.SelectedTag = ink;
		}

		private void SetDescription(string text)
		{
			descriptionLabel.text = text;
			descriptionBoxFitter.SetLayoutVertical();
		}
	}
}
