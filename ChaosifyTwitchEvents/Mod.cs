using HarmonyLib;
using KMod;
using ONITwitchLib;
using ONITwitchLib.Core;

namespace ChaosifyTwitch
{
	public class Mod : UserMod2
	{
		[HarmonyPatch(typeof(Db), "Initialize")]
		public class Db_Initialize_Patch
		{
			[HarmonyPriority(Priority.Last)]
			public static void Postfix()
			{
				if (!TwitchModInfo.TwitchIsPresent)
					return;

				foreach (var group in TwitchDeckManager.Instance.GetGroups())
				{
					var weights = group.GetWeights();
					if (weights != null)
					{
						foreach (var weight in weights)
						{
							if (weight.Value > 0 && weight.Value < 99)
								group.SetWeight(weight.Key, Consts.EventWeight.Common);
						}
					}
				}
			}
		}
	}
}
