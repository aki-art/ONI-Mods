using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecorPackB.Content.Scripts
{
	public class GiantExhibitionFoundationMonitor : KMonoBehaviour
	{
		[MyCmpReq] FoundationMonitor foundationMonitor;

		public AccessTools.FieldRef<FoundationMonitor, List<HandleVector<int>.Handle>> partitionerEntriesRef;

		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			partitionerEntriesRef = AccessTools.FieldRefAccess<FoundationMonitor, List<HandleVector<int>.Handle>>("partitionerEntries");
		}


	}
}
