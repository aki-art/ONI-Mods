using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DecorPackA.Buildings.StainedGlassTile
{
	public class StainedGlassTiles
	{
		public static Material veryShiny;

		public static string GetFormattedName(string element)
		{
			return STRINGS.BUILDINGS.PREFABS.DECORPACKA_DEFAULTSTAINEDGLASSTILE.STAINED_NAME.Replace("{element}", element);
		}

		/// Other mods who want to add tiles: <see cref="ModAPI.AddTile"/>
		public static List<TileInfo> tileInfos =
		[
			new TileInfo(SimHashes.Algae),
			new TileInfo(SimHashes.Aluminum),
			new TileInfo(SimHashes.Bitumen),
			new TileInfo(SimHashes.BleachStone),
			new TileInfo("Blood").SpecColor(ModAssets.Colors.bloodRed).NotSolid(),
			new TileInfo(SimHashes.Brine).NotSolid(),
			new TileInfo(SimHashes.BrineIce),
			new TileInfo(SimHashes.Ceramic),
			new TileInfo(SimHashes.Clay),
			new TileInfo(SimHashes.Cobalt).DLC(DlcManager.AVAILABLE_EXPANSION1_ONLY),
			new TileInfo(SimHashes.Copper),
			new TileInfo(SimHashes.Corium).DLC(DlcManager.AVAILABLE_EXPANSION1_ONLY).SpecColor(ModAssets.Colors.corium),
			new TileInfo(SimHashes.CrudeOil).NotSolid(),
			new TileInfo(SimHashes.DepletedUranium).DLC(DlcManager.AVAILABLE_EXPANSION1_ONLY).SpecColor(ModAssets.Colors.uraniumGreen),
			new TileInfo(SimHashes.Diamond).SpecColor(ModAssets.Colors.W_H_I_T_E),
			new TileInfo(SimHashes.Dirt),
			new TileInfo(SimHashes.Electrum),
			new TileInfo(SimHashes.Ethanol).NotSolid().SpecColor(Color.cyan * 1.1f),
			new TileInfo(SimHashes.Fossil).NotSolid(),
			new TileInfo(SimHashes.Gold).SpecColor(ModAssets.Colors.gold),
			new TileInfo(SimHashes.Granite),
			new TileInfo(SimHashes.HardPolypropylene).SpecColor(ModAssets.Colors.extraBlue),
			new TileInfo(SimHashes.Ice),
			new TileInfo(SimHashes.IgneousRock),
			new TileInfo(SimHashes.Iron),
			new TileInfo(SimHashes.Isoresin),
			new TileInfo(SimHashes.Katairite).SpecColor(ModAssets.Colors.abyssalite),
			new TileInfo(SimHashes.Lead),
			new TileInfo(SimHashes.Lime),
			new TileInfo(SimHashes.Naphtha).NotSolid(),
			new TileInfo(SimHashes.SugarWater).DLC(DlcManager.AVAILABLE_DLC_2).NotSolid(),
			new TileInfo(SimHashes.Niobium),
			new TileInfo(SimHashes.MaficRock),
			new TileInfo(SimHashes.Magma).SpecColor(ModAssets.Colors.bloodRed).NotSolid(),
			new TileInfo(SimHashes.Mercury).NotSolid(),
			new TileInfo(SimHashes.Milk).NotSolid(),
			new TileInfo(SimHashes.Mud).DLC(DlcManager.AVAILABLE_EXPANSION1_ONLY),
			new TileInfo(SimHashes.NuclearWaste)
				.DLC(DlcManager.AVAILABLE_EXPANSION1_ONLY)
				.NotSolid()
				.SpecColor(ModAssets.Colors.uraniumGreen),
			new TileInfo(SimHashes.Obsidian),
			new TileInfo(SimHashes.Petroleum).NotSolid(),
			new TileInfo(SimHashes.Polypropylene),
			new TileInfo(SimHashes.Phosphorite),
			new TileInfo(SimHashes.Regolith).SpecColor(ModAssets.Colors.W_H_I_T_E),
			new TileInfo(SimHashes.Rust),
			new TileInfo(SimHashes.Salt),
			new TileInfo(SimHashes.SaltWater).NotSolid(),
			new TileInfo(SimHashes.Sand),
			new TileInfo(SimHashes.SandStone),
			new TileInfo(SimHashes.SedimentaryRock),
			new TileInfo("SolidBrass"),
			new TileInfo("SolidSilver"),
			new TileInfo("SolidZinc").SpecColor(ModAssets.Colors.uraniumGreen),
			new TileInfo(SimHashes.SlimeMold),
			new TileInfo(SimHashes.Snow).SpecColor(ModAssets.Colors.W_H_I_T_E),
			new TileInfo(SimHashes.Steel),
			new TileInfo(SimHashes.Sucrose),
			new TileInfo(SimHashes.Sulfur),
			new TileInfo(SimHashes.SuperCoolant).NotSolid(),
			new TileInfo(SimHashes.SuperInsulator).SpecColor(ModAssets.Colors.extraPink),
			new TileInfo(SimHashes.Tallow),
			new TileInfo(SimHashes.TempConductorSolid),
			new TileInfo(SimHashes.Tungsten),
			new TileInfo("UnobtaniumAlloy"),
			new TileInfo(SimHashes.ViscoGel).NotSolid().SpecColor(ModAssets.Colors.extraPurple),
			new TileInfo(SimHashes.Water).NotSolid(),
			new TileInfo(SimHashes.WoodLog)
		];

		// need fast lookups for build menu
		public static Dictionary<Tag, Tag> tileTagDict = new();

		// need fast lookups for build menu
		public static Dictionary<Tag, Tag> reverseTileTagDict = new();

		public static EffectorValues decor;

		public static string GetID(string name) => $"{Mod.PREFIX}{name}StainedGlassTile";

		public static void RegisterAll()
		{
			tileInfos.RemoveAll(t => t.IsInvalid);
			tileInfos.OrderBy(t => t.ID);

			foreach (var info in tileInfos)
			{
				RegisterTile(info);
			}
		}

		private static void RegisterTile(TileInfo info)
		{
			var config = new DefaultStainedGlassTileConfig
			{
				name = info.ElementTag.ToString()
			};

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

			private Color? specColor;
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
				ID = Mod.PREFIX + elementName.ToString() + "StainedGlassTile";
			}

			public void ConfigureDef(BuildingDef def)
			{
				if (specColor.HasValue)
				{
					if (specColor == ModAssets.Colors.W_H_I_T_E)
					{
						def.BlockTileMaterial = GetVeryShiny(def);
					}
					else
					{
						def.BlockTileMaterial = new Material(def.BlockTileMaterial);
						def.BlockTileMaterial.SetColor("_ShineColour", specColor.Value);
					}
				}

				def.ShowInBuildMenu = true;
				def.RequiredDlcIds = dlcIds;
				def.BuildingComplete.AddTag(ModAssets.Tags.noBackwall);
			}

			private Material GetVeryShiny(BuildingDef def)
			{
				if (veryShiny == null)
				{
					veryShiny = new Material(def.BlockTileMaterial);
					veryShiny.SetColor("_ShineColour", ModAssets.Colors.W_H_I_T_E);
				}

				return veryShiny;
			}

			public TileInfo DLC(string[] dlcIds)
			{
				this.dlcIds = dlcIds;
				return this;
			}

			public TileInfo SpecColor(Color specColor)
			{
				this.specColor = specColor;
				return this;
			}

			public TileInfo NotSolid()
			{
				solid = false;
				return this;
			}
		}
	}
}
