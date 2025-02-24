using Database;
using DecorPackB.Content.Db;
using HarmonyLib;

namespace DecorPackB.Content.Scripts
{
	public class GiantExhibition : Artable
	{
		private static AccessTools.FieldRef<Artable, string> userChosenTargetStage;

		private GiantFossilCableVisualizer visualizer;

		public GiantExhibition()
		{
			SetOffsetTable(OffsetGroups.InvertedWideTable);
		}

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();

			var f_userChosenTargetStage = AccessTools.Field(typeof(Artable), "userChosenTargetStage");
			userChosenTargetStage = AccessTools.FieldRefAccess<Artable, string>(f_userChosenTargetStage);

			workerStatusItem = global::Db.Get().DuplicantStatusItems.Arting;
			attributeConverter = global::Db.Get().AttributeConverters.ResearchSpeed;
			skillExperienceSkillGroup = global::Db.Get().SkillGroups.Research.Id;
			requiredSkillPerk = global::Db.Get().SkillPerks.AllowNuclearResearch.Id;

			SetWorkTime(Mod.DebugMode ? 8f : 160f);

			overrideAnims = new[]
			{
				Assets.GetAnim("anim_interacts_sculpture_kanim")
			};

			synchronizeAnims = false;
		}

		public override void OnSpawn()
		{
			base.OnSpawn();
			visualizer = GetComponentInChildren<GiantFossilCableVisualizer>();
		}

		public override void OnCompleteWork(WorkerBase worker)
		{
			if (userChosenTargetStage(this) == null || userChosenTargetStage(this).IsNullOrWhiteSpace())
				SetRandomStage(worker);
			else
				SetUserChosenStage();

			shouldShowSkillPerkStatusItem = false;
			UpdateStatusItem(null);
			Prioritizable.RemoveRef(gameObject);
		}

		private void SetUserChosenStage()
		{
			SetStage(userChosenTargetStage(this), false);
			userChosenTargetStage(this) = null;
		}

		private void SetRandomStage(WorkerBase worker)
		{
			var potentialStages = global::Db.GetArtableStages().GetPrefabStages(this.PrefabID());
			potentialStages.RemoveAll(IsStageInvalid);

			var selectedStage = potentialStages.GetRandom();

			SetStage(selectedStage.id, false);
			EmoteOnCompletion(worker);
		}

		private bool IsStageInvalid(ArtableStage stage)
		{
			if (stage.statusItem.StatusType < ArtableStatuses.ArtableStatusType.LookingGreat)
				return true;

			if (DPArtableStages.hangables.Contains(stage.id))
				return !visualizer.IsHangable();

			return !visualizer.IsGrounded();
		}

		public override void SetStage(string stage_id, bool skip_effect)
		{
			base.SetStage(stage_id, skip_effect);

			var stage = global::Db.GetArtableStages().Get(CurrentStage);
			if (stage != null)
				Trigger(DPIIHashes.FossilStageSet, stage.statusItem.StatusType);
		}

		private static void EmoteOnCompletion(WorkerBase worker)
		{
			new EmoteChore(worker.GetComponent<ChoreProvider>(), global::Db.Get().ChoreTypes.EmoteHighPriority, "anim_cheer_kanim", new HashedString[]
			{
				"cheer_pre",
				"cheer_loop",
				"cheer_pst"
			}, null);
		}
	}
}
