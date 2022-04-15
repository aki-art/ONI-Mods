using System.Collections.Generic;
using UnityEngine;

namespace TrueTiles.Cmps
{
    public class TileAssetsDict
    {
        private Dictionary<string, Dictionary<SimHashes, TextureAsset>> textureAssets;

        public TileAssetsDict()
        {
            textureAssets = new Dictionary<string, Dictionary<SimHashes, TextureAsset>>();
        }

        public bool TryGet(string def, SimHashes element, out TextureAsset asset)
        {
            if(textureAssets.TryGetValue(def, out var elementVariants))
            {
                return elementVariants.TryGetValue(element, out asset);
            }

            asset = null;
            return false;
        }

        public void Add(string def, SimHashes element, TextureAsset asset)
        {
            if (!textureAssets.ContainsKey(def))
            {
                textureAssets.Add(def, new Dictionary<SimHashes, TextureAsset>());
            }

            textureAssets[def][element] = asset;
        }

        public void Clear()
        {
            foreach (var asset in textureAssets.Values)
            {
                foreach (var item in asset.Values)
                {
                    item.top = null;
                    item.normalMap = null;
                    item.topSpecular = null;
                    item.main = null;
                    item.specular = null;
                }
            }

            textureAssets.Clear();
        }

        public void Merge(TileAssetsDict other)
        {
            foreach (var buildingID in other.textureAssets)
            {
                if (textureAssets.ContainsKey(buildingID.Key))
                {
                    foreach (var elementVariant in buildingID.Value)
                    {
                        var firstBuildingEntry = textureAssets[buildingID.Key];

                        if (firstBuildingEntry.ContainsKey(elementVariant.Key))
                        {
                            firstBuildingEntry[elementVariant.Key] = elementVariant.Value;
                        }
                        else
                        {
                            firstBuildingEntry.Add(elementVariant.Key, elementVariant.Value);
                        }
                    }
                }
                else
                {
                    textureAssets.Add(buildingID.Key, buildingID.Value);
                }
            }
        }

        public class TextureAsset
        {
            public Texture2D main;
            public Texture2D specular;
            public Color specularColor = Color.white;
            public Texture2D top;
            public Texture2D topSpecular;
            public Color topSpecularColor = Color.white;
            public Texture2D normalMap;
            public float specularFrequency = 1f;
        }
    }
}
