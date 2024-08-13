using FUtility.ElementUtil;
using System.Collections.Generic;

namespace RadShieldTile.Content
{
	public class RSTElements
	{
		public static ElementInfo RadShield = ElementInfo.Solid("RadShieldTileRadShield", ModAssets.Colors.radGreen);

		public static void RegisterSubstances(List<Substance> list)
		{
			list.Add(RadShield.CreateSubstance());
		}

		public static ElementsAudio.ElementAudioConfig[] CreateAudioConfigs(ElementsAudio _)
		{
			return
			[
				ElementUtil.CopyElementAudioConfig(SimHashes.PhosphateNodules, RadShield),
			];
		}
	}
}
