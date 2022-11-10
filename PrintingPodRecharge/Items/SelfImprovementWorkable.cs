using FUtility;
using Klei.AI;
using System.Linq;
using TUNING;
using UnityEngine;
using static Operational;

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
				Assets.GetAnim("rpp_interacts_read_book_kanim")
			};

			synchronizeAnims = false;
		}

		protected override void OnSpawn()
		{
			SetWorkTime(6f);
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
			ToggleBook(worker, true);
			workTimeRemaining = GetWorkTime();
			base.OnStopWork(worker);
		}

        protected override void OnStartWork(Worker worker)
        {
            base.OnStartWork(worker);
            ToggleBook(worker, false);
        }

        private static void ToggleBook(Worker worker, bool visible)
        {
            var book = worker.GetComponent<Storage>().FindFirst(BookOfSelfImprovementConfig.ID);

            if (book != null)
            {
                Log.Debuglog("book " + book.transform.position);
                Storage.MakeItemInvisible(book, !visible, false);

                book.GetComponent<KAnimControllerBase>().Offset = visible ? Vector3.zero : new Vector3(40000, 0);
            }
        }
    }
}
