using HarmonyLib;
using KSerialization;
using System.Collections.Generic;

namespace DecorPackB.Content.Scripts
{
	public class PausableFilteredStorage(KMonoBehaviour root, Tag[] forbiddenTags, IUserControlledCapacity capacityControl, bool useLogicMeter, ChoreType fetchChoreType) : FilteredStorage(root, forbiddenTags, capacityControl, useLogicMeter, fetchChoreType)
	{
		[Serialize] public bool isPaused;
		[Serialize] public HashSet<Tag> tags;

		// override
		[HarmonyPatch(typeof(FilteredStorage), "OnFilterChanged")]
		public class FilteredStorage_OnFilterChanged_Patch
		{
			public static bool Prefix(FilteredStorage __instance)
			{
				return !(__instance is PausableFilteredStorage pausable && pausable.isPaused);
			}
		}

		public void Pause()
		{
			isPaused = true;
			if (fetchList != null)
				fetchList.Cancel("Paused");
		}

		public void Resume()
		{
			isPaused = false;
			OnFilterChanged(filterable.GetTags());
		}
	}
}
