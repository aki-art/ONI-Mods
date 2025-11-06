using ONITwitchLib;
using Twitchery.Content.Scripts;
using Twitchery.Utils;

namespace Twitchery.Content.Events.EventTypes.PandoraSubEvents
{
	public class PandoraExplodeEvent(float weight) : PandoraEventBase(weight)
	{
		public override Danger GetDanger() => Danger.High;

		public override void Run(PandorasBox box)
		{
			base.Run(box);

			ExplosionUtil.ExplodeInRadius(box.NaturalBuildingCell(), 1f, new UnityEngine.Vector2(8, 14), 4);

			End();
		}
	}
}
