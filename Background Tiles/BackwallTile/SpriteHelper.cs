using FUtility;
using System;
using UnityEngine;

namespace BackgroundTiles.BackwallTile
{
    public class SpriteHelper
    {
        public static Sprite GetSpriteForDef(BuildingDef def)
        {
            Texture2D cropped = GetTexture2(def);

            int size = 192;
            return Sprite.Create(cropped, new Rect(0, 0, cropped.width, cropped.height), Vector3.zero, 64, 0, SpriteMeshType.Tight, new Vector4(0, 0, size, size));
        }

        public static void ConfigureAnim(BuildingDef def, TextureAtlas originalTA)
        {
            Log.Debuglog("Configuring anim of " + def.Tag);
            
            // Copy animation
            KAnimFile anim = UnityEngine.Object.Instantiate(def.AnimFiles[0]);

            anim.name = def.Tag.ToString().ToLowerInvariant() + "_" + anim.name;
            if (anim is null)
            {
                Log.Warning($"Building {def.Tag} has no kanim associated.");
                return;
            }

            foreach (var texture in anim.textureList)
            {
                Log.Debuglog(texture.name);
            }

            //def.AnimFiles[0] = ModUtil.AddKAnim(def.Tag.ToString().ToLowerInvariant() + "_" + anim.name, anim.animBytes, anim.buildBytes, anim.textureList);
        }


        public static Texture2D GetTexture2(BuildingDef def)
        {
            float multiplier = 1.4f;

            float ymultiplier = 0.2f;
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

            Texture2D tex = def.BlockTileAtlas.texture;

            Texture2D texture2D = new Texture2D(tex.width, tex.height, TextureFormat.RGBA32, false);

            RenderTexture renderTexture = new RenderTexture(tex.width, tex.height, 32);
            Graphics.Blit(tex, renderTexture);

            texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            texture2D.Apply();

            int xo = (int)(uvBox.x * tw);
            int yo = (int)(uvBox.w * th);

            int size = (int)(uvBox.z * tw - xo);
            int xe = (int)(size / multiplier);
            int ye = (int)(size - (ymultiplier * xe));

            Texture2D cropped = new Texture2D(xe, ye, TextureFormat.RGBA32, true);

            for (int x = 0; x < xe; x++)
            {
                for (int y = 0; y < ye; y++)
                {
                    cropped.SetPixel(x, y, texture2D.GetPixel(
                        (int)(multiplier * x) + xo,
                        (int)(x * ymultiplier) + y + yo));
                }
            }

            cropped.Apply();

            return cropped;
        }
    }
}
