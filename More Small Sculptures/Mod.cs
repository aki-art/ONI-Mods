using FUtility;
using FUtility.SaveData;
using HarmonyLib;
using KMod;
using System.Collections.Generic;

namespace MoreSmallSculptures
{
	public class Mod : UserMod2
	{
		public static List<string> mySculptureIds = new List<string>();

		public static SaveDataManager<Config> config;

		public static Config Settings => config.Settings;

		public override void OnLoad(Harmony harmony)
		{
			base.OnLoad(harmony);
			Log.PrintVersion(this);

			config = new SaveDataManager<Config>(path);
		}
	}
}
