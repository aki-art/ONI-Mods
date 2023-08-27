global using FUtility;
using HarmonyLib;
using KMod;

namespace Moonlet
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
