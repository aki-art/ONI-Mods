using FUtility;
using HarmonyLib;
using System.Reflection;
using Twitchery.Content.Scripts;

namespace Twitchery.Patches
{
	public class TwitchDeckManagerPatch
	{
		private static PropertyInfo p_EventId;
		private static PropertyInfo p_Group;
		private static PropertyInfo p_EventNamespace;
		private static FieldInfo f_Name;

		public static void TryPatch(Harmony harmony)
		{
			var type = ONITwitchLib.Core.CoreTypes.TwitchDeckManagerType;
			if (type != null)
			{
				var original = type.GetMethod("Draw", BindingFlags.Public | BindingFlags.Instance);

				if (original == null)
				{
					Log.Warning("TwitchDeckManager.Draw doesn't exist or it's signature was changed.");
					return;
				}

				var postfix = typeof(TwitchDeckManagerPatch).GetMethod(nameof(Postfix), []);

				p_EventId = ONITwitchLib.Core.CoreTypes.EventInfoType.GetProperty("EventId");
				p_EventNamespace = ONITwitchLib.Core.CoreTypes.EventInfoType.GetProperty("EventNamespace");
				p_Group = ONITwitchLib.Core.CoreTypes.EventInfoType.GetProperty("Group");
				f_Name = ONITwitchLib.Core.CoreTypes.EventGroupType.GetField("Name");

				harmony.Patch(original, postfix: new HarmonyMethod(postfix));
			}
		}

		public static void Postfix(object __result)
		{
			var eventId = p_EventId.GetValue(__result) as string;
			var nameSpace = p_EventNamespace.GetValue(__result) as string;
			var group = p_Group.GetValue(__result);
			var groupId = group == null ? string.Empty : f_Name.GetValue(group) as string;

			AkisTwitchEvents.Instance.OnDraw(groupId, nameSpace, eventId);
		}
	}
}
