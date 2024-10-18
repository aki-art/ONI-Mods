using Moonlet.Utils;
using UnityEngine;

namespace Moonlet.Scripts.Commands
{
	public abstract class BaseCommand
	{
		public float Chance { get; set; }

		public string At { get; set; }

		public abstract void Run(object go);

		public Conditions Conditions { get; set; }

		public HashedString GetEventHash() => At == null ? HashedString.Invalid : new HashedString(At);

		public virtual bool CanApplyTo(GameObject prefab)
		{
			if (Conditions == null)
				return true;

			if (!DlcManager.IsDlcListValidForCurrentContent(Conditions.DlcIds))
				return false;

			if (Conditions.Mods != null)
			{
				foreach (var mod in Conditions.Mods)
					if (!Mod.loadedModIds.Contains(mod))
						return false;
			}

			return true;
		}
	}
}
