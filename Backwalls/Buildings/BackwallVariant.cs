using System;
using UnityEngine;

namespace Backwalls.Buildings
{
    public class BackwallPattern : IComparable<BackwallPattern>
    {
        public TextureAtlas atlas;
        public Material material;
        public Sprite UISprite;
        public int sortOrder;
        public readonly string ID;
        private static Material defaultMaterial;
        private static TextureAtlas defaultAtlas;
        public string name;

        public static void InitDefaultMaterial()
        {
            defaultMaterial = new Material(Shader.Find("TextMeshPro/Sprite"));
            defaultAtlas = Assets.GetTextureAtlas("tiles_solid");
        }

        public int CompareTo(BackwallPattern other)
        {
            return sortOrder.CompareTo(other.sortOrder);
        }

        public BackwallPattern(string ID, string name, Texture2D texture, Sprite UISprite, int sortOrder, Material material = null)
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

        public BackwallPattern(BuildingDef def)
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
