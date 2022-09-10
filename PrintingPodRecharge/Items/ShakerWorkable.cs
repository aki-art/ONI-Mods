using UnityEngine;

namespace PrintingPodRecharge.Items
{
    public class ShakerWorkable : Workable
    {
        [MyCmpReq]
        private Shaker shaker;

        private Chore chore;

		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			workerStatusItem = Db.Get().DuplicantStatusItems.Eating;

			overrideAnims = new []
			{
				Assets.GetAnim("anim_equip_clothing_kanim")
			};

			synchronizeAnims = false;
		}

		protected override void OnSpawn()
		{
			SetWorkTime(1.5f);
			shaker.OnAssign += RefreshChore;
		}

		private void CreateChore()
		{
			chore = new GetRerolledChore(this);
		}

		public void CancelChore(string reason = "")
		{
			if (chore != null)
			{
				chore.Cancel(reason);
				Prioritizable.RemoveRef(shaker.gameObject);
				chore = null;
			}
		}

		private void RefreshChore(IAssignableIdentity target)
		{
			if (chore != null)
			{
				CancelChore("Shaker Reassigned");
			}

			if (target != null)
			{
				CreateChore();
			}
		}

		protected override void OnCompleteWork(Worker worker)
		{
			worker.GetComponent<KBatchedAnimController>().TintColour = Random.ColorHSV();
			Util.KDestroyGameObject(gameObject);
		}

		protected override void OnStopWork(Worker worker)
		{
			workTimeRemaining = GetWorkTime();
			base.OnStopWork(worker);
		}
	}
}
