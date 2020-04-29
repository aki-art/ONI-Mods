namespace Slag.Buildings
{
    class FiltrationTileWorkable : Workable
	{
		private static readonly HashedString[] CLEAN_ANIMS = new HashedString[]
		{
			"unclog_pre",
			"unclog_loop"
		};

		private static readonly HashedString PST_ANIM = new HashedString("unclog_pst");
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			workerStatusItem = Db.Get().DuplicantStatusItems.Cleaning;
			workingStatusItem = Db.Get().MiscStatusItems.Cleaning;
			attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
			attributeExperienceMultiplier = TUNING.DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
			workAnims = CLEAN_ANIMS;
			workingPstComplete = new HashedString[]
			{
				PST_ANIM
			};
			workingPstFailed = new HashedString[]
			{
				PST_ANIM
			};
		}

		protected override void OnCompleteWork(Worker worker)
		{
			base.OnCompleteWork(worker);
		}
	}
}
