using FUtility;
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
		
		// keeping this data around for a few versions, just in case
		[Serialize] public Color hairColor;
		[Serialize] public string descKey;
		[Serialize] public bool dyedHair;
		[Serialize] public int hairID;
		[Serialize] public bool unColoredMeep;
		[Serialize] public bool migrated;

		[MyCmpReq] private MinionIdentity identity;

		protected override void OnSpawn()
		{
			base.OnSpawn();

			if(!migrated)
			{
				Log.Debuglog("spawned minion: " + descKey);
				Log.Debuglog("spawned minion: " + name);
			}
		}
	}
}
