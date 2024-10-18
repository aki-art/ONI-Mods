using KSerialization;
using UnityEngine;

namespace Moonlet.Scripts
{
	public class Moonlet_ExtendedUprootable : Uprootable
	{
		[MyCmpReq] public KBatchedAnimController kbac;

		[SerializeField] public string deathAnimation;

		[Serialize] public bool allowUprooting;

		public static bool Uproot(Uprootable __instance)
		{
			if (__instance is Moonlet_ExtendedUprootable self)
			{
				if (self.uprootComplete || self.allowUprooting)
					return true;

				self.kbac.Play(self.deathAnimation);
				self.kbac.onAnimComplete += self.UpRootedForReal;

				return false;
			}

			return true;
		}

		private void UpRootedForReal(HashedString name)
		{
			allowUprooting = true;
			Uproot();
		}
	}
}
