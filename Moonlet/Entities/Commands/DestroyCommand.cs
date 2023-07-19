using FUtility;
using Moonlet.Content.Scripts;
using System.Collections.Generic;
using UnityEngine;

namespace Moonlet.Entities.Commands
{
	public class DestroyCommand : BaseCommand
	{
		public override void RunOnPrefab(GameObject prefab)
		{
			if (prefab is null)
				return;

			MoonletEntityComponent moonletEntityComponent = prefab.AddOrGet<MoonletEntityComponent>();
			moonletEntityComponent.AddOnSpawnFn(OnEntitySpawn);
			moonletEntityComponent.test = " TEST";
		}

		private bool OnEntitySpawn(MoonletEntityComponent target, Dictionary<string, object> _)
		{
			Log.Debuglog("DestroyCommand Spawn");
			Util.KDestroyGameObject(target);
			return true;
		}
	}
}
