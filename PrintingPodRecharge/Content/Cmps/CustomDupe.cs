using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PrintingPodRecharge.Content.Cmps
{
	[Obsolete]
	public class CustomDupe : KMonoBehaviour
	{
		public static HashSet<MinionStartingStats> stats = new HashSet<MinionStartingStats>();

		[Serialize] public Color hairColor;
		[Serialize] public string descKey; // used for base personality ID, but leaving the name for backwards compatibility reasons
		[Serialize] public bool dyedHair;
		[Serialize] public int hairID;
		[Serialize] public bool unColoredMeep;
	}
}
