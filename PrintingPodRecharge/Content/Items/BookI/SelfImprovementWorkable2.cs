using FUtility;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PrintingPodRecharge.Content.Items.BookI
{
    public class SelfImprovementWorkable2 : Workable
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
                Assets.GetAnim("anim_react_thumbsup_kanim")
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
            chore = new SelfImprovementChore(this);
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
            book.OnUse(worker);
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

        private void ToggleBook(Worker worker, bool visible)
        {
            var book = worker.GetComponent<Storage>().FindFirst(gameObject.PrefabID());

            if (book != null)
            {
                Storage.MakeItemInvisible(book, !visible, false);
                book.GetComponent<KAnimControllerBase>().Offset = visible ? Vector3.zero : new Vector3(40000, 0);
            }
        }
    }
}
