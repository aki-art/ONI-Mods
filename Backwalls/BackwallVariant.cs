using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Backwalls
{
    public class BackwallVariant : IComparable<BackwallVariant>
    {
        public TextureAtlas atlas;
        public Material material;
        public Sprite UISprite;
        public int sortOrder;
        public readonly string ID;
        private static Material defaultMaterial;
        private static TextureAtlas defaultAtlas;
        public string name;
        //public Texture2D texture;

        public static void InitDefaultMaterial()
        {
            defaultMaterial = new Material(Shader.Find("TextMeshPro/Sprite"));
            defaultAtlas = Assets.GetTextureAtlas("tiles_solid");
        }

        public int CompareTo(BackwallVariant other)
        {
            return sortOrder.CompareTo(other.sortOrder);
        }

        public BackwallVariant(string ID, string name, Texture2D texture, Sprite UISprite, int sortOrder, Material material = null)
        {
            atlas = CreateAtlas(defaultAtlas, texture);
            this.name = name;
            this.UISprite = UISprite;
            this.material = defaultMaterial;
            this.ID = ID;
            this.sortOrder = sortOrder;
            //this.texture = texture;
        }

        private TextureAtlas CreateAtlas(TextureAtlas original, Texture2D texture)
        {
            var atlas = ScriptableObject.CreateInstance<TextureAtlas>();
            atlas.texture = texture;
            atlas.scaleFactor = original.scaleFactor;
            atlas.items = original.items;

            return atlas;
        }

        public BackwallVariant(BuildingDef def)
        {
            ID = def.PrefabID;
            atlas = def.BlockTileAtlas;
            UISprite = def.GetUISprite();
            material = defaultMaterial;
            sortOrder = GetSortOrder(def);
            name = def.Name;
            //texture = def.BlockTileAtlas.texture;
        }

        private int GetSortOrder(BuildingDef def)
        {
            if(def.PrefabID == TileConfig.ID)
            {
                return -1;
            }

            if(def.BuildingComplete.HasTag("DecorPackA_StainedGlass"))
            {
                return 1;
            }

            return 0;
        }
    }
}
