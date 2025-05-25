global using FUtility;
using HarmonyLib;
using KMod;

namespace SmartDeconstruct
{
	public class Mod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			Log.PrintVersion(this);
			base.OnLoad(harmony);
		}
	}
}
