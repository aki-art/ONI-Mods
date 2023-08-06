using HarmonyLib;
using Twitchery.Content;

namespace Twitchery.Patches
{
	public class AccessorizerPatch
	{
		[HarmonyPatch(typeof(Accessorizer), "OnDeserialized")]
		public class Accessorizer_OnDeserialized_Patch
		{
			public static void Prefix(Accessorizer __instance)
			{
				var identity = __instance.GetComponent<MinionIdentity>();
				if (identity.personalityResourceId == (HashedString)TPersonalities.HULK)
				{
					var personality = Db.Get().Personalities.Get(TPersonalities.HULK);
					__instance.bodyData = MinionStartingStats.CreateBodyData(personality);

					__instance.accessories.RemoveAll(x => x.Get() == null);

					var accessorySlots = Db.Get().AccessorySlots;
					Switch(__instance, accessorySlots.HeadShape, __instance.bodyData.headShape);
					Switch(__instance, accessorySlots.Mouth, __instance.bodyData.mouth);
					Switch(__instance, accessorySlots.Eyes, __instance.bodyData.eyes);
					Switch(__instance, accessorySlots.Body, __instance.bodyData.body);
					Switch(__instance, accessorySlots.Arm, __instance.bodyData.arms);
					Switch(__instance, accessorySlots.ArmLower, __instance.bodyData.armslower);
					Switch(__instance, accessorySlots.ArmLowerSkin, __instance.bodyData.armLowerSkin);
					Switch(__instance, accessorySlots.ArmUpperSkin, __instance.bodyData.armUpperSkin);
					Switch(__instance, accessorySlots.Leg, __instance.bodyData.legs);
					Switch(__instance, accessorySlots.LegSkin, __instance.bodyData.legSkin);
					Switch(__instance, accessorySlots.Pelvis, __instance.bodyData.pelvis);
					Switch(__instance, accessorySlots.Cuff, __instance.bodyData.cuff);
					Switch(__instance, accessorySlots.Hand, __instance.bodyData.hand);
					Switch(__instance, accessorySlots.Belt, __instance.bodyData.belt);
					Switch(__instance, accessorySlots.Neck, __instance.bodyData.neck);
				}
			}

			private static void Switch(Accessorizer __instance, AccessorySlot slot, HashedString id)
			{
				if (__instance.GetAccessory(slot) == null)
					__instance.AddAccessory(slot.Lookup(id));
			}
		}
	}
}
