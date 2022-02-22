using FUtility;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace TrueTiles
{
    public class TileAssets : KMonoBehaviour
    {
        public static TileAssets Instance { get; private set; }

        private Dictionary<string, Dictionary<SimHashes, TextureAsset>> textureAssets;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Instance = this;
            textureAssets = new Dictionary<string,Dictionary<SimHashes, TextureAsset>>();
        }

        public TextureAsset Get(string def, SimHashes material)
        {
            if(textureAssets != null && textureAssets.TryGetValue(def, out Dictionary<SimHashes,TextureAsset> assets))
            {
                if(assets.TryGetValue(material, out TextureAsset asset))
                {
                    return asset;
                }
            }

            return null;
        }

        public void Add(string def, SimHashes material, TextureAsset asset)
        {
            if (!textureAssets.ContainsKey(def))
            {
                textureAssets.Add(def, new Dictionary<SimHashes, TextureAsset>());
            }

            textureAssets[def][material] = asset;
        }

        public bool ContainsDef(string prefabID)
        {
            return textureAssets.ContainsKey(prefabID);
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();
            textureAssets = null;
            Instance = null;
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
