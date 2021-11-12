using FUtility;
using System;
using UnityEngine;
using static Rendering.BlockTileRenderer;

namespace BackgroundTiles.BackwallTile
{
    public class SpriteHelper
    {
        // TODO: atlas stitching
        public static Sprite GetSpriteForDef(BuildingDef def)
        {
            Texture2D cropped = GetTexture(def);

            return Sprite.Create(cropped, new Rect(0, 0, cropped.width, cropped.height), Vector3.zero, 64, 0, SpriteMeshType.Tight); //, new Vector4(0, 0, 100, 100));
        }

        public static Texture2D GetTexture(BuildingDef def)
        {
            int num3 = def.BlockTileAtlas.items[0].name.Length - 4 - 8;
            int startIndex = num3 - 1 - 8;

            Vector4 uvBox = Vector4.zero;

            for (int k = 0; k < def.BlockTileAtlas.items.Length; k++)
            {
                TextureAtlas.Item item = def.BlockTileAtlas.items[k];

                string value = item.name.Substring(startIndex, 8);
                int requiredConnections = Convert.ToInt32(value, 2);

                if (requiredConnections == 0)
                {
                    uvBox = item.uvBox;
                    break;
                }
            }

            int tw = def.BlockTileAtlas.texture.width;
            int th = def.BlockTileAtlas.texture.height;

            /*
            int x = Mathf.FloorToInt(uvBox.x * tw);
            int y = Mathf.FloorToInt(uvBox.y * th);
            int w = Mathf.FloorToInt(uvBox.z * tw - x);
            int h = Mathf.FloorToInt(uvBox.w * th - y);*/

            int x = Mathf.FloorToInt(uvBox.x * tw);
            int y = Mathf.FloorToInt(uvBox.w * th);
            int w = Mathf.FloorToInt(uvBox.z * tw - x);
            int h = Mathf.FloorToInt(uvBox.y * th - y);

            Log.Debuglog("tw:", tw, "th:", th); // tw:, 1024, th:, 1024
            Log.Debuglog("x:", x, "y:", y, "w:", w, "h:", h); // x:, 8, y:, 824, w:, 192, h:, 192

            Texture2D readable = new Texture2D(tw, th, TextureFormat.RGBA32, false);
            Graphics.ConvertTexture(def.BlockTileAtlas.texture, readable);

            Texture2D test = new Texture2D(tw, th, TextureFormat.RGBA32, true);
            test.SetPixels(readable.GetPixels());
            test.Apply(true, true);

            Log.Assert("test", test);
            Log.Debuglog(test.width, test.height);

            return test;
        }
    }
}
