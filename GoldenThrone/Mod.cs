using GoldenThrone.Settings;
using HarmonyLib;
using KMod;
using PeterHan.PLib.Core;
using PeterHan.PLib.Options;

namespace GoldenThrone
{
	public class Mod : UserMod2
	{
		public static Config Settings { get; private set; }


		public override void OnLoad(Harmony harmony)
		{
			base.OnLoad(harmony);
			PUtil.InitLibrary(false);
			InitConfig();
		}

		private void InitConfig()
		{
			new POptions().RegisterOptions(this, typeof(Config));
			Settings = POptions.ReadSettings<Config>() ?? new Config();
		}
	}
}
