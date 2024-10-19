using KSerialization;
using MonoMod.Utils;
using Moonlet.Templates;
using System.Collections.Generic;
using UnityEngine;
using static ProcGen.SubWorld;

namespace Moonlet.Scripts
{
	[DefaultExecutionOrder(10)]
	[SerializationConfig(MemberSerialization.OptIn)]
	public class Moonlet_Mod : KMonoBehaviour, ISim200ms, IRender200ms
	{
		public static Moonlet_Mod Instance;

		public static Dictionary<SimHashes, ElementTemplate.EffectsEntry> stepOnEffects;

		[Serialize] public Dictionary<int, ZoneType> zoneTypeOverrides = [];
		[Serialize] public Dictionary<int, ZoneType> pendingZoneTypeOverrides = [];
		[Serialize] public Dictionary<int, string> cachedZoneTypesIndices;

		private bool zoneTypesDirty;

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			Instance = this;
		}

		public void AddZoneTypeOverride(int cell, ZoneType zoneType)
		{
			pendingZoneTypeOverrides[cell] = zoneType;
			zoneTypesDirty = true;
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			Instance = null;
		}

		public void RegenerateBackwallTexture() => RegenerateBackwallTexture(zoneTypeOverrides);

		public void RegenerateBackwallTexture(Dictionary<int, ZoneType> overrides)
		{
			if (World.Instance.zoneRenderData == null)
			{
				Debug.Log("Subworld zone render data is not yet initialized.");
				return;
			}

			var zoneRenderData = World.Instance.zoneRenderData;

			var colorData = zoneRenderData.colourTex.GetRawTextureData();
			var indexData = zoneRenderData.indexTex.GetRawTextureData();

			foreach (var tile in overrides)
			{
				var cell = tile.Key;
				var zoneType = (byte)tile.Value;

				var color = World.Instance.zoneRenderData.zoneColours[zoneType];
				indexData[cell] = (tile.Value == ZoneType.Space) ? byte.MaxValue : zoneType;

				colorData[cell * 3] = color.r;
				colorData[cell * 3 + 1] = color.g;
				colorData[cell * 3 + 2] = color.b;

				World.Instance.zoneRenderData.worldZoneTypes[cell] = tile.Value;
			}

			zoneRenderData.colourTex.LoadRawTextureData(colorData);
			zoneRenderData.indexTex.LoadRawTextureData(indexData);
			zoneRenderData.colourTex.Apply();
			zoneRenderData.indexTex.Apply();

			zoneRenderData.OnShadersReloaded();
		}

		public void Sim200ms(float dt)
		{
		}

		// run even when paused
		public void Render200ms(float dt)
		{
			if (zoneTypesDirty)
			{
				RegenerateBackwallTexture(pendingZoneTypeOverrides);
				zoneTypeOverrides.AddRange(pendingZoneTypeOverrides);
				pendingZoneTypeOverrides.Clear();
				World.Instance.zoneRenderData.OnActiveWorldChanged();

				zoneTypesDirty = false;
			}
		}
	}
}
