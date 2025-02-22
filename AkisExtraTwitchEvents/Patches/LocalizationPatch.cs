﻿using HarmonyLib;
using Twitchery.Content;
using Twitchery.Content.Events.EventTypes;

namespace Twitchery.Patches
{
	public class LocalizationPatch
	{
		[HarmonyPatch(typeof(Localization), "Initialize")]
		public class Localization_Initialize_Patch
		{
			public static void Postfix()
			{
				FUtility.FLocalization.Translations.RegisterForTranslation(typeof(STRINGS), true);

				// duplicate entry, but the game expects both keys to exist
				Strings.Add("STRINGS.ITEMS.FOOD.AETE_JELLO.NAME", STRINGS.ELEMENTS.AETE_JELLO.NAME);
				Strings.Add("STRINGS.ITEMS.FOOD.AETE_JELLO.DESC", STRINGS.ELEMENTS.AETE_JELLO.DESC);

				Strings.Add("STRINGS.ITEMS.FOOD.AETE_FROZENHONEY.NAME", STRINGS.ELEMENTS.AETE_FROZENHONEY.NAME);
				Strings.Add("STRINGS.ITEMS.FOOD.AETE_FROZENHONEY.DESC", STRINGS.ELEMENTS.AETE_FROZENHONEY.DESC);

				Strings.Add("STRINGS.ELEMENTS.AETE_FAKELUMBER.NAME", global::STRINGS.ELEMENTS.WOODLOG.NAME);
				Strings.Add("STRINGS.ELEMENTS.AETE_FAKELUMBER.DESC", global::STRINGS.ELEMENTS.WOODLOG.DESC);

				Strings.Add($"STRINGS.AETE_EVENTS.{PimplesEventMedium.ID.ToUpperInvariant()}.TOAST", STRINGS.AETE_EVENTS.PIMPLES.TOAST);
				Strings.Add($"STRINGS.AETE_EVENTS.{PimplesEventMedium.ID.ToUpperInvariant()}.DESC", STRINGS.AETE_EVENTS.PIMPLES.DESC);

				Strings.Add($"STRINGS.AETE_EVENTS.{SolarStormEventMedium.ID.ToUpperInvariant()}.DESC", STRINGS.AETE_EVENTS.SOLARSTORMSMALL.DESC);

				Strings.Add("STRINGS.DUPLICANTS.MODIFIERS.AKISEXTRATWITCHEVENTS_CAFFEINATEDEFFECT.ADDITIONAL_EFFECTS", string.Format(STRINGS.DUPLICANTS.MODIFIERS.AKISEXTRATWITCHEVENTS_CAFFEINATEDEFFECT.ADDITIONAL_EFFECTS, GameUtil.GetFormattedPercent((TEffects.WORKSPEED_MULTIPLIER - 1f) * 100f)));
			}
		}
	}
}
