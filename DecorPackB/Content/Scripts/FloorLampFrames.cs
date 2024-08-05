using DecorPackB.Content.Defs.Buildings;
using System.Collections.Generic;
using System.Linq;

namespace DecorPackB.Content.Scripts
{
	public class FloorLampFrames
	{
		public static List<TileInfo> tileInfos =
		[
			new TileInfo(SimHashes.GoldAmalgam),
			new TileInfo(SimHashes.IronOre)
			//Aluminum
			//Cobalt
			//Copper
			//Wolframite
			//thermium
			//niob

			//zinc
			//silver
			//

			//neutronium alloy
		];

		// need fast lookups for build menu
		public static Dictionary<Tag, Tag> tileTagDict = [];

		// need fast lookups for build menu
		public static Dictionary<Tag, Tag> reverseTileTagDict = [];

		public static EffectorValues decor;

		public static string GetFormattedName(string element) => STRINGS.BUILDINGS.PREFABS.DECORPACKB_DEFAULT_FLOORLAMP.FRAME_NAME.Replace("{element}", element);


		public static void RegisterAll()
		{
			tileInfos.RemoveAll(t => t.IsInvalid);
			tileInfos.OrderBy(t => t.ID);

			foreach (var info in tileInfos)
				RegisterTile(info);
		}

		private static void RegisterTile(TileInfo info)
		{
			var config = new DefaultFloorLampConfig
			{
				name = info.ElementTag.ToString()
			};

			if (config == null) Log.Warning("config is null");
			if (config.name == null) Log.Warning("config.name is null");

			BuildingConfigManager.Instance.RegisterBuilding(config);

			var def = Assets.GetBuildingDef(config.ID);
			if (def != null)
			{
				info.ConfigureDef(def);
				tileTagDict.Add(info.ElementTag, def.Tag);
				reverseTileTagDict.Add(def.Tag, info.ElementTag);
			}
		}

		public class TileInfo
		{
			public string ID { get; private set; }

			public Tag ElementTag { get; private set; } = Tag.Invalid;

			private string[] dlcIds = DlcManager.AVAILABLE_ALL_VERSIONS;

			public bool solid = true;

			public bool IsInvalid => ElementLoader.elements is null || ElementLoader.GetElement(ElementTag) is null;

			public TileInfo(Tag elementTag)
			{
				ElementTag = elementTag;
				SetID(elementTag);
			}

			public TileInfo(SimHashes elementHash) : this(elementHash.CreateTag()) { }

			private void SetID(Tag elementName)
			{
				ID = $"{Mod.PREFIX}{elementName}{DefaultFloorLampConfig.SUFFIX}";
			}

			public void ConfigureDef(BuildingDef def)
			{
				def.ShowInBuildMenu = true;
				def.RequiredDlcIds = dlcIds;
				def.BuildingComplete.AddTag(ModTags.noBackwall);
			}

			public TileInfo DLC(string[] dlcIds)
			{
				this.dlcIds = dlcIds;
				return this;
			}
		}
	}
}
