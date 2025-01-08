using System.Collections.Generic;
using System.Linq;
using TUNING;
using UnityEngine;

namespace PrintingPodRecharge
{
	public class DupeGenHelper
	{
		private static readonly HashSet<string> forbiddenNames =
		[
		   "Pener",
		   "Pee"
		];

		public static void AddGeneShufflerTrait(MinionStartingStats __instance)
		{
			AddRandomTraits(__instance, 1, 1, DUPLICANTSTATS.GENESHUFFLERTRAITS);
		}

		public static int AddRandomTraits(MinionStartingStats __instance, int min, int max, List<DUPLICANTSTATS.TraitVal> pool)
		{
			if (max <= 0)
			{
				return 0;
			}

			var traitDb = Db.Get().traits;

			var list = new List<DUPLICANTSTATS.TraitVal>(pool);
			list.RemoveAll(l => !IsTraitInvalid(l, __instance));

			var traitPool = list
				.Select(t => traitDb.Get(t.id))
				.Except(__instance.Traits)
				.ToList();

			traitPool.Shuffle();

			var randomExtra = Random.Range(min, max);

			randomExtra = Mathf.Min(traitPool.Count, randomExtra);

			for (var i = 0; i < randomExtra; i++)
			{
				__instance.Traits.Add(traitPool[i]);
			}

			return randomExtra;
		}

		private static bool IsTraitInvalid(DUPLICANTSTATS.TraitVal traitVal, MinionStartingStats stats)
		{
			if (DlcManager.IsContentSubscribed(traitVal.dlcId))
				return false;

			return stats.Traits.Any(t => IsTraitExclusive(traitVal, t.Id));
		}

		private static bool IsTraitExclusive(DUPLICANTSTATS.TraitVal l, string trait)
		{
			if (l.mutuallyExclusiveTraits == null)
			{
				return false;
			}
			foreach (var exclusiveTrait in l.mutuallyExclusiveTraits)
			{
				if (exclusiveTrait == trait)
				{
					return true;
				}
			}

			return false;
		}

		public static string SetRandomName(MinionStartingStats __instance)
		{
			if (Mod.otherMods.IsMeepHere)
			{
				return __instance.Name;
			}

			var name = GetRandomName();
			if (!name.IsNullOrWhiteSpace())
			{
				__instance.Name = name;
				var key = "STRINGS.DUPLICANTS.PERSONALITIES." + name.ToUpperInvariant().Replace("-", "") + ".NAME";
				Strings.Add(key, name);
				//__instance.NameStringKey = PPersonalities.SHOOK_ID;

				return name;
			}

			return "Unknown";
		}

		public static string GetRandomName()
		{
			var name = "";
			var maxAttempts = 16;

			var prefixes = STRINGS.MISC.NAME_PREFIXES.text.Split(',');
			var suffixes = STRINGS.MISC.NAME_SUFFIXES.text.Split(',');

			if (prefixes.Length == 0 || suffixes.Length == 0)
			{
				return null;
			}

			while ((name.IsNullOrWhiteSpace() || forbiddenNames.Contains(name)) && maxAttempts-- > 0)
			{
				name = prefixes.GetRandom() + suffixes.GetRandom();
			}

			return name;
		}
	}
}
