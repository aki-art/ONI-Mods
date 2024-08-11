using HarmonyLib;
using Klei.AI;
using System.Collections.Generic;
using static GoldenThrone.STRINGS.DUPLICANTS.STATUSITEMS;

namespace GoldenThrone.Patches
{
	public class ModifierSetPatch
	{
		[HarmonyPatch(typeof(ModifierSet))]
		[HarmonyPatch(nameof(ModifierSet.Initialize))]
		public static class ModifierSet_Initialize_Patch
		{
			public static void Postfix(ModifierSet __instance)
			{
				__instance.effects.Add(ConfigureRoyallyRelieved());
				__instance.effects.Add(ConfigureTooComfortable());
			}

			private static Effect ConfigureRoyallyRelieved()
			{
				var effect = new Effect(
					ModAssets.Effects.RoyallyRelievedID,
					GOLDENTHRONE_ROYALLYRELIEVED.NAME,
					GOLDENTHRONE_ROYALLYRELIEVED.TOOLTIP,
					Mod.Settings.RoyallyRelievedDurationInSeconds,
					true,
					true,
					false);

				effect.SelfModifiers = new List<AttributeModifier>() {
					new AttributeModifier(Db.Get().Attributes.QualityOfLife.Id, Mod.Settings.MoralBonus, GOLDENTHRONE_ROYALLYRELIEVED.NAME),
					new AttributeModifier(Db.Get().Attributes.Decor.Id, Mod.Settings.DecorBonus, GOLDENTHRONE_ROYALLYRELIEVED.NAME)
				};

				return effect;
			}

			private static Effect ConfigureTooComfortable()
			{
				var effect = new Effect(
					ModAssets.Effects.TooComfortable,
					GOLDENTHRONE_TOOCOMFORTABLE.NAME,
					GOLDENTHRONE_TOOCOMFORTABLE.TOOLTIP,
					float.PositiveInfinity,
					true,
					true,
					true);

				effect.SelfModifiers = new List<AttributeModifier>() {
					new AttributeModifier(Db.Get().Attributes.ToiletEfficiency.Id, Mod.Settings.LavatoryUsePenalty, GOLDENTHRONE_TOOCOMFORTABLE.NAME, true, false)
				};

				return effect;
			}
		}
	}
}
