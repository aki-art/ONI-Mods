using FUtility;
using ImGuiNET;
using Klei.AI;
using KSerialization;

namespace Twitchery.Content.Scripts
{
	public class WereVoleContainer : StateMachineComponent<WereVoleContainer.SMInstance>, IImguiDebug
	{
		[MyCmpReq] private MinionStorage minionStorage;
		[MyCmpReq] private UserNameable nameable;
		[MyCmpReq] private SymbolOverrideController soc;
		[MyCmpReq] private KBatchedAnimController kbac;

		[Serialize] public float diggingBonus;
		[Serialize] public string minionKey;

		public int extraDigging = 10;

		public AttributeModifier diggingModifier;

		private Ref<MinionAssignablesProxy> assignablesProxy;


		public override void OnSpawn()
		{
			base.OnSpawn();
			GameClock.Instance.Subscribe((int)GameHashes.NewDay, OnNewDay);

			smi.StartSM();
			kbac.SetSymbolVisiblity("del_ginger1", false);
			kbac.SetSymbolVisiblity("del_ginger2", false);
			kbac.SetSymbolVisiblity("del_ginger3", false);
			kbac.SetSymbolVisiblity("del_ginger4", false);
			kbac.SetSymbolVisiblity("del_ginger5", false);
			kbac.SetSymbolVisiblity("snapto_pivot", false);
			kbac.SetSymbolVisiblity("tag", false);

			UpdateAnimation();

			if (minionStorage.serializedMinions.Count > 0)
				UpdateDiggingBonus();

			Mod.wereVoles.Add(this);
		}

		private void UpdateAnimation()
		{
			var colorSet = TWereVoleSkins.GetForPersonality(minionKey);

			var drillOverrides = Assets.GetAnim("aete_werevole_tinterswap_kanim");
			var head = drillOverrides.GetData().build.GetSymbol("head");
			var body = drillOverrides.GetData().build.GetSymbol("body");

			soc.AddSymbolOverride("body_drilltinter", body, 4);
			soc.AddSymbolOverride("head_drilltinter", head, 4);

			kbac.SetSymbolTint("body_drilltinter", colorSet.drill);

			kbac.SetSymbolTint("body", colorSet.skin);
			kbac.SetSymbolTint("tail", colorSet.skin);
			kbac.SetSymbolTint("head", colorSet.skin);

			kbac.SetSymbolTint("tail_bloom", colorSet.accent);
			kbac.SetSymbolTint("body_bloom", colorSet.accent);
		}

		private void OnNewDay(object obj) => ReleaseMinion();

		public void FromMinion(AETE_MinionStorage minion)
		{
			if (minionStorage.serializedMinions.Count > 0)
				return;

			var identity = minion.GetComponent<MinionIdentity>();

			minionKey = identity.nameStringKey;
			diggingBonus = minion.GetAttributes().Get(Db.Get().Attributes.Digging.Id).GetTotalValue();

			nameable.SetName(minion.GetProperName());
			assignablesProxy = identity.assignableProxy;
			minionStorage.SerializeMinion(minion.gameObject);

			UpdateDiggingBonus();
			UpdateAnimation();
		}

		private void UpdateDiggingBonus()
		{
			var attributes = this.GetAttributes();

			if (diggingModifier != null)
				attributes.Remove(diggingModifier);

			diggingModifier = new AttributeModifier(
				Db.Get().Attributes.Digging.Id,
				diggingBonus + extraDigging,
				STRINGS.DUPLICANTS.TRAITS.AKISEXTRATWITCHEVENTS_WEREVOLE.NAME);


			attributes.Add(diggingModifier);
		}

		public void ReleaseMinion()
		{
			var minion = minionStorage.serializedMinions[0];
			minionStorage.DeserializeMinion(minion.id, transform.position);

			foreach (var storage in GetComponents<Storage>())
				storage.DropAll();

			Util.KDestroyGameObject(gameObject);
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			GameClock.Instance.Unsubscribe((int)GameHashes.NewDay, OnNewDay);
			Mod.wereVoles.Remove(this);
		}

		public void OnImgui()
		{
			ImGui.Text($"Dupes: {minionStorage.serializedMinions.Count}");

			if (smi.IsInsideState(smi.sm.idle) && ImGui.Button("Return and release"))
				smi.sm.forceReturn.Trigger(smi);

			if (ImGui.Button("Release dupe"))
				ReleaseMinion();
		}

		public class States : GameStateMachine<States, SMInstance, WereVoleContainer>
		{
			public State idle;
			public State returning;
			public State changingBack;
			public State changeBack;

			public Signal forceReturn;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = idle;

				idle
					.UpdateTransition(returning, IsDaybreakSoon)
					.OnSignal(forceReturn, returning);

				returning
					.ToggleBehaviour(TTags.returningHome, HasTargetCell, ClearTargetCell)
					.EventTransition(ModEvents.FoundSafety, changingBack);

				changingBack
					.QueueAnim("shearing_pre")
					.OnAnimQueueComplete(changeBack);

				changeBack
					.Enter(smi => smi.master.ReleaseMinion());
			}

			private void ClearTargetCell(SMInstance smi)
			{
				smi.targetCell = -1;
			}

			private bool HasTargetCell(SMInstance smi)
			{
				var minion = smi.master.minionStorage.serializedMinions[0];
				var serializedMinion = minion.serializedMinion.Get();

				if (serializedMinion == null)
					return false;

				// trying bed
				var owner = serializedMinion.GetComponent<StoredMinionIdentity>().GetSoleOwner();
				if (owner != null)
				{
					var bed = owner.GetSlot(Db.Get().AssignableSlots.Bed);

					if (bed != null)
					{
						smi.targetCell = Grid.PosToCell(bed.assignable); // TODO
						return true;
					}
				}

				smi.targetCell = Grid.PosToCell(GameUtil.GetActiveTelepad().transform.position); // TODO
				return true;
			}

			private bool IsDaybreakSoon(SMInstance smi, float dt)
			{
				return GameClock.Instance.GetTimeSinceStartOfCycle() > (CONSTS.CYCLE_LENGTH - 10f);
			}
		}

		public class SMInstance(WereVoleContainer master) : GameStateMachine<States, SMInstance, WereVoleContainer, object>.GameInstance(master)
		{
			public int targetCell;
		}
	}
}
