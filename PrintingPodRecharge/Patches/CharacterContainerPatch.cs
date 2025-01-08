using FUtility;
using HarmonyLib;
using Klei.AI;
using PrintingPodRecharge.Content.Cmps;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using TMPro;
using UnityEngine;

namespace PrintingPodRecharge.Patches
{
	public class CharacterContainerPatch
	{
		[HarmonyPatch(typeof(CharacterContainer), "SetAnimator")]
		public class CharacterContainer_SetAnimator_Patch
		{
			public static void Postfix(MinionStartingStats ___stats, KBatchedAnimController ___bgAnimController)
			{
				if (___stats.personality.model == GameTags.Minions.Models.Bionic)
					return;

				var randoDupe = ___stats != null && CustomDupe.stats.Contains(___stats);

				if (ImmigrationModifier.Instance.IsOverrideActive || randoDupe)
				{
					var activeBundle = randoDupe
						? ImmigrationModifier.Instance.GetBundle(Bundle.Shaker)
						: ImmigrationModifier.Instance.GetActiveCarePackageBundle();

					if (activeBundle.replaceAnim && activeBundle.bgAnim != null)
					{
						___bgAnimController.SwapAnims(activeBundle.bgAnim);

						var bg = activeBundle.printerBgTint;
						var glow = activeBundle.printerBgTintGlow;

						if (ImmigrationModifier.Instance.randomColor || randoDupe)
						{
							var color = GetRandomHairColorLegacy();
							bg = GetComplementaryColor(color);
							glow = GetComplementaryColor(color);
						}

						___bgAnimController.SetSymbolTint("forever", bg);
						___bgAnimController.SetSymbolTint("grid_bloom", glow);
						___bgAnimController.SetSymbolTint("inside_rough", glow);

						___bgAnimController.SetDirty();
						___bgAnimController.UpdateAnim(1);
						___bgAnimController.Play("crewSelect_bg", KAnim.PlayMode.Loop);
					}
				}
			}
		}

		private static Color GetRandomHairColorLegacy() => Random.ColorHSV(0, 1, 0f, 0.9f, 0.1f, 1f);

		private static Color GetComplementaryColor(Color color)
		{
			Color.RGBToHSV(color, out var h, out _, out _);

			h = (h + 0.5f) % 1f; // invert hue
			var s = 0.55f; // not too saturated. against the blue of the window this looks vibrant enough
			var v = 0.75f; // bright

			return Color.HSVToRGB(h, s, v);
		}

		[HarmonyPatch(typeof(CharacterContainer), "GenerateCharacter")]
		public class CharacterContainer_GenerateCharacter_Patch
		{
			public static void Prefix(ref List<Tag> ___permittedModels)
			{
				if (ImmigrationModifier.Instance.ActiveBundle == Bundle.Bionic)
					___permittedModels = [GameTags.Minions.Models.Bionic];

				var data = ImmigrationModifier.Instance.GetActiveCarePackageBundle();

				if (data != null && data.permittedDupeModels != null)
					___permittedModels = data.permittedDupeModels;
			}
		}

		// colors traits purple for vacillator traits, or yellow for need traits
		[HarmonyPatch(typeof(CharacterContainer), "SetInfoText")]
		public class CharacterContainer_SetInfoText_Patch
		{
			public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
			{
				var codes = orig.ToList();

				var m_SetSimpleTooltip = AccessTools.Method(typeof(ToolTip), "SetSimpleTooltip");
				var index = codes.FindIndex(c => c.Calls(m_SetSimpleTooltip));

				if (index == -1)
					return codes;

				var m_SetColorForTrait = AccessTools.Method(typeof(CharacterContainer_SetInfoText_Patch), "SetColorForTrait", [typeof(LocText), typeof(Trait)]);

				codes.InsertRange(index + 1,
				[
					new CodeInstruction(OpCodes.Ldloc_3),
					new CodeInstruction(OpCodes.Ldloc_1),
					new CodeInstruction(OpCodes.Call, m_SetColorForTrait)
				]);

				Log.PrintInstructions(codes);

				return codes;
			}

			private static void SetColorForTrait(LocText locTest, Trait trait)
			{
				if (ModAssets.needTraits.Contains(trait.Id))
				{
					locTest.color = ModAssets.Colors.gold;
				}
				else if (ModAssets.vacillatorTraits.Contains(trait.Id))
				{
					locTest.colorGradient = new VertexGradient()
					{
						topLeft = ModAssets.Colors.purple,
						topRight = ModAssets.Colors.purple,
						bottomLeft = ModAssets.Colors.magenta,
						bottomRight = ModAssets.Colors.magenta,
					};

					locTest.enableVertexGradient = true;
					locTest.color = Color.white;
				}
			}
		}
	}
}
