using Bestagons.Content.Map;
using HarmonyLib;
using Klei;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static ProcGen.SubWorld;

namespace Bestagons.Content.ModDb
{
    public class HexTiles : ResourceSet<HexTile>
    {
        public HexTiles()
        {
            LoadFromYaml(Path.Combine(Utils.ModPath, "hexes"));
        }

        private void LoadFromYaml(string path)
        {
            if (!Directory.Exists(path))
            {
                Log.Warning("No hex files found, cannot load mod.");
                return;
            }

            var errors = ListPool<YamlIO.Error, HexTiles>.Allocate();

            foreach (var file in Directory.EnumerateFiles(path, "*.yaml"))
            {
                var yaml = Mod.TryRead(file);
                var hexes = YamlIO.LoadFile<HexTileCollection>(file, (error, _) => errors.Add(error));

                if (hexes == null)
                    continue;

                foreach (var hex in hexes.HexTiles)
                    TryAddHex(hex, null);
            }

            Global.Instance.modManager.HandleErrors(errors);
            errors.Recycle();
        }

        private void TryAddHex(HexTileCollection.Data hex, string[] additionalTags)
        {
            if (hex == null)
            {
                Log.Warning("hex null");
                return;
            }

            Log.Debug($"Loading {hex.Id}");

            if (hex.DlcIds != null && !DlcManager.IsDlcListValidForCurrentContent(hex.DlcIds))
                return;

            if (!AreRequiredModsActive(hex.RequiredModIds))
                return;

            var tags = new List<string>();

            if(hex.Tags != null)
            {
                Log.Debug("has tags: " + hex.Tags.Join());
                tags.AddRange(hex.Tags);
            }

            if (additionalTags != null)
                tags.AddRange(additionalTags);

            if (hex.Provides != null)
                tags.AddRange(hex.Provides);

            Add(new HexTile(
                hex.Id,
                hex.TemplateId,
                hex.Icon,
                hex.ZoneType.IsNullOrWhiteSpace() ? ZoneType.Sandstone : (ZoneType)Enum.Parse(typeof(ZoneType), hex.ZoneType),
                hex.Price?.Select(p => new PurchasableHex.Price()
                {
                    currencyId = p.Currency,
                    amount = p.Amount
                }).ToList(),
                tags.ToHashSet(),
                hex.Provides?.ToHashSet()));
        }

        private bool AreRequiredModsActive(string[] requiredModIds) => requiredModIds == null || !requiredModIds.Any(Mod.loadedModIds.Contains);

    }
}
