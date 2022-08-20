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

        protected static HashedString[] symbols = new HashedString[]
        {
            "shell_breaking_0",
            "shell_breaking_1",
            "shell_breaking_2",
            "shell_breaking_3",
            "shell_breaking_4",
            "shell_breaking_5",
            "shell_breaking_6",
            "shell_breaking_7",
            "shell_breaking_8",
            "shell_breaking_9",
            "shell_breaking_10",
            "shell_breaking_11",
            "shell_breaking_12"
        };

        [Serialize]
        private bool markedForMining;

        [MyCmpReq]
        private KSelectable kSelectable;

        [MyCmpReq]
        private KPrefabID kPrefabID;

        [MyCmpReq]
        private KBatchedAnimController kbac;

        [MyCmpReq]
        private SymbolOverrideController symbolOverrideController;

        private Chore chore;

        [SerializeField]
        public float maxShellHealth = 100f;

        [Serialize]
        public float shellIntegrity;

        private int currentBreakingStage = -1;

        public int GetHardness()
        {
            return 100;
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

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
#if DEBUG
            var status_item = new StatusItem("debugshell", "Shell {0}", "", "", StatusItem.IconType.Exclamation, NotificationType.Neutral, false, default);
            status_item.SetResolveStringCallback((str, obj) => string.Format(str, (obj as MineableCreature)?.shellIntegrity));
            var status = GetComponent<KSelectable>().AddStatusItem(status_item);
#endif
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

        private void RefreshSymbols(float progress)
        {
            //var symbolIdx = progress >= 1f ? 0 : Mathf.FloorToInt(symbols.Length * (1f - progress));
            var symbolIdx = (int)(symbols.Length * progress);

            if (symbolIdx != currentBreakingStage)
            {
                var index = Mathf.Clamp(symbolIdx, 0, symbols.Length - 1);
                var symbol = kbac.AnimFiles[0].GetData().build.GetSymbol(symbols[index]);
                if (symbol != null)
                {
                    symbolOverrideController.AddSymbolOverride("shell_breaking_0", symbol);
                }

                currentBreakingStage = symbolIdx;
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
            return kPrefabID.HasTag(ModAssets.Tags.grownShell);
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
            Log.Debuglog("OnStartWork " + worker?.GetProperName());
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

            gameObject.GetSMI<ShellGrowthMonitor.Instance>().Mine();
            MarkForDig(false);
        }

        protected override bool OnWorkTick(Worker worker, float dt)
        {
            var damage = dt / workTime;
            ApplyDamage(damage);

            return base.OnWorkTick(worker, dt);
        }

        public void ApplyDamage(float damage)
        {
            shellIntegrity -= damage * maxShellHealth;
            RefreshSymbols(shellIntegrity / maxShellHealth);
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
