namespace Twitchery.Content.Scripts
{
	public class AssignableWerevoleCure : AssignableGeneric
	{
		public override void OnUse(WorkerBase worker)
		{
			if (worker.TryGetComponent(out AETE_MinionStorage minion))
			{
				minion.CureWereVole();
			}
		}

		public override bool CanUse(MinionIdentity minionIdentity)
		{
			return minionIdentity.TryGetComponent(out AETE_MinionStorage storage) && storage.IsWereVole;
		}
	}
}
