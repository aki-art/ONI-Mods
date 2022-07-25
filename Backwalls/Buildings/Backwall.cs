using Backwalls.Integration.Blueprints;
using FUtility;
using KSerialization;
using UnityEngine;

namespace Backwalls.Buildings
{
    public class Backwall : KMonoBehaviour
    {
        [Serialize]
        public int colorIdx;

        [Serialize]
        public string pattern;

        private BackwallPattern currentVariant;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Subscribe((int)GameHashes.CopySettings, OnCopySettings);
        }

        public Backwall()
        {
            pattern = Mod.Settings.DefaultPattern;
            colorIdx = Mod.Settings.DefaultColorIdx;
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
            SetColor(colorIdx);

            GetComponent<KSelectable>().SetName(this.NaturalBuildingCell().ToString());
        }

        private void BlueprintsIntegration()
        {
            var cell = this.NaturalBuildingCell();

            if (BluePrintsPatch.wallDataCache.TryGetValue(cell, out var data))
            {
                pattern = data.Pattern;
                colorIdx = data.ColorIdx;
                BluePrintsPatch.wallDataCache[cell] = null;
            }
        }

        public bool Matches(Backwall other)
        {
            return other != null && other.colorIdx == colorIdx && other.pattern == pattern;
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

        public void SetColor(int colorIdx)
        {
            var cell = Grid.PosToCell(this);

            colorIdx = Mathf.Clamp(colorIdx, 0, ModAssets.colors.Length - 1);

            Mod.renderer.colorInfos[cell] = ModAssets.colors[colorIdx];
            Mod.renderer.Rebuild(cell);

            this.colorIdx = colorIdx;
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
                    SetColor(wall.colorIdx);
                }
            }
        }
    }
}
