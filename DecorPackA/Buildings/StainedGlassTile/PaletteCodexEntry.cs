using System.Collections.Generic;
using UnityEngine;

namespace DecorPackA.Buildings.StainedGlassTile
{
    public class PaletteCodexEntry
    {
        public const string PALETTE = "STAINEDGLASSPALETTE";
        public const string MODS_CATEGORY = "MODS";

        public static CodexEntry GeneratePaletteEntry()
        {
            var categoryEntry = CodexEntryGenerator.GenerateCategoryEntry(PALETTE, STRINGS.UI.CODEX.PALETTE, new Dictionary<string, CodexEntry>());
            categoryEntry.category = MODS_CATEGORY;
            PopulateEntries(categoryEntry);

            return categoryEntry;
        }

        // Makes those little clickable cards in the codex
        // These cards are "fake", they don't lead to additional articles
        private static void PopulateEntries(CategoryEntry categoryEntry)
        {
            var contentContainer = new ContentContainer(new List<ICodexWidget>(), ContentContainer.ContentLayout.Grid);

            foreach (var tile in StainedGlassTiles.tileTagDict)
            {
                var tilePrefab = Assets.GetPrefab(tile.Value);

                if (tilePrefab == null)
                {
                    continue;
                }

                var elementId = tile.Key.ToString();
                var elementName = StainedGlassTiles.GetFormattedName(tile.Key.ProperNameStripLink());
                var icon = Def.GetUISprite(tilePrefab);

                contentContainer.content.Add(new StainedGlassCodexLabel(
                    elementName,
                    CodexTextStyle.BodyWhite,
                    new Tuple<Sprite, Color>((icon != null) ? icon.first : Assets.GetSprite("unknown"), Color.white),
                    elementId + "_dye",
                    elementId));
            }

            categoryEntry.contentContainers.Add(contentContainer);
        }
    }
}
