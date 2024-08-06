global using FUtility;
using HarmonyLib;
using KMod;

namespace TrueSpices
{
	public class Mod : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			base.OnLoad(harmony);
			Log.PrintVersion(this);
		}
	}
}
