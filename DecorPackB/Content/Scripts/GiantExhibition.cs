using Database;
using DecorPackB.Content.ModDb;
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

		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();

			var f_userChosenTargetStage = AccessTools.Field(typeof(Artable), "userChosenTargetStage");
			userChosenTargetStage = AccessTools.FieldRefAccess<Artable, string>(f_userChosenTargetStage);

			workerStatusItem = Db.Get().DuplicantStatusItems.Arting;
			attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
			skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
			requiredSkillPerk = Db.Get().SkillPerks.AllowNuclearResearch.Id;

			SetWorkTime(Mod.DebugMode ? 8f : 160f);

			overrideAnims = new[]
			{
				Assets.GetAnim("anim_interacts_sculpture_kanim")
			};

			synchronizeAnims = false;
		}

		protected override void OnSpawn()
		{
			base.OnSpawn();
			visualizer = GetComponentInChildren<GiantFossilCableVisualizer>();
		}

		protected override void OnCompleteWork(Worker worker)
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

		private void SetRandomStage(Worker worker)
		{
			var potentialStages = Db.GetArtableStages().GetPrefabStages(this.PrefabID());
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

			var stage = Db.GetArtableStages().Get(CurrentStage);
			if (stage != null)
				Trigger(DPIIHashes.FossilStageSet, stage.statusItem.StatusType);
		}

		private static void EmoteOnCompletion(Worker worker)
		{
			new EmoteChore(worker.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, "anim_cheer_kanim", new HashedString[]
			{
				"cheer_pre",
				"cheer_loop",
				"cheer_pst"
			}, null);
		}
	}
}
