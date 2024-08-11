using FUtility;
using Newtonsoft.Json;
using PeterHan.PLib.Options;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GoldenThrone.Settings
{
	[ModInfo("Golden Throne", "assets/settings_icon.png")]
	[JsonObject(MemberSerialization.OptIn)]
	[ConfigFile]
	[RestartRequired]
	public class Config
	{
		[Option(
			"GoldenThrone.STRINGS.UI.MODSETTINGS.RELIEVED_DURATION.TITLE",
			"GoldenThrone.STRINGS.UI.MODSETTINGS.RELIEVED_DURATION.TOOLTIP")]
		[Limit(0.1f, 3f)]
		[JsonProperty]
		public float RoyallyRelievedDurationInCycles { get; set; }

		[Option(
			"GoldenThrone.STRINGS.UI.MODSETTINGS.SPEED_PENALTY.TITLE",
			"GoldenThrone.STRINGS.UI.MODSETTINGS.SPEED_PENALTY.TOOLTIP")]
		[JsonProperty]
		[Limit(-100, 0)]
		public float GoldLavatoryUsePenaltyPercent { get; set; }

		[Option(
			"GoldenThrone.STRINGS.UI.MODSETTINGS.DECOR_BONUS.TITLE",
			"GoldenThrone.STRINGS.UI.MODSETTINGS.DECOR_BONUS.TOOLTIP")]
		[JsonProperty]
		[Limit(0, 50)]
		public int DecorBonus { get; set; }

		[Option(
			"GoldenThrone.STRINGS.UI.MODSETTINGS.MORAL_BONUS.TITLE",
			"GoldenThrone.STRINGS.UI.MODSETTINGS.MORAL_BONUS.TOOLTIP")]
		[JsonProperty]
		[Limit(0, 16)]
		public int MoralBonus { get; set; }

		[Option(
			"GoldenThrone.STRINGS.UI.MODSETTINGS.USE_CROWN.TITLE",
			"GoldenThrone.STRINGS.UI.MODSETTINGS.USE_CROWN.TOOLTIP")]
		[JsonProperty]
		public bool CustomCrown { get; set; }

		[Option(
			"GoldenThrone.STRINGS.UI.MODSETTINGS.USE_PARTICLES.TITLE",
			"GoldenThrone.STRINGS.UI.MODSETTINGS.USE_PARTICLES.TOOLTIP")]
		[JsonProperty]
		public bool UseParticles { get; set; }

		[Option(
			"GoldenThrone.STRINGS.UI.MODSETTINGS.PRECIOUS_METALS.TITLE",
			"GoldenThrone.STRINGS.UI.MODSETTINGS.PRECIOUS_METALS.TOOLTIP")]
		[JsonProperty]
		public string PreciousMetalsStr { get; set; }


		[Option(
			"GoldenThrone.STRINGS.UI.MODSETTINGS.ELEMENT_ID_LIST.TITLE",
			"GoldenThrone.STRINGS.UI.MODSETTINGS.ELEMENT_ID_LIST.TOOLTIP")]
		public Action<object> ElementLink => OpenElementIDs;

		private static void OpenElementIDs(object obj)
		{
			Application.OpenURL("https://aki-art.github.io/oni-ids/all_ids/Elements/");
		}

		[JsonIgnore]
		public float RoyallyRelievedDurationInSeconds => RoyallyRelievedDurationInCycles * FUtility.CONSTS.CYCLE_LENGTH;

		[JsonIgnore]
		public float LavatoryUsePenalty => GoldLavatoryUsePenaltyPercent * 0.01f;

		[JsonIgnore]
		public HashSet<SimHashes> PreciousMetals { get; set; }

		public void SetPreciousMetals()
		{
			PreciousMetals = new HashSet<SimHashes>();
			if (!PreciousMetalsStr.IsNullOrWhiteSpace())
			{
				var entries = PreciousMetalsStr.Replace(@"\s+", "").Split(',');

				foreach (var entry in entries)
				{
					if (Enum.TryParse(entry, true, out SimHashes simHash))
					{
						PreciousMetals.Add(simHash);
					}
				}

				if (PreciousMetals.Count > 0)
				{
					return;
				}
			}


			Log.Warning("Invalid elements set as precious metals. Using defaults.\n Please edit the mod settings to contain at least 1 valid element id, separated by commas.");

			PreciousMetals.Add(SimHashes.Gold);
			PreciousMetals.Add(SimHashes.GoldAmalgam);
			PreciousMetals.Add(SimHashes.FoolsGold);

			if (Enum.TryParse("SolidTitanium", out SimHashes platinumId))
			{
				PreciousMetals.Add(platinumId);
			}
		}

		public Config()
		{
			RoyallyRelievedDurationInCycles = 0.5f;
			GoldLavatoryUsePenaltyPercent = -20;
			CustomCrown = true;
			UseParticles = true;
			DecorBonus = 5;
			MoralBonus = 2;
			PreciousMetalsStr = "GoldAmalgam, Gold, FoolsGold, SolidPlatinum";
		}
	}
}
