using Klei.AI;
using System.Linq;
using TUNING;

namespace PrintingPodRecharge.Items
{
    public class SelfImprovementWorkable : Workable
	{
		[MyCmpReq]
		private SelfImprovement book;

		private Chore chore;

		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			workerStatusItem = Db.Get().DuplicantStatusItems.Equipping;

			overrideAnims = new[]
			{
				Assets.GetAnim("anim_equip_clothing_kanim")
			};

			synchronizeAnims = false;
		}

		protected override void OnSpawn()
		{
			SetWorkTime(1.5f);
			book.OnAssign += RefreshChore;
		}

		private void CreateChore()
		{
			chore = new RidBadTraitsChore(this);
		}

		public void CancelChore(string reason = "")
		{
			if (chore != null)
			{
				chore.Cancel(reason);
				Prioritizable.RemoveRef(book.gameObject);
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
			var traits = worker.GetComponent<Traits>();

			var badTrait = traits.TraitList.Find(t => !t.PositiveTrait && ModAssets.badTraits.Contains(t.Id));
			if(badTrait != null)
            {
				traits.Remove(badTrait);
            }

			Util.KDestroyGameObject(gameObject);
		}

		protected override void OnStopWork(Worker worker)
		{
			workTimeRemaining = GetWorkTime();
			base.OnStopWork(worker);
		}
	}
}