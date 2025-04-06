/*using Database;
using DecorPackB.Content.ModDb;
using DecorPackB.Content.Scripts.BigFossil;
using HarmonyLib;

namespace DecorPackB.Content.Scripts
{
	public class GiantExhibition : Artable
	{
		[MyCmpReq] private BigFossilCablesRenderer renderer;
		[MyCmpReq] private AnchorMonitor anchorMonitor;
		[MyCmpReq] private Operational operational;

		public static readonly Operational.Flag foundationFlag = new("decorpackb_requires_foundation", Operational.Flag.Type.Functional);

		public GiantExhibition()
		{
			SetOffsetTable(OffsetGroups.InvertedWideTable);
		}

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();

			workerStatusItem = Db.Get().DuplicantStatusItems.Arting;
			attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
			skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
			requiredSkillPerk = Db.Get().SkillPerks.AllowNuclearResearch.Id;
			overrideAnims = [Assets.GetAnim("anim_interacts_sculpture_kanim")];
			synchronizeAnims = false;
			SetWorkTime(Mod.DebugMode ? 8f : 160f);

		}

		public override void OnCompleteWork(WorkerBase worker)
		{
			if (userChosenTargetStage == null || userChosenTargetStage.IsNullOrWhiteSpace())
				SetRandomStage(worker);
			else
				SetUserChosenStage();

			shouldShowSkillPerkStatusItem = false;
			UpdateStatusItem(null);
			Prioritizable.RemoveRef(gameObject);
		}

		private void SetUserChosenStage()
		{
			SetStage(userChosenTargetStage, false);
			userChosenTargetStage = null;
		}

		private void SetRandomStage(WorkerBase worker)
		{
			var potentialStages = Db.GetArtableStages().GetPrefabStages(this.PrefabID());

			var isCeilingInReach = anchorMonitor.isCeilingInReach;
			var isGrounded = anchorMonitor.isGrounded;

			potentialStages.RemoveAll(s => IsStageInvalid(s, isCeilingInReach, isGrounded));

			Log.Debug($"2 selecting stage, options: {potentialStages.Join(s => s.Name)}");
			var selectedStage = potentialStages.GetRandom();


			SetStage(selectedStage.id, false);
			EmoteOnCompletion(worker);
		}

		private bool IsStageInvalid(ArtableStage stage, bool hangable, bool grounded)
		{
			Log.Debug($"checking {stage.id}");
			if (stage.statusItem.StatusType < ArtableStatuses.ArtableStatusType.LookingGreat)
				return true;

			var isHangableStage = DPArtableStages.hangables.Contains(stage.id);

			if (hangable)
			{
				Log.Debug($"contained {DPArtableStages.hangables.Contains(stage.id)}");
				return !isHangableStage;
			}

			return false;
		}

		public override void SetStage(string stage_id, bool skip_effect)
		{
			base.SetStage(stage_id, skip_effect);
			Trigger(DPIIHashes.FossilStageSet, Db.GetArtableStages().Get(CurrentStage).statusItem.StatusType);
		}

		private static void EmoteOnCompletion(WorkerBase worker)
		{
			new EmoteChore(worker.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, "anim_cheer_kanim",
			[
				"cheer_pre",
				"cheer_loop",
				"cheer_pst"
			], null);
		}
	}
}
*/