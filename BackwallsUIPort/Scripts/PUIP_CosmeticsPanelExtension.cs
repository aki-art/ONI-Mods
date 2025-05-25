using FUtility.FUI;
using UnityEngine;

namespace BackwallsUIPort.Scripts
{
	public class PUIP_CosmeticsPanelExtension : KMonoBehaviour
	{
		public KButton copyPatternButton;
		public KButton rotateButton;

		public void RefreshUI()
		{
			if (copyPatternButton == null)
			{
				// Body/BodySkin/CategoryHeader/Horizontal/ScrollRect/Content/cosmeticsPanel
				// Body/BodyMaterial/ScrollRect/Content/DetailsScreenMaterialPanel/OpenChangeMaterialButton

				Transform parent = transform;
				while (parent.name != "Body")
				{
					if (parent.parent == null)
					{
						Log.Warning("cannot initialize UI. Did this sidescreen change?");
						Helper.ListChildren(transform);
						return;
					}

					parent = parent.parent;
				}

				var buttonPrefab = parent.Find("BodyMaterial/ScrollRect/Content/DetailsScreenMaterialPanel/OpenChangeMaterialButton");

				if (buttonPrefab == null)
				{
					Log.Warning("cannot initialize UI. Did this sidescreen change?");
					Helper.ListChildren(transform);
					return;
				}

				var facadeSelector = transform.Find("CosmeticSlotContainer/FacadeSelectionPanel/FacadeSelector");
				copyPatternButton = Instantiate(buttonPrefab).GetComponent<KButton>();
				copyPatternButton.transform.Find("Label").GetComponent<LocText>().SetText("Copy Settings");
				Helper.ListComponents(facadeSelector.gameObject);

				copyPatternButton.transform.parent = facadeSelector;

				rotateButton = Instantiate(buttonPrefab).GetComponent<KButton>();
				rotateButton.transform.parent = facadeSelector;
				rotateButton.transform.Find("Label").GetComponent<LocText>().SetText("Rotate");

				rotateButton.transform.parent = facadeSelector;
			}
		}
	}
}
