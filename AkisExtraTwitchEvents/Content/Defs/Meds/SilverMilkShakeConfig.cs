/*using System;
using System.Collections.Generic;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Defs.Meds
{
	public class SilverMilkShakeConfig : IEntityConfig
	{
		public const string ID = "AkisExtraTwitchEvents_SilverMilkShake";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateLooseEntity(
				ID,
				"Silver Milkshake",
				"Cures a Werevole",
				1f,
				true,
				Assets.GetAnim("pill_foodpoisoning_kanim"),
				"object",
				Grid.SceneLayer.Creatures,
				EntityTemplates.CollisionShape.RECTANGLE,
				0.8f,
				0.4f,
				true,
				additionalTags: new List<Tag>()
				{
					ONITwitchLib.ExtraTags.OniTwitchSurpriseBoxForceDisabled
				});

			var med = new MedicineInfo(ID, null, MedicineInfo.MedicineType.CureSpecific, null, new string[] { });

			EntityTemplates.ExtendEntityToMedicine(prefab, med);
			prefab.GetComponent<MedicinalPillWorkable>().OnWorkableEventCB += (workable, ev) =>
			{
				if (ev == Workable.WorkableEvent.WorkCompleted
				&& workable.worker != null)
				{
					if (workable.worker.TryGetComponent(out AETE_MinionStorage minion))
					{
						minion.CureWereVole();
					}
				}
			};

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
*/