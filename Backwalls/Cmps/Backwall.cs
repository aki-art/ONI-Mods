﻿using Backwalls.Buildings;
using FUtility;
using KSerialization;
using UnityEngine;

namespace Backwalls.Cmps
{
    public class Backwall : KMonoBehaviour
    {
        [Serialize]
        public string colorHex;

        [Serialize]
        public string pattern;

        [Serialize]
        public int swatchIdx; // used for the swatch selector

        public Color color;

        private BackwallPattern currentVariant;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Subscribe((int)GameHashes.CopySettings, OnCopySettings);
        }

        public Backwall()
        {
            pattern = Mod.Settings.DefaultPattern;
            colorHex = Mod.Settings.DefaultColor;
            swatchIdx = -1;
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            BlueprintsIntegration();

            if (pattern.IsNullOrWhiteSpace())
            {
                pattern = Mod.Settings.DefaultPattern;
            }

            var backwallPattern = Mod.variants.Find(v => v.ID == pattern);

            if (backwallPattern == null)
            {
                Log.Warning("no pattern with ID " + Mod.Settings.DefaultPattern);
                Mod.variants.Find(v => v.ID == Mod.Settings.DefaultPattern);
            }

            SetPattern(backwallPattern);

            if (swatchIdx > -1)
            {
                SetColor(swatchIdx);
            }
            else
            {
                SetColor(colorHex);
            }
        }

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

        public bool Matches(Backwall other)
        {
            return other != null && other.colorHex == colorHex && other.pattern == pattern;
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

            this.pattern = pattern.ID;
            currentVariant = pattern;
            Mod.renderer.AddBlock((int)Grid.SceneLayer.Backwall, currentVariant, cell);
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

            colorHex = color.ToHexString();
            swatchIdx = index;
        }

        public void SetColor(string hex)
        {
            var cell = Grid.PosToCell(this);

            var color = Util.ColorFromHex(hex);

            Mod.renderer.colorInfos[cell] = color;
            Mod.renderer.Rebuild(cell);

            colorHex = hex;
        }

        private void OnCopySettings(object obj)
        {
            if (((GameObject)obj).TryGetComponent(out Backwall wall))
            {
                if (Mod.Settings.CopyPattern)
                {
                    SetPattern(wall.currentVariant);
                }
                if (Mod.Settings.CopyColor)
                {
                    SetColor(wall.colorHex);
                }
            }
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();

            var cell = Grid.PosToCell(this);

            Mod.renderer.colorInfos[cell] = Color.white;
            Mod.renderer.RemoveBlock(currentVariant, cell);
            Mod.renderer.Rebuild(cell);
        }
    }
}
