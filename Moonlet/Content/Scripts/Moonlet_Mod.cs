using KSerialization;
using ProcGenGame;
using System;
using static ProcGen.SubWorld;
using System.Collections.Generic;

namespace Moonlet.Content.Scripts
{
	public class Moonlet_Mod : KMonoBehaviour
	{
		public static Moonlet_Mod Instance;

		public static string loadedClusterId;

		[Serialize] public Dictionary<int, ZoneType> zoneTypeOverrides = new();

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			Instance = this;
		}

		public override void OnSpawn()
		{
			base.OnSpawn();
			RegenerateBackwallTexture();
			World.Instance.zoneRenderData.OnActiveWorldChanged();
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			Instance = null;
		}

		public void WorldLoaded(Cluster cluster)
		{
			if (cluster == null)
			{
				loadedClusterId = null;
				return;
			}

			loadedClusterId = cluster.Id;
		}

		public void RegenerateBackwallTexture()
		{
			if (World.Instance.zoneRenderData == null)
			{
				Debug.Log("Subworld zone render data is not yet initialized.");
				return;
			}

			var zoneRenderData = World.Instance.zoneRenderData;

			var zoneIndices = zoneRenderData.colourTex.GetRawTextureData();
			var colors = zoneRenderData.indexTex.GetRawTextureData();

			foreach (var tile in zoneTypeOverrides)
			{
				var cell = tile.Key;
				var zoneType = (byte)tile.Value;

				var color = World.Instance.zoneRenderData.zoneColours[zoneType];
				colors[cell] = (tile.Value == ZoneType.Space) ? byte.MaxValue : zoneType;

				zoneIndices[cell * 3] = color.r;
				zoneIndices[cell * 3 + 1] = color.g;
				zoneIndices[cell * 3 + 2] = color.b;

				World.Instance.zoneRenderData.worldZoneTypes[cell] = tile.Value;
			}

			zoneRenderData.colourTex.LoadRawTextureData(zoneIndices);
			zoneRenderData.indexTex.LoadRawTextureData(colors);
			zoneRenderData.colourTex.Apply();
			zoneRenderData.indexTex.Apply();

			zoneRenderData.OnShadersReloaded();
		}

	}
}
