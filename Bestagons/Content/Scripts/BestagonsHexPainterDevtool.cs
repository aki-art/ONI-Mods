using Bestagons.Content.Map;
using Bestagons.Content.ModDb;
using ImGuiNET;
using Klei;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static ProcGen.SubWorld;

namespace Bestagons.Content.Scripts
{
    public class BestagonsHexPainterDevtool : DevTool
    {
        public int centerCell;

        private const string SAMPLE_TEMPLATE = "bestagons/debug/smallerHexNeutronium";
        private HashSet<int> borderCells;
        private int cell;
        private TemplateContainer template;
        private ushort neutroniumIdx;
        private string templateName = "template name";
        private string icon = "";
        private int selectedZoneType;
        private string[] zoneTypes;
        private string newTag = "";
        private List<string> tags = new();
        private bool templateNameExists = false;
        private bool iconInvalid = false;

        public BestagonsHexPainterDevtool()
        {
            RequiresGameRunning = true;
            neutroniumIdx = ElementLoader.GetElementIndex(SimHashes.Unobtanium);

            zoneTypes = GetZonesList()
                .Select(z => z.ToString())
                .ToArray();
        }

        public List<ZoneType> GetZonesList()
        {
            var zones = new List<ZoneType>();

            foreach (ZoneType zone in Enum.GetValues(typeof(ZoneType)))
            {
                zones.Add(zone);
            }

            return zones;
        }

        public override void RenderTo(DevPanel panel)
        {
            if (ImGui.Button("Place Empty Template"))
                PlaceEmptyTemplate();

            if (borderCells != null)
                CheckBorder();

            if (ImGui.InputText("template name", ref templateName, 255))
                CheckTemplateName(templateName);

            if (templateNameExists)
                ImGui.TextColored(new(1, 0, 0, 1), "Template name already exists!");

            if (ImGui.InputText("icon", ref icon, 255))
                CheckIcon(icon);

            if (iconInvalid)
                ImGui.TextColored(new(1, 0, 0, 1), "No icon with this name!");

            ImGui.Spacing();
            ImGui.Text("Tags");

            for (int i = 0; i < tags.Count; i++)
            {
                string tag = tags[i];
                ImGui.Text("   " + tag.ToString());
                ImGui.SameLine();
                if (ImGui.SmallButton("X"))
                    tags.RemoveAt(i);
            }

            ImGui.InputText("add tag", ref newTag, 255);
            ImGui.SameLine();
            if (ImGui.Button("Add"))
            {
                tags.Add(newTag);
                newTag = "";
            }

            ImGui.Spacing();

            if (ImGui.Combo("ZoneType", ref selectedZoneType, zoneTypes, zoneTypes.Length))
                ChangeZoneType();

            ImGui.Spacing();

            if (ImGui.Button("Save"))
            {
                var templatePath = SaveTemplate();
                var data = new HexTileCollection.Data()
                {
                    TemplateId = templatePath,
                    Id = templateName,
                    Icon = icon,
                    ZoneType = GetSelectedZoneType().ToString(),
                    Price = new List<HexTileCollection.Price>(),
                    Tags = tags.ToArray()
                };

                // TODO: select collection
                var collection = new HexTileCollection()
                {
                    HexTiles = new List<HexTileCollection.Data> { data }
                };

                var folder = Path.Combine(Utils.ModPath, "hexes");
                SaveYaml(collection, folder, templateName);
            }
        }

        private string SaveTemplate()
        {
            DebugBaseTemplateButton.Instance.ClearSelection();

            Grid.CellToXY(cell, out int xOffset, out int yOffset);

            foreach (var templateCell in template.cells)
            {
                var actualCell = Grid.XYToCell(templateCell.location_x + xOffset, templateCell.location_y + yOffset);
                DebugBaseTemplateButton.Instance.AddToSelection(actualCell);
            }

            var asset = DebugBaseTemplateButton.Instance.GetSelectionAsAsset();

            var folder = Path.Combine(Utils.ModPath, "templates", "bestagons", "hexes");
            SaveYaml(asset, folder, templateName);

            TemplateCache.Clear();
            TemplateCache.Init();

            DebugBaseTemplateButton.Instance.ClearSelection();

            return "bestagons/hexes/" + templateName;
        }

        private void SaveYaml(object asset, string folder, string fileName)
        {
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var filename = Path.Combine(folder, fileName + ".yaml");
            YamlIO.Save(asset, fileName);
        }

        private void CheckIcon(string icon) => iconInvalid = !Assets.TryGetAnim(icon, out _);

        private void CheckTemplateName(string templateName) => 
            templateNameExists = TemplateCache.templates.ContainsKey("bestagons/hexes/" + templateName);

        private void ChangeZoneType()
        {
            if (template == null)
                return;

            Grid.CellToXY(cell, out int xOffset, out int yOffset);

            foreach (var templateCell in template.cells)
            {
                var actualCell = Grid.XYToCell(templateCell.location_x + xOffset, templateCell.location_y + yOffset);
                ZoneType zone = GetSelectedZoneType();
                SimMessages.ModifyCellWorldZone(actualCell, (byte)zone);
                Bestagons_Mod.Instance.AddZoneOverride(actualCell, zone);
            }

            Bestagons_Mod.Instance.RegenerateBackwallTexture();
        }

        private ZoneType GetSelectedZoneType()
        {
            return (ZoneType)Enum.Parse(typeof(ZoneType), zoneTypes[selectedZoneType]);
        }

        private void CheckBorder()
        {
            foreach (var cell in borderCells)
            {
                if (Grid.ElementIdx[cell] != neutroniumIdx)
                {
                    SimMessages.AddRemoveSubstance(
                        cell,
                        SimHashes.Unobtanium,
                        null,
                        1000f,
                        300f,
                        byte.MaxValue,
                        0,
                        false);
                }
            }
        }

        private void PlaceEmptyTemplate()
        {
            cell = SelectTool.Instance.selectedCell;
            var position = Grid.CellToPos(cell);

            template = TemplateCache.GetTemplate(SAMPLE_TEMPLATE);
            if (template == null)
            {
                Log.Warning($"Cannot place non-existing template: {SAMPLE_TEMPLATE}");
                return;
            }

            var bounds = template.GetTemplateBounds(position);
            var min = Grid.XYToCell(bounds.min.x, bounds.min.y);
            var max = Grid.XYToCell(bounds.max.x, bounds.max.y);

            if (!Grid.IsValidBuildingCell(min) || !Grid.IsValidBuildingCell(max))
            {
                Log.Warning("template outside of bounds");
                return;
            }

            TemplateLoader.Stamp(template, position, OnTemplatePlaced);
        }

        private void OnTemplatePlaced()
        {
            Grid.CellToXY(cell, out int xOffset, out int yOffset);
            borderCells = template.cells
                .Where(c => c.element == SimHashes.Unobtanium)
                .Select(c => Grid.XYToCell(c.location_x + xOffset, c.location_y + yOffset))
                .ToHashSet();
        }
    }
}
