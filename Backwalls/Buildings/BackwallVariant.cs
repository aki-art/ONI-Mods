using System;
using UnityEngine;

namespace Backwalls.Buildings
{
	// stores info on the patterns, like texture and UI sprite
	public class BackwallPattern : IComparable<BackwallPattern>
	{
		public TextureAtlas atlas;
		public Texture2D specularTexture;
		public Material uniqueMaterial;
		public Color specularColor;
		public Sprite UISprite;
		private int sortOrder;
		public readonly string ID;
		public string HashedId;
		public string HashedIdShiny;
		public string BorderTag;
		private static TextureAtlas defaultAtlas;
		public string name;
		public float biomeTint = 0.2f;

		public int CompareTo(BackwallPattern other)
		{
			return sortOrder.CompareTo(other.sortOrder);
		}

		public int GetSortOrder()
		{
			return sortOrder;
		}

		public BackwallPattern(string ID, string name, Texture2D texture, Sprite UISprite, int sortOrder)
		{
			if (defaultAtlas == null)
			{
				defaultAtlas = Assets.GetTextureAtlas("tiles_solid");
			}

			Log.Debug("registered wall with ID " + ID);

			atlas = CreateAtlas(defaultAtlas, texture);
			this.name = name;
			this.UISprite = UISprite;
			this.ID = ID;
			this.sortOrder = sortOrder;
			HashedId = ID;
			HashedIdShiny = ID + "_Shiny";
			BorderTag = ID;
		}

		private TextureAtlas CreateAtlas(TextureAtlas original, Texture2D texture)
		{
			var atlas = ScriptableObject.CreateInstance<TextureAtlas>();
			atlas.texture = texture;
			atlas.scaleFactor = original.scaleFactor;
			atlas.items = original.items;

			return atlas;
		}

		public BackwallPattern(BuildingDef def)
		{
			ID = def.PrefabID;
			Log.Debug("registered wall with ID " + ID);
			atlas = def.BlockTileAtlas;

			if (def.BlockTileShineAtlas != null)
			{
				specularTexture = def.BlockTileShineAtlas.texture;
				specularColor = def.BlockTileMaterial.GetColor("_ShineColour");
			}

			UISprite = def.GetUISprite();
			sortOrder = GetSortOrder(def);
			name = def.Name;

			if (Mod.isNoZoneTintHere)
			{
				biomeTint = 0;
			}

			BorderTag = ID;
			HashedId = ID;
			HashedIdShiny = ID + "_Shiny";
		}

		private int GetSortOrder(BuildingDef def)
		{
			if (def.PrefabID == TileConfig.ID)
				return -1;

			if (def.BuildingComplete.HasTag(FTags.stainedGlass))
				return 1;

			return 0;
		}
	}
}
