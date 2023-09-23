using HarmonyLib;
using KMod;
using System.Collections.Generic;
using UnityEngine;

namespace DecorPackA_ExampleAddon
{
	public class Mod : UserMod2
	{
		[HarmonyPatch(typeof(Db), "Initialize")]
		public class Db_Initialize_Patch
		{
			public static void Postfix()
			{
				if (DecorPackA_ModAPI.TryInitialize(true))
				{
					DecorPackA_ModAPI.AddMoodLamp(
						"Test",
						"Test Name",
						null,
						"dpi_moodlamp_unicorn_kanim",
						Color.blue);

					DecorPackA_ModAPI.AddComponentToLampPrefab(typeof(TestMoodlampBehavior));
				}
			}
		}
	}
}
