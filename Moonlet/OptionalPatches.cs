using HarmonyLib;
using Moonlet.Patches;
using System;

namespace Moonlet
{
	public class OptionalPatches
	{
		public static PatchRequests requests;

		public static void OnAllModsLoaded(Harmony harmony)
		{
			if (requests.HasFlag(PatchRequests.Enums))
			{
				Log.Debug("patching enums");
				EnumPatch.SimHashes_Parse_Patch.Patch(harmony);
				EnumPatch.SimHashes_ToString_Patch.Patch(harmony);
			}

			if (requests.HasFlag(PatchRequests.ElementColor))
			{

			}
		}

		[Flags]
		public enum PatchRequests
		{
			Enums,
			ElementColor
		}
	}
}
