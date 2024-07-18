using System.Collections.Generic;
using UnityEngine;

namespace TrueTiles.Cmps
{
	public class TileAssets : KMonoBehaviour
	{
		public static TileAssets Instance { get; private set; }

		private Dictionary<string, Dictionary<SimHashes, TextureAsset>> textureAssets;

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			Instance = this;
			textureAssets = new Dictionary<string, Dictionary<SimHashes, TextureAsset>>();
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

		public void UnloadTextures()
		{
			foreach (var asset in textureAssets)
			{
				foreach (var tex in asset.Value.Values)
				{
					tex.top = null;
					tex.normalMap = null;
					tex.main = null;
					tex.specular = null;
					tex.topSpecular = null;
				}
			}
		}

		public TextureAsset Get(string def, SimHashes material)
		{
			if (textureAssets != null && textureAssets.TryGetValue(def, out var assets) && assets.TryGetValue(material, out var asset))
				return asset;

			return null;
		}

		public void Add(string def, SimHashes material, TextureAsset asset)
		{
			if (!textureAssets.ContainsKey(def))
				textureAssets.Add(def, new Dictionary<SimHashes, TextureAsset>());

			textureAssets[def][material] = asset;
		}

		public bool ContainsDef(string prefabID)
		{
			return textureAssets.ContainsKey(prefabID);
		}

		public override void OnCleanUp()
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
