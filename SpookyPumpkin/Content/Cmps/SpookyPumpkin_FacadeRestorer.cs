using FUtility;
using KSerialization;
using System;
using UnityEngine;

namespace SpookyPumpkinSO.Content.Cmps
{
	// forces the game to save without modded facades, to loading the save without my mod doesn't softlock it
	public class SpookyPumpkin_FacadeRestorer : KMonoBehaviour
	{
		[Serialize] public string facadeID;
		[SerializeField] public Func<SpookyPumpkin_FacadeRestorer, string> playAnimCb;

		[MyCmpReq] public BuildingFacade buildingFacade;
		[MyCmpReq] public KBatchedAnimController kbac;

		public override void OnSpawn()
		{
			base.OnSpawn();

			Mod.facadeRestorers.Add(this);

			if (!facadeID.IsNullOrWhiteSpace())
			{
				var facade = Db.GetBuildingFacades().TryGet(facadeID);

				if (facade != null)
				{
					buildingFacade.ApplyBuildingFacade(facade);

					if (playAnimCb != null)
						kbac.Play(playAnimCb(this));
				}
				else
					Log.Warning($"tried to restore facade {facadeID}, but it no longer seems to exist. restoring to default.");
			}
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			Mod.facadeRestorers.Remove(this);
		}

		public void OnSaveGame()
		{
			if (SPFacades.myFacades.Contains(buildingFacade.currentFacade))
			{
				facadeID = buildingFacade.currentFacade;
				buildingFacade.currentFacade = null;
			}
			else
				facadeID = null;
		}

		public void AfterSave()
		{
			if (facadeID != null)
				buildingFacade.currentFacade = facadeID;
		}
	}
}
