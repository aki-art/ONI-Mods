using FUtility;
using PrintingPodRecharge.Content.Cmps;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PrintingPodRecharge
{
	public class ModAPI
	{
		public static void RerollCustomData(MinionStartingStats stats, bool rerollName)
		{
			RerollCustomData(stats, rerollName, Mod.otherMods.IsMeepHere
				? (int)DupeGenHelper2.DupeType.Meep
				: (int)DupeGenHelper2.DupeType.Shaker);
		}

		// rerolls only the appearance of a dupe, but with type manually defined
		// 0 Shaker (default), 1 Wacky (brighter wilder hair colors), 2 Meep (dyed meep hairs)
		public static void RerollCustomData(MinionStartingStats stats, bool rerollName, int dupeType)
		{
			if (rerollName)
			{
				DupeGenHelper2.AddRandomizedData(stats, (DupeGenHelper2.DupeType)dupeType);
			}
			else
			{
				var name = stats.Name;
				var descKey = stats.NameStringKey;
				if (CustomDupe.rolledData.TryGetValue(stats, out var data))
				{
					name = data.name;
					descKey = data.descKey;
				}

				DupeGenHelper2.AddRandomizedData(stats, (DupeGenHelper2.DupeType)dupeType);


				if (CustomDupe.rolledData.TryGetValue(stats, out var newData))
				{
					newData.name = name;
					newData.descKey = descKey;
				}
			}
		}

		public static bool HasCustomHair(MinionStartingStats stats)
		{
			return CustomDupe.rolledData.TryGetValue(stats, out var dupe) && dupe.colorHair;
		}

		public static void RemoveCustomData(MinionStartingStats stats)
		{
			if (CustomDupe.rolledData.ContainsKey(stats))
			{
				CustomDupe.rolledData.Remove(stats);
			}
		}

		public static void CopyFromMinion(MinionIdentity originalIdentity, MinionIdentity newIdentity)
		{
			if (originalIdentity.TryGetComponent(out CustomDupe original)
				&& newIdentity.TryGetComponent(out CustomDupe dst))
			{
				dst.hairColor = original.hairColor;
				dst.hairID = original.hairID;
				dst.runtimeHair = original.runtimeHair;
				dst.unColoredMeep = original.unColoredMeep;
				dst.descKey = original.descKey;
				dst.dyedHair = original.dyedHair;
				dst.initialized = false;

				dst.UpdateAccessories();
			};

		}

		/// for supported hairstyles, <see cref="DupeGenHelper2.allowedHairIds"/>
		public static void SetCustomHair(MinionStartingStats stats, int hairType, Color hairColor)
		{
			if (!DupeGenHelper2.allowedHairIds.Contains(hairType))
			{
				Log.Warning($"{hairType} is not an allowed hairtype.");
				return;
			}

			if (CustomDupe.rolledData.TryGetValue(stats, out var dupe))
			{
				dupe.colorHair = true;
				dupe.hairColor = hairColor;
				dupe.hair = hairType;
			}
			else
			{
				var descKey = DupeGenHelper2.NAMES.Contains(stats.NameStringKey) ? stats.NameStringKey : DupeGenHelper2.NAMES.GetRandom();
				CustomDupe.rolledData[stats] = new CustomDupe.MinionData()
				{
					colorHair = true,
					hairColor = hairColor,
					hair = hairType,
					descKey = descKey,
					name = DupeGenHelper.SetRandomName(stats)
				};
			}
		}

		public static List<CarePackageInfo> GetCurrentPool()
		{
			return ImmigrationModifier.Instance.ActiveBundle == Bundle.None
				? null
				: ImmigrationModifier.Instance.GetActiveCarePackageBundle()?.info;
		}
	}
}
