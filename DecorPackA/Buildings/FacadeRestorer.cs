using KSerialization;
using UnityEngine;

namespace DecorPackA.Buildings
{
	// forces the game to save without modded facades, to loading the save without my mod doesn't softlock it
	public class FacadeRestorer : KMonoBehaviour
	{
		[Serialize] public string facadeID;

		[SerializeField] public string defaultAnim;

		[MyCmpReq] public BuildingFacade buildingFacade;
		[MyCmpReq] public KBatchedAnimController kbac;

		public override void OnSpawn()
		{
			base.OnSpawn();

			Mod.facadeRestorers.Add(this);

			if (facadeID != null)
			{
				var facade = Db.GetBuildingFacades().TryGet(facadeID);

				if (facade != null)
				{
					buildingFacade.ApplyBuildingFacade(facade);
					if (!TryGetComponent(out BuildingComplete _))
						kbac.Play("place");             // only works for built. place has some weird bug
														// that is not in my scope to fix:
														// https://forums.kleientertainment.com/klei-bug-tracker/oni/skinned-bed-not-yet-builded-appear-as-if-it-was-after-a-reload-r39445/
					else if (defaultAnim != null)
						kbac.Play(defaultAnim);
				}
				else
				{
					Log.Warning($"tried to restore facade {facadeID}, but it no longer seems to exist. restoring to default.");
				}
			}
		}

		public void OnSave()
		{
			if (DPFacades.myFacades.Contains(buildingFacade.currentFacade))
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
