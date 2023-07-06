using FUtility;
using PrintingPodRecharge.Content.Cmps;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PrintingPodRecharge
{
	public class ModAPI
	{
		public static List<CarePackageInfo> GetCurrentPool()
		{
			return ImmigrationModifier.Instance.ActiveBundle == Bundle.None
				? null
				: ImmigrationModifier.Instance.GetActiveCarePackageBundle()?.info;
		}
	}
}
