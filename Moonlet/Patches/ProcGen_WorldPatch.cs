﻿using System.Linq;

namespace Moonlet.Patches
{
	public class ProcGen_WorldPatch
	{
		//[HarmonyPatch(typeof(ProcGen.World.AllowedCellsFilter), nameof(ProcGen.World.AllowedCellsFilter.Validate))]
		public class ProcGen_World_AllowedCellsFilter_Validate_Patch
		{
			// discarding file location validation for Moonlet files
			public static bool Prefix(string parentFile, ProcGen.World __instance)
			{
				if (__instance == null)
				{
					Log.Warn("WTF");
					return true;
				}

				if (parentFile == null)
				{
					Log.Warn("ParentFile wtf");
					return true;
				}

				if (__instance.subworldFiles != null)
				{
					foreach (var subWorld in __instance.subworldFiles)
					{
						if (subWorld == null)
						{
							Log.Warn("subworld null");
							continue;
						}

						if (!__instance.subworldFiles.Any(s =>
						{
							if (s == null)
							{
								Log.Warn("s null");
								return true;
							}

							return s.name == subWorld.name;
						}))
							Log.Warn($"World {parentFile}: should include {subWorld.name} in its subworldFiles since it's used in a command");
					}
				}

				return false;
			}
		}
	}
}
