using GoldenThrone.Cmps;
using HarmonyLib;

namespace GoldenThrone.Patches
{
	public class FlushToiletPatch
	{
		private static bool IsGold(FlushToilet toilet)
		{
			var id = toilet.GetComponent<PrimaryElement>().ElementID;
			return Mod.Settings.PreciousMetals.Contains(id);
		}

		[HarmonyPatch(typeof(FlushToilet), "OnSpawn")]
		public class FlushToilet_OnSpawn_Patch
		{
			public static void Prefix(FlushToilet __instance)
			{
				if (IsGold(__instance))
				{
					MakeShiny(__instance);
					ReplaceAnimation(__instance);
				}
			}

			private static void ReplaceAnimation(FlushToilet flushToilet)
			{
				var kbac = flushToilet.GetComponent<KBatchedAnimController>();
				var anim = new KAnimFile[]
				{
					Assets.GetAnim("gold_flush_kanim")
				};

				// Replace main kanim
				kbac.SwapAnims(anim);

				// Animations with foreground flagged symbols get a second cached copy in a KAnimLayering instance. 
				// This needs to be also updated here
				var layering = Traverse.Create(kbac).Field<KAnimLayering>("layering").Value;
				var fgController = Traverse.Create(layering).Field<KAnimControllerBase>("foregroundController").Value;

				(fgController as KBatchedAnimController).SwapAnims(anim);


				// Rehide the symbols from the new foreground animation
				layering.HideSymbols();
			}

			private static void MakeShiny(FlushToilet flushToilet)
			{
				flushToilet.gameObject.AddComponent<RandomlyShine>();
			}
		}

		[HarmonyPatch(typeof(FlushToilet.States), "InitializeStates")]
		public class FlushToilet_States_InitializeStates_Patch
		{
			public static void Postfix(FlushToilet.States __instance)
			{
				__instance.ready.inuse
					.Enter(StartUsing);
			}

			private static void StartUsing(FlushToilet.SMInstance smi)
			{
				if (smi.master.GetComponent<ToiletWorkableUse>().worker is Worker worker && IsGold(smi.master))
				{
					worker.Trigger((int)ModHashes.BeganUsingGoldToilet);
				}
			}
		}

		[HarmonyPatch(typeof(FlushToilet), "Flush")]
		public class FlushToilet_Flush_Patch
		{
			public static void Postfix(FlushToilet __instance, Worker worker)
			{
				if (worker != null && IsGold(__instance))
				{
					worker.Trigger((int)ModHashes.FinishedUsingGoldToilet);
				}
			}
		}
	}
}
