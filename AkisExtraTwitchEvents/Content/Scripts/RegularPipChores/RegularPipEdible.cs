using FUtility;
using UnityEngine;

namespace Twitchery.Content.Scripts.RegularPipChores
{
	public class RegularPipEdible : Workable
	{
		[SerializeField] public float kcalPerKg;

		public override void OnSpawn()
		{
			base.OnSpawn();
			SetWorkTime(10f);
			showProgressBar = false;
			synchronizeAnims = false;
			GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Normal);
			CreateChore();
		}

		private void CreateChore()
		{
			Log.Debuglog("creating chore");
			new RegularPipEatChore(this);
		}

		public override void OnCompleteWork(Worker worker)
		{
			base.OnCompleteWork(worker);

			new RegularPipEatChore(this);
		}
	}
}
