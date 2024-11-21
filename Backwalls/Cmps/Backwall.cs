using Backwalls.Buildings;
using KSerialization;
using System;
using UnityEngine;

namespace Backwalls.Cmps
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class Backwall : KMonoBehaviour
	{
		[Serialize] public BackwallSettings settings;
		[Serialize] public bool initializedColor; // dumb field name as it has more responsibility than color, but keeping because of serialization name
		[Serialize] public bool migrated;

		public bool copiedColor;
		public bool constructed;

		[Obsolete][Serialize] private string colorHex;
		[Obsolete][Serialize] private string pattern;
		[Obsolete][Serialize] private int swatchIdx;

		public Color color;

		private BackwallPattern currentVariant;

		private static BackwallSettings DefaultSettings => new()
		{
			pattern = Mod.Settings.DefaultPattern,
			borderTag = Mod.Settings.DefaultPattern,
			colorHex = Mod.Settings.DefaultColor,
			swatchIdx = -1,
			//shiny = true,
			showBorders = true
		};

		[Serializable]
		public struct BackwallSettings : IEquatable<BackwallSettings>
		{
			public string colorHex;
			public string pattern;
			public string borderTag;
			public int swatchIdx;
			//public bool shiny;
			public bool showBorders;

			public readonly bool Connects(BackwallSettings other)
			{
				if (borderTag == other.borderTag)
				{
					return colorHex == other.colorHex && swatchIdx == other.swatchIdx;
				}

				return colorHex == other.colorHex
				&& pattern == other.pattern
				&& swatchIdx == other.swatchIdx;
			}

			public readonly bool Equals(BackwallSettings other) =>
				colorHex == other.colorHex
				&& pattern == other.pattern
				&& swatchIdx == other.swatchIdx;
		}

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			Subscribe((int)GameHashes.CopySettings, OnCopySettings);
			Subscribe((int)GameHashes.NewConstruction, OnNewConstruction);
		}

		private void OnNewConstruction(object data)
		{
			if (data is Constructable constructable
				&& constructable.TryGetComponent(out BackwallUnderConstruction backwallUnderConstruction))
			{
				constructed = true;

				if (backwallUnderConstruction.hasCopyData)
					CopySettingsFrom(backwallUnderConstruction.settings);
			}
		}

		private void CopySettingsFrom(BackwallSettings settings)
		{
			if (!settings.pattern.IsNullOrWhiteSpace())
				TrySetPattern(settings.pattern);

			if (settings.swatchIdx != -1)
				SetColor(settings.swatchIdx);
			else if (!settings.colorHex.IsNullOrWhiteSpace())
				SetColor(settings.colorHex);

			copiedColor = true;
		}

		public override void OnSpawn()
		{
			base.OnSpawn();

			var instantBuildMode = DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive && SandboxToolParameterMenu.instance.settings.InstantBuild;

			if (instantBuildMode && !constructed && Backwalls_Mod.Instance.hasCopyOverride)
				CopySettingsFrom(Backwalls_Mod.Instance.copySettings);

			if (copiedColor)
			{
				initializedColor = true;
				migrated = true;
			}
			else if (!initializedColor)
			{
				settings = DefaultSettings;
			}
			else if (!migrated && !pattern.IsNullOrWhiteSpace())
			{
				settings = new BackwallSettings()
				{
					colorHex = colorHex,
					pattern = pattern,
					swatchIdx = swatchIdx,
					//shiny = true,
					showBorders = true
				};
			}

			if (Mod.variants == null || Mod.variants.Count == 0)
			{
				Log.Warning("No backwall variants are registered");
				return;
			}

			settings.pattern ??= Mod.Settings.DefaultPattern;

			if (!TrySetPattern(settings.pattern))
				if (!TrySetPattern(Mod.Settings.DefaultPattern))
					TrySetPattern("BlankPattern");

			if (settings.swatchIdx > -1)
				SetColor(settings.swatchIdx);
			else
				SetColor(settings.colorHex);

			/*			if(!settings.shiny && Mod.Settings.EnableShinyTilesGlobal)
							UpdateShiny();*/

			initializedColor = true;
			migrated = true;
		}

		public bool TrySetPattern(string patternId)
		{
			if (Mod.variants.TryGetValue(patternId, out var backwallPattern))
			{
				SetPattern(backwallPattern);
				return true;
			}

			Log.Debug($"could not set pattern {patternId}");
			return false;
		}

		// attempt at restoring planned backwalls with their settings intact
		/*
        private void BlueprintsIntegration()
        {
            var cell = this.NaturalBuildingCell();

            Log.Assert("backwallstorage", BackwallStorage.Instance);
            if (BackwallStorage.Instance.data != null && BackwallStorage.Instance.data.TryGetValue(cell, out var data))
            {
                if (data != null)
                {
                    pattern = data.Pattern;
                    colorHex = data.ColorHex;
                }

                BackwallStorage.Instance.data.Remove(cell);
            }
        }
        */

		public bool Matches(Backwall other)
		{
			return other != null && other.settings.Connects(settings);
		}

		public void SetPattern(BackwallPattern pattern)
		{
			if (pattern == null || pattern.atlas == null)
			{
				return;
			}

			var cell = Grid.PosToCell(this);

			if (currentVariant != null)
			{
				Mod.renderer.RemoveBlock(currentVariant, cell);
			}

			settings.pattern = pattern.ID;
			settings.borderTag = pattern.BorderTag;
			currentVariant = pattern;

			//var shiny = !Mod.renderer.shinyDisabled.Contains(cell);
			Mod.renderer.AddBlock((int)Grid.SceneLayer.Backwall, currentVariant, cell, true);
		}

		public void SetColor(int swatchIndex)
		{
			var color = ModAssets.colors[swatchIndex];
			SetColor(color, swatchIndex);
		}

		public void SetColor(Color color, int index = -1)
		{
			var cell = Grid.PosToCell(this);

			Mod.renderer.colorInfos[cell] = color;
			Mod.renderer.Rebuild(cell);

			settings.colorHex = color.ToHexString();
			settings.swatchIdx = index;
		}

		public void SetColor(string hex)
		{
			Log.Debug("setting color to " + hex);

			if (hex.IsNullOrWhiteSpace())
			{
				Log.Warning("Invalid color");
				hex = "FFFFFF";
			}

			var cell = Grid.PosToCell(this);

			var color = Util.ColorFromHex(hex);

			Mod.renderer.colorInfos[cell] = color;
			Mod.renderer.Rebuild(cell);

			// make it full length, or the equality comparison may not work well
			if (hex.Length == 6)
			{
				hex += "FF";
			}

			settings.colorHex = hex;
			settings.swatchIdx = -1;
		}

		private void OnCopySettings(object obj)
		{
			if (((GameObject)obj).TryGetComponent(out Backwall wall))
			{
				if (Backwalls_Mod.Instance.CopyPattern)
					SetPattern(wall.currentVariant);

				if (Backwalls_Mod.Instance.CopyColor)
				{
					if (wall.settings.swatchIdx == -1)
						SetColor(wall.settings.colorHex);
					else
						SetColor(wall.settings.swatchIdx);

					//SetShiny(wall.settings.shiny);
				}
			}
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();

			var cell = Grid.PosToCell(this);

			Mod.renderer.colorInfos[cell] = Color.white;
			//Mod.renderer.shinyDisabled.Remove(cell);
			Mod.renderer.RemoveBlock(currentVariant, cell);
			Mod.renderer.Rebuild(cell);
		}

		/*		public void SetShiny(bool isShiny)
				{
					settings.shiny = isShiny;
					UpdateShiny();
				}

				public void UpdateShiny()
				{
					var isMatt = currentVariant.specularTexture != null;

					if (isMatt)
						return;

					var cell = Grid.PosToCell(this);

					if (settings.shiny)
						Mod.renderer.shinyDisabled.Add(cell);
					else
						Mod.renderer.shinyDisabled.Remove(cell);

					Mod.renderer.RemoveBlock(currentVariant, cell);
					Mod.renderer.AddBlock((int)Grid.SceneLayer.Backwall, currentVariant, cell, settings.shiny);
				}

				public bool CanBeShiny() => currentVariant.specularTexture != null;*/
	}
}
