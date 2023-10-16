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
					else Log.Warn("ClusterLayout setting is unset.");
				}
			}
		}
	}
}
