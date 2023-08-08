using FUtility;
using HarmonyLib;

namespace PrintingPodRecharge.Patches
{
	internal class AccessorizerPatch
	{

		[HarmonyPatch(typeof(Accessorizer), "OnDeserialized")]
		public class Accessorizer_OnDeserialized_Patch
		{
			public static void Prefix(Accessorizer __instance)
			{
				var identity = __instance.GetComponent<MinionIdentity>();

				if (identity == null)
					Log.Debuglog("identity is null");

				var headshape = __instance.GetAccessory(Db.Get().AccessorySlots.HeadShape);

				Log.Assert("headshape", headshape);

				if (headshape?.Id == " Root.Accessories.headshape_hulk")
				{
					identity.personalityResourceId = Db.Get().Personalities.TryGet("AKISEXTRATWITCHEVENTS_HULK").Id;
				}
				if (!IsValidPersonality(identity))
				{
					identity.personalityResourceId = Db.Get().Personalities.TryGet("MEEP").Id;
				}
			}

			private static bool IsValidPersonality(MinionIdentity identity)
			{
				return identity.personalityResourceId.IsValid
					&& Db.Get().Personalities.TryGet(identity.personalityResourceId) != null;
			}
		}
	}
}
