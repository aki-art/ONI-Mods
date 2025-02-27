using KSerialization;
using Moonlet.TemplateLoaders;
using Moonlet.Templates;
using Moonlet.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ProcGen.SubWorld;

namespace Moonlet.Scripts
{
	[DefaultExecutionOrder(10)]
	[SerializationConfig(MemberSerialization.OptIn)]
	public class Moonlet_Mod : KMonoBehaviour, IRender200ms
	{
		public static Moonlet_Mod Instance { get; private set; }

		public static Dictionary<SimHashes, ElementTemplate.EffectsEntry> stepOnEffects;
		public static Dictionary<SimHashes, ElementTemplate.EffectsEntry> stepOnGasEffects;
		public static Dictionary<int, List<ZoneTile>> zoneTiles = [];

		[Serialize] public Dictionary<int, ZoneType> zoneTypeOverrides = [];
		[Serialize] public Dictionary<int, ZoneType> pendingZoneTypeOverrides = [];
		[Serialize] public Dictionary<int, string> cachedZoneTypesIndices;

		public static Dictionary<Vector2I, TemplateContainer> worldgenZoneTypeOverrides;

		private bool zoneTypesDirty;

		public override void OnSpawn()
		{
			base.OnSpawn();
			StartCoroutine(UpdateZoneTypes());
		}

		public IEnumerator UpdateZoneTypes()
		{
			yield return SequenceUtil.waitForEndOfFrame;

			pendingZoneTypeOverrides = new(zoneTypeOverrides);

			if (worldgenZoneTypeOverrides != null)
			{
				foreach (var zoneTypeOverride in worldgenZoneTypeOverrides)
					ApplyZoneTypeOverrides(zoneTypeOverride.Value, zoneTypeOverride.Key);
			}

			zoneTypesDirty = zoneTypeOverrides.Count > 0 || pendingZoneTypeOverrides.Count > 0;
		}

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			Instance = this;
		}

		public void ApplyZoneTypeOverrides(TemplateContainer template, Vector2 rootLocation)
		{
			if (template.info?.tags == null)
				return;

			foreach (var tag in template.info.tags)
			{
				if (tag.name.StartsWith(MTemplateLoader.LOOKUP_PREFIX))
				{
					var templateId = tag.name.Substring(MTemplateLoader.LOOKUP_PREFIX.Length);

					if (Mod.templatesLoader.TryGet(templateId, out var loader))
					{
						if (loader.zoneTypeOverrides != null)
						{
							foreach (var zoneOverride in loader.zoneTypeOverrides)
							{
								var cell = Grid.PosToCell(new Vector2(zoneOverride.x + rootLocation.x, zoneOverride.y + rootLocation.y));

								AddZoneTypeOverride(cell, zoneOverride.zone);
							}
						}
					}

					return;
				}
			}
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
				Log.Warn("Subworld zone render data is not yet initialized.");
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

				var index = (tile.Value == ZoneType.Space)
					? byte.MaxValue
					: (byte)World.Instance.zoneRenderData.zoneTextureArrayIndices[zoneType];

				indexData[cell] = index;

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

		public void AddZoneTile(ZoneTile zoneTile)
		{
			foreach (var cell in zoneTile.building.PlacementCells)
			{
				if (!zoneTiles.ContainsKey(cell))
					zoneTiles[cell] = [zoneTile];
				else
					zoneTiles[cell].Add(zoneTile);
			}
		}

		public void RemoveZoneTile(ZoneTile zoneTile)
		{
			foreach (var cell in zoneTile.building.PlacementCells)
			{
				zoneTiles[cell].Remove(zoneTile);
				if (zoneTiles[cell].Count == 0)
					zoneTiles.Remove(cell);
			}
		}

		// run even when paused
		public void Render200ms(float dt)
		{
			if (zoneTypesDirty)
			{
				foreach (var pending in pendingZoneTypeOverrides)
				{
					if (!zoneTiles.ContainsKey(pending.Key))
						SimMessages.ModifyCellWorldZone(pending.Key, pending.Value == ZoneType.Space ? byte.MaxValue : (byte)pending.Value);
				}

				RegenerateBackwallTexture(pendingZoneTypeOverrides);

				zoneTypeOverrides.MergeRange(pendingZoneTypeOverrides);
				pendingZoneTypeOverrides.Clear();

				World.Instance.zoneRenderData.OnActiveWorldChanged();

				zoneTypesDirty = false;
			}
		}
	}
}
