using ONITwitchLib;
using Twitchery.Content.Defs;
using Twitchery.Content.Scripts;
using Twitchery.Content.Scripts.Worm;

namespace Twitchery.Content.Events.EventTypes.PandoraSubEvents
{
	public class PandoraBigWormEvent(float weight) : PandoraEventBase(weight)
	{
		public override Danger GetDanger() => Danger.Deadly;

		public override void Run(PandorasBox box)
		{
			base.Run(box);

			var worm = FUtility.Utils.Spawn(BigWormConfig.ID, box.gameObject);
			var wormHead = worm.GetComponent<WormHead>();
			wormHead.BustWall();
			wormHead.lifeTime = 120f;

			End();
		}
	}
}
