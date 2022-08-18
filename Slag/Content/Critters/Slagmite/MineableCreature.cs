using FUtility;
using KSerialization;
using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace Slag.Content.Critters.Slagmite
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class MineableCreature : Workable, IApproachable
    {
        public bool allowMining = true;

        private static List<Tuple<string, Tag>> lasersForHardness;

        [Serialize]
        private bool markedForMining;

        [MyCmpReq]
        private KSelectable kSelectable;

        [MyCmpReq]
        private KPrefabID kPrefabID;

        [MyCmpReq]
        private Navigator navigator;

        private Chore chore;

        public int GetHardness()
        {
            return 100;
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            //lasersForHardness = typeof(Diggable).GetField("lasersForHardness").GetValue(null) as List<Tuple<string, Tag>>;

            workerStatusItem = Db.Get().DuplicantStatusItems.Digging;
            readyForSkillWorkStatusItem = Db.Get().BuildingStatusItems.DigRequiresSkillPerk;


            resetProgressOnStop = true;
            faceTargetWhenWorking = true;

            attributeConverter = Db.Get().AttributeConverters.DiggingSpeed;
            attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;

            skillExperienceSkillGroup = Db.Get().SkillGroups.Mining.Id;
            skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;

            multitoolContext = "dig";
            multitoolHitEffectTag = "fx_dig_splash";

            workingPstComplete = null;
            workingPstFailed = null;

            preferUnreservedCell = true;

            SetOffsetTable(OffsetGroups.InvertedStandardTable);

            Prioritizable.AddRef(gameObject);
        }
        public new int GetCell()
        {
            return Grid.PosToCell(this);
        }

        protected override void OnSpawn()
        {
            Subscribe((int)GameHashes.Died, OnDeath);
            Subscribe((int)GameHashes.RefreshUserMenu, OnRefreshUserMenu);
            Subscribe((int)GameHashes.TagsChanged, OnTagsChanged);

            if (markedForMining)
            {
                Prioritizable.AddRef(gameObject);
            }

            UpdateStatusItem();
            UpdateChore();
            SetWorkTime(10f);
        }

        /*
        public override Vector3 GetTargetPoint()
        {
            var pos = GetComponent<KBoxCollider2D>()?.bounds.center ?? transform.GetPosition();
            pos.z = 0f;

            return pos;
        }
        */

        private void OnRefreshUserMenu(object data)
        {
            if (IsMineable())
            {
                if (!markedForMining)
                {
                    var info = new KIconButtonMenu.ButtonInfo("action_uproot", UI.USERMENUACTIONS.DIG.NAME, () => MarkForDig(true), tooltipText: UI.USERMENUACTIONS.DIG.TOOLTIP);
                    Game.Instance.userMenu.AddButton(gameObject, info);
                }
                else
                {
                    var info = new KIconButtonMenu.ButtonInfo("icon_cancel", UI.USERMENUACTIONS.CANCELDIG.NAME, () => MarkForDig(false), tooltipText: UI.USERMENUACTIONS.CANCELDIG.TOOLTIP);
                    Game.Instance.userMenu.AddButton(gameObject, info);
                }
            }
        }

        private void MarkForDig(bool dig)
        {
            MarkForDig(dig, new PrioritySetting(PriorityScreen.PriorityClass.basic, 5));
        }

        private void MarkForDig(bool mark, PrioritySetting priority)
        {
            mark = mark && IsMineable();

            if (markedForMining && !mark)
            {
                Prioritizable.RemoveRef(gameObject);
            }
            else if (!markedForMining && mark)
            {
                Prioritizable.AddRef(gameObject);
                if (TryGetComponent(out Prioritizable prioritizable))
                {
                    prioritizable.SetMasterPriority(priority);
                }
            }

            markedForMining = mark;
            UpdateStatusItem();
            UpdateChore();
        }

        public bool IsMineable()
        {
            return true;// !kPrefabID.HasTag(ModAssets.Tags.beingMined);
        }

        private void OnTagsChanged(object obj)
        {
            MarkForDig(markedForMining);
        }

        private void OnDeath(object obj)
        {
            allowMining = false;
            markedForMining = false;
            UpdateChore();
        }

        private void UpdateStatusItem()
        {
            shouldShowSkillPerkStatusItem = markedForMining;
            base.UpdateStatusItem(null);

            if (markedForMining)
            {
                kSelectable.AddStatusItem(Db.Get().MiscStatusItems.WaitingForDig);
            }
            else
            {
                kSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.WaitingForDig, false);
            }
        }

        protected override void OnStartWork(Worker worker)
        {
            kPrefabID.AddTag(GameTags.Creatures.Stunned);
            kPrefabID.AddTag(ModAssets.Tags.beingMined);
        }

        protected override void OnStopWork(Worker worker)
        {
            kPrefabID.RemoveTag(GameTags.Creatures.Stunned);
            kPrefabID.RemoveTag(ModAssets.Tags.beingMined);
        }

        protected override void OnCompleteWork(Worker worker)
        {
            Log.Debuglog("COMPLETE");
            MarkForDig(false);
        }

        protected override bool OnWorkTick(Worker worker, float dt)
        {
            var damage = dt / workTime;
            this.GetSMI<ShellDamageMonitor.Instance>().Damage(damage);
            return base.OnWorkTick(worker, dt);
        }

        private void UpdateChore()
        {
            if (markedForMining && chore == null)
            {
                chore = new WorkChore<MineableCreature>(Db.Get().ChoreTypes.Dig, this, is_preemptable: true);
            }
            else
            {
                if (!markedForMining && chore != null)
                {
                    chore.Cancel("not marked for mining");
                    chore = null;
                }
            }
        }

        public override List<Descriptor> GetDescriptors(GameObject go)
        {
            var descriptors = base.GetDescriptors(go);
            if (allowMining)
            {
                descriptors.Add(new Descriptor(STRINGS.UI.MINEABLE.TITLE, STRINGS.UI.MINEABLE.TOOLTIP));
            }

            return descriptors;
        }
    }
}
