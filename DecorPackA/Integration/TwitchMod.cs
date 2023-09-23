using DecorPackA.Buildings.StainedGlassTile;
using DecorPackA.Integration.Twitch;
using ONITwitchLib;
using ONITwitchLib.Core;
using System.Collections.Generic;
using System.Linq;

namespace DecorPackA.Integration
{
	public class TwitchMod
	{
		public static HashSet<string> forbiddenUpgradeElements;

		public static void Initialize()
		{
			if (!TwitchModInfo.TwitchIsPresent)
				return;

			var deckInst = TwitchDeckManager.Instance;

			var (floorUpgradeEvent, floorUpgradeGroup) = EventGroup.DefaultSingleEventGroup(
				 FloorUpgradeCommand.ID,
				 33,
				 STRINGS.TWITCH.FLOOR_UPGRADE.NAME);

			floorUpgradeEvent.AddListener(FloorUpgradeCommand.Run);
			floorUpgradeEvent.AddCondition(FloorUpgradeCommand.Condition);
			floorUpgradeEvent.Danger = Danger.Small;
			deckInst.AddGroup(floorUpgradeGroup);
		}

		public static void InitModCompatibility(IReadOnlyList<KMod.Mod> mods)
		{
			var bitumenMods = new HashSet<string>()
			{
				"Asphalt",
				"Baator_BumminsMod",
			};

			if (!mods.Any(mod => mod.IsEnabledForActiveDlc() && bitumenMods.Contains(mod.staticID)))
				forbiddenUpgradeElements.Add(StainedGlassTiles.GetID(SimHashes.Bitumen.ToString()));
		}
	}
}
