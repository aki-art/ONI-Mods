using PrintingPodRecharge.Content.Cmps;
using System;
using UnityEngine;

namespace PrintingPodRecharge.Content.Items
{
	public class BionicBioInkConfig : IEntityConfig, IHasDlcRestrictions
	{
		public const string ID = "PrintingPodRecharge_Bionic";

		public GameObject CreatePrefab()
		{
			return BioInkConfig.CreateBioInk(ID, STRINGS.ITEMS.BIONIC_BIO_INK.NAME, STRINGS.ITEMS.BIONIC_BIO_INK.DESC, "rrp_bionic_bioink_kanim", Bundle.Bionic);
		}

		[Obsolete]
		public string[] GetDlcIds() => null;

		public string[] GetForbiddenDlcIds() => null;

		public string[] GetRequiredDlcIds() => DlcManager.DLC3;

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
