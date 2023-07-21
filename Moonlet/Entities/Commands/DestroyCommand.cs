using UnityEngine;

namespace Moonlet.Entities.Commands
{
	public class DestroyCommand : BaseCommand
	{
		public override void Run(GameObject go)
		{
			Util.KDestroyGameObject(go);
		}
	}
}
