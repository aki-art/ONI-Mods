using FUtility;
using System.Collections.Generic;

namespace PrintingPodRecharge
{
	public class ModData
	{
		public bool IsArtifactsInCarePackagesHere;
		public bool IsDGSMHere;
		public bool IsSomeRerollModHere;
		public bool IsMeepHere;
		public bool IsDecorPackIHere;
		public bool IsDiseasesExpandedHere;
		public bool IsTwitchIntegrationHere;
		public bool IsBeachedHere;

		public HashSet<string> modList = new HashSet<string>();

		public ModData(IReadOnlyList<KMod.Mod> mods)
		{
			// all of these mods replace the reject button with a reroll button
			var rerollMods = new HashSet<string>()
			{
				"immigrantsReroll",
				"luo001PrintingPodRefresh",
				"RefreshImmigratScreenJustForTest",
				"2363561445.Steam", // Refresh Immigrants / 刷新选人
                "2641977549.Steam", // [test]ReshufflingArchetype
            };

			foreach (var mod in mods)
			{
				if (mod.IsEnabledForActiveDlc())
				{
					modList.Add(mod.staticID);

					switch (mod.staticID)
					{
						case "DGSM":
							IsDGSMHere = true;
							break;
						case "asquared31415.Meep":
							IsMeepHere = true;
							break;
						case "DecorPackA":
							IsDecorPackIHere = true;
							break;
						case "pether-pg.DiseasesExpanded":
							IsDiseasesExpandedHere = true;
							break;
						case "beached":
							IsBeachedHere = true;
							break;
						default:
							if (rerollMods.Contains(mod.staticID))
							{
								if (!IsSomeRerollModHere)
									Log.Info($"{mod.title} found in modlist: Added default Bio-Ink to regular Care Packages as a way to obtain ink rather than rejection.");

								IsSomeRerollModHere = true;
							}
							else if (mod.staticID == "asquared31415.TwitchIntegration")
								IsTwitchIntegrationHere = true;

							break;
					}
				}
			}
		}
	}
}
