using HarmonyLib;
using Twitchery.Content.Scripts;

namespace Twitchery.Patches
{
	// Update the hair color on the little UI dupe portraits
	public class CrewPortraitPatch
	{
		[HarmonyPatch(typeof(CrewPortrait), "SetPortraitData")]
		public class CrewPortrait_SetPortraitData_Patch
		{
			public static void Postfix(IAssignableIdentity identityObject, KBatchedAnimController controller)
			{
				if (controller == null)
					return;

				if (identityObject is MinionIdentity identity && identity.TryGetComponent(out AETE_MinionStorage storage))
					storage.ApplyTwitchLook(controller);

				if (identityObject is StoredMinionIdentity storedIdentity && storedIdentity.TryGetComponent(out storage))
					storage.ApplyTwitchLook(controller);

				else if (identityObject is MinionAssignablesProxy proxy && proxy.target is KMonoBehaviour minion && minion.TryGetComponent(out storage))
					storage.ApplyTwitchLook(controller);
			}
		}
	}
}
