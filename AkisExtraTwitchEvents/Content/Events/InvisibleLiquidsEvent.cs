using Twitchery.Content.Scripts;

namespace Twitchery.Content.Events
{
	public class InvisibleLiquidsEvent : ITwitchEvent
	{
		public bool Condition(object data) => true;

		public string GetID() => "InvisibleLiquids";

		public void Run(object data)
		{
			AkisTwitchEvents.Instance.hideLiquids = true;
			AkisTwitchEvents.Instance.ApplyLiquidTransparency(WaterCubes.Instance);

			GameScheduler.Instance.Schedule("test", 30f, _ =>
			{
				AkisTwitchEvents.Instance.hideLiquids = false;
				AkisTwitchEvents.Instance.ApplyLiquidTransparency(WaterCubes.Instance);
			});
		}
	}
}
