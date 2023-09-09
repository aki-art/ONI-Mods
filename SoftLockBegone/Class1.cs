using HarmonyLib;
using KMod;

namespace SoftLockBegone
{
	public class Class1 : UserMod2
	{
		public override void OnLoad(Harmony harmony)
		{
			base.OnLoad(harmony);
			//SaveManager.DEBUG_OnlyLoadThisCellsObjects = 
		}
	}
}