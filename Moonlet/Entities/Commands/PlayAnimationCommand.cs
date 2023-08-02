using UnityEngine;

namespace Moonlet.Entities.Commands
{
	public class PlayAnimationCommand : BaseCommand
	{
		public string Animation { get; set; }

		public KAnim.PlayMode Mode { get; set; }

		public override void Run(GameObject go)
		{
			go.GetComponent<KBatchedAnimController>().Play(Animation, Mode);
		}
	}
}
