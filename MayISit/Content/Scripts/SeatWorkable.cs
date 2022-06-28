using Klei.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUNING;

namespace MayISit.Content.Scripts
{
    internal class SeatWorkable : Workable, IWorkerPrioritizable
    {
        public int basePriority = RELAXATION.PRIORITY.TIER0;

		[MyCmpReq]
		private Seat seat;

        private SeatWorkable()
        {
            SetReportType(ReportManager.ReportType.PersonalTime);
        }

		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();

			overrideAnims = new KAnimFile[]
			{
				Assets.GetAnim("anim_interacts_sit_kanim")
			};

			workAnims = null;
			workingPstComplete = null;
			workingPstFailed = null;
			showProgressBar = true;
			resetProgressOnStop = true;
			synchronizeAnims = false;
			lightEfficiencyBonus = false;
			SetWorkTime(150f);
		}

		protected override void OnCompleteWork(Worker worker)
		{
			var effects = worker.GetComponent<Effects>();
			effects.Add(seat.specificEffect, true);
			effects.Add(seat.trackingEffect, true);
		}

        protected override void OnStartWork(Worker worker)
        {
			worker.GetComponent<Effects>().Add(ModDb.Effects.RELAXING, false);
		}

        protected override void OnStopWork(Worker worker)
        {
			worker.GetComponent<Effects>().Remove(ModDb.Effects.RELAXING);
		}

        public bool GetWorkerPriority(Worker worker, out int priority)
		{
			priority = basePriority;

			var component = worker.GetComponent<Effects>();

			if (component.HasEffect(seat.trackingEffect))
			{
				priority = 0;
				return false;
			}

			if (component.HasEffect(seat.specificEffect))
			{
				priority = RELAXATION.PRIORITY.RECENTLY_USED;
			}

			return true;
		}
    }
}
