/*using KSerialization;
using Moonlet.TemplateLoaders;
using Moonlet.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static ProcGen.SubWorld;

namespace Moonlet.Scripts
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class Moonlet_ZoneTypeTracker : KMonoBehaviour, IRender200ms
	{
		[Serialize] public List<string> zoneTypeCache;
		[Serialize] public Dictionary<int, ZoneType> zoneTypeOverrides = [];
		[Serialize] public Dictionary<int, ZoneType> pendingZoneTypeOverrides = [];

		public static Dictionary<ZoneType, int> textureArrayIndices = [];

		private bool zoneTypesDirty;
		private Dictionary<ZoneType, ZoneType> migrateZones;

		public static Dictionary<Vector2I, TemplateContainer> worldgenZoneTypeOverrides;

		public static Moonlet_ZoneTypeTracker Instance { get; private set; }

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			Instance = this;
		}

		public override void OnSpawn()
		{
			InitializeCache();
			StartCoroutine(UpdateZoneTypes());
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			Instance = null;
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

			ReapplyZones();

			World.Instance.zoneRenderData.GenerateTexture();
			World.Instance.zoneRenderData.OnShadersReloaded();

			Log.Debug("Zone Types:");
			for (int i = 0; i < zoneTypeCache.Count; i++)
			{
				string zoneCache = zoneTypeCache[i];
				Log.Debug($"\t{i} -  {zoneCache}");
			}

			zoneTypesDirty = true;
		}

		private bool IsVanillaZone(ZoneType zoneType)
		{
			var values = Enum.GetValues(typeof(ZoneType));
			foreach (var value in values)
			{
				if ((ZoneType)value == zoneType)
					return true;
			}

			return false;
		}

		private bool IsVanillaZone(string zoneType)
		{
			var values = Enum.GetNames(typeof(ZoneType));
			foreach (var value in values)
			{
				if (value == zoneType)
					return true;
			}

			return false;
		}

		private string GetZoneTypeName(ZoneType zoneType)
		{
			return ZoneTypeUtil.TryGetName(zoneType, out var name) ? name : zoneType.ToString();
		}

		public int AddOrGetIndex(ZoneType zoneType)
		{
			var name = GetZoneTypeName(zoneType);

			for (int i = 0; i < zoneTypeCache.Count; i++)
			{
				if (zoneTypeCache[i] == name)
					return i;
			}

			if (zoneTypeCache.Count > 255)
			{
				Log.Warn("Ran out of zone type space. There can not be more than 255 (21 vanilla + 234 modded) biome backgrounds.");
				return 0;
			}

			zoneTypeCache.Add(name);

			Log.Debug($"cached zonetype: {name} {zoneTypeCache.Count - 1}");

			return zoneTypeCache.Count - 1;
		}

		public void ApplyZoneTypeOverrides(TemplateContainer template, Vector2 rootLocation)
		{
			if (template.info?.tags == null)
				return;

			foreach (var tag in template.info.tags)
			{
				if (tag.name.StartsWith(MTemplateLoader.LOOKUP_PREFIX))
				{
					Log.Debug($"adding zone type overrides {rootLocation}");

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

		// run even when paused
		public void Render200ms(float dt)
		{
			if (zoneTypesDirty)
			{
				foreach (var pending in pendingZoneTypeOverrides)
				{
					SimMessages.ModifyCellWorldZone(pending.Key, pending.Value == ZoneType.Space ? byte.MaxValue : (byte)pending.Value);

					AddOrGetIndex(pending.Value);
				}

				RegenerateBackwallTexture(pendingZoneTypeOverrides);

				zoneTypeOverrides.MergeRange(pendingZoneTypeOverrides);
				pendingZoneTypeOverrides.Clear();

				World.Instance.zoneRenderData.OnActiveWorldChanged();

				zoneTypesDirty = false;
			}
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


		public void ReapplyZones()
		{
			migrateZones = [];

			for (int i = 0; i < zoneTypeCache.Count; i++)
			{
				var zoneName = zoneTypeCache[i];

				var currentZoneType = (ZoneType)i;

				if (IsVanillaZone(zoneName))
					continue;

				if (EnumUtils.TryParse(zoneName, out var expectedZoneType, ZoneTypeUtil.quickLookup))
				{
					if (expectedZoneType != currentZoneType)
					{
						migrateZones[currentZoneType] = expectedZoneType;
						Log.Info($"Zone Types have changed, remapping {currentZoneType} [{i}] to {expectedZoneType} [{(int)expectedZoneType}]");
					}
				}
				else
				{
					migrateZones[currentZoneType] = ZoneType.CrystalCaverns;
					Log.Info($"Missing zonetype {zoneName} [{i}], remapping to CrystalCaverns.");
				}

				UpdateZoneTextureIndices(i, expectedZoneType);
				*//*
								if (zoneRenderData.zoneColours.Length <= i)
									Array.Resize(ref zoneRenderData.zoneColours, i);*//*
			}

			if (migrateZones.Count == 0)
				return;

			foreach (var overworldCell in SaveLoader.Instance.clusterDetailSave.overworldCells)
			{
				if(migrateZones.TryGetValue(overworldCell.zoneType, out var zone)) 
					overworldCell.zoneType = zone;
			}

			var newOverrides = new Dictionary<int, ZoneType>();

			foreach (var zoneTypeOverride in zoneTypeOverrides)
			{
				if (migrateZones.TryGetValue(zoneTypeOverride.Value, out var zone))
					newOverrides[zoneTypeOverride.Key] = zone;
				else
					newOverrides[zoneTypeOverride.Key] = zoneTypeOverride.Value;
			}

			zoneTypeOverrides = newOverrides;


			var count = Mod.zoneTypesLoader.GetCount();

		}

		private static void UpdateZoneTextureIndices(int i, ZoneType expectedZoneType)
		{
			var zoneRenderData = World.Instance.zoneRenderData;

			if (textureArrayIndices.TryGetValue(expectedZoneType, out var index))
			{
				if (zoneRenderData.zoneTextureArrayIndices.Length <= i)
					Array.Resize(ref zoneRenderData.zoneTextureArrayIndices, i);

				Log.Debug($"set array index to currentZoneType: {expectedZoneType} -> {i}");
				zoneRenderData.zoneTextureArrayIndices[i] = index;
			}
		}

		private void InitializeCache()
		{
			if (zoneTypeCache != null)
				return;

			zoneTypeCache = [];

			var vanillaZones = Enum.GetValues(typeof(ZoneType)).Cast<ZoneType>().ToArray();

			int i;
			for (i = 0; i < vanillaZones.Length; i++)
				zoneTypeCache.Add(GetZoneTypeName(vanillaZones[i]));

			foreach (var overworldCell in SaveLoader.Instance.clusterDetailSave.overworldCells)
				AddOrGetIndex(overworldCell.zoneType);

			if (Moonlet_Mod.Instance.zoneTypeOverrides != null)
			{
				foreach (var zoneOverride in Moonlet_Mod.Instance.zoneTypeOverrides)
					AddOrGetIndex(zoneOverride.Value);

				Moonlet_Mod.Instance.zoneTypeOverrides = null;
			}
		}
	}
}
*/