using Database;
using HarmonyLib;
using Twitchery.Content.Defs.Critters;

namespace Twitchery.Patches
{
	public class ChorePreconditionsPatch
	{
		[HarmonyPatch(typeof(ChorePreconditions), MethodType.Constructor)]
		public class ChorePreconditions_Ctor_Patch
		{
			public static void Postfix(ChorePreconditions __instance)
			{
				var fn = __instance.HasSkillPerk.fn;
				__instance.HasSkillPerk.fn = (ref Chore.Precondition.Context context, object data) =>
					fn(ref context, data) || PipWithPerk(ref context, data);
			}

			private static bool PipWithPerk(ref Chore.Precondition.Context context, object data)
			{
				if (!context.consumerState.prefabid.IsPrefabID(RegularPipConfig.ID))
					return false;

				var prefabId = context.consumerState.prefabid;

				return data switch
				{
					SkillPerk skillData => prefabId.HasTag(skillData.IdHash.ToString()),
					HashedString hashData => prefabId.HasTag(hashData.ToString()),
					string strData => prefabId.HasTag(TagManager.Create(strData).hash.ToString()),
					_ => false,
				};

				/*				if (context.consumerState.worker.TryGetComponent(out RegularPip pip))
								{
									return data switch
									{
										SkillPerk skillData => pip.HasPerk(skillData.IdHash),
										HashedString hashData => pip.HasPerk(hashData),
										string strData => pip.HasPerk((HashedString)strData),
										_ => false,
									};
								}*/
			}
		}
	}
}
