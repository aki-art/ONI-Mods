using HarmonyLib;
using Klei.CustomSettings;
using ProcGen;

namespace Moonlet.Patches.WorldGenErrorChecks
{
	public class NewGameSettingSeedPatch
	{
		[HarmonyPatch(typeof(NewGameSettingSeed), "Refresh")]
		public class NewGameSettingSeed_Refresh_Patch
		{
			public static void Prefix()
			{
				Log.Debug("Loaded clusters:");
				foreach (var cluster in SettingsCache.clusterLayouts.clusterCache)
					Log.Debug($"\t - {cluster.Key} {cluster.Value.filePath}");

				if (CustomGameSettings.Instance == null) Log.Warn("CustomGameSettings.Instance is null");
				if (CustomGameSettings.Instance.GetCurrentClusterLayout() == null)
				{
					Log.Warn("CustomGameSettings.Instance.GetCurrentClusterLayout() returned null");

					var setting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout);

					if (setting != null)
					{
						var data = SettingsCache.clusterLayouts.GetClusterData(setting.id);
						if (data == null)
						{
							Log.Debug("Clusterlayout data is null.");
						}
					}
					else Log.Warn("Worldgen failure. Clusterlayout could not be generated.");
				}
			}
		}
	}
}
