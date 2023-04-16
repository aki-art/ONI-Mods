using FUtility;
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
        public int sortOrder;
        public readonly string ID;
        private static TextureAtlas defaultAtlas;
        public string name;
        public float biomeTint = 0.2f;

        public int CompareTo(BackwallPattern other)
        {
            return sortOrder.CompareTo(other.sortOrder);
        }

        public BackwallPattern(string ID, string name, Texture2D texture, Sprite UISprite, int sortOrder)
        {
            if (defaultAtlas == null)
            {
                defaultAtlas = Assets.GetTextureAtlas("tiles_solid");
            }

            Log.Debuglog("registered wall with ID " + ID);

            atlas = CreateAtlas(defaultAtlas, texture);
            this.name = name;
            this.UISprite = UISprite;
            this.ID = ID;
            this.sortOrder = sortOrder;
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
            Log.Debuglog("registered wall with ID " + ID);
            atlas = def.BlockTileAtlas;

            if(def.BlockTileShineAtlas != null)
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
        }

        private int GetSortOrder(BuildingDef def)
        {
            if (def.PrefabID == TileConfig.ID)
            {
                return -1;
            }

            if (def.BuildingComplete.HasTag("DecorPackA_StainedGlass"))
            {
                return 1;
            }

            return 0;
        }
    }
}
