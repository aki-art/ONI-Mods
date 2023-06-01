using Bestagons.Content.ModDb;
using ImGuiNET;
using Klei;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
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

        // collections
        private int collectionIdx = 0;
        private string[] collections;
        private string newCollectionId = "";
        private bool editingNewCollection = false;
        private bool justStartedEditingNewCollection = false;

        private bool editCollectionPopup = false;

        // project
        private int projectIdx = 0;
        private string[] projects;
        private NewProjectSettings newProjectSettings = new();
        private bool newProjectVisible = false;
        private readonly string[] contentVersions = new[]
        {
            "Base Game only",
            "Space Out! only",
            "All Versions"
        };

        private float projectSaved = 0;
        private const float TEXT_FADE_DURATION = 5f;

        public const string CREATE_NEW = "Create New";
        public const string SEPARATOR = "_______";

        public BestagonsHexPainterDevtool()
        {
            RequiresGameRunning = true;

            template = TemplateCache.GetTemplate(SAMPLE_TEMPLATE);
            if (template == null)
            {
                Log.Warning($"Cannot find base template: {SAMPLE_TEMPLATE}");
                return;
            }

            neutroniumIdx = ElementLoader.GetElementIndex(SimHashes.Unobtanium);

            zoneTypes = GetZonesList()
                .Select(z => z.ToString())
                .ToArray();

            // TODO: collect them
            collections = new string[]
            {
                "Vanilla",
                "SpacedOut"
            };

            projects = new string[]
            {
                "Bestagons"
            };
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
            RenderCollections();
            RenderCollectionPopup();

            ImGui.Spacing();

            RenderProjects();

            ImGui.Spacing();

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

            ImGui.InputTextWithHint("##newtag", "new tag", ref newTag, 255);
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

        private void RenderCollectionPopup()
        {
            if(ImGui.BeginPopup("collection_editor"))
            {
                bool vanilla = true, dlc = true;
                ImGui.Checkbox("Available Vanilla", ref vanilla);
                ImGui.Checkbox("Available Spaced Out!", ref dlc);

                ImGui.Separator();

                ImGui.SetTooltip("Tags that get applied to every hex in this collection");
                ImGui.BeginTooltip();
                ImGui.Text("Tags:");
                ImGui.EndTooltip();

                ImGui.BulletText("Vanilla"); ImGui.SameLine(); ImGui.SmallButton("X");
                ImGui.BulletText("SpacedOut"); ImGui.SameLine(); ImGui.SmallButton("X");
                // TODO: tags

                ImGui.InputTextWithHint("##newtag", "new tag", ref newTag, 255);
                ImGui.SameLine();
                if (ImGui.Button("Add"))
                {
                    tags.Add(newTag);
                    newTag = "";
                }

                ImGui.Separator();

                ImGui.Button("Save");
                ImGui.SameLine();

                ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0.3f, 0.3f, 0.85f, 1f));
                ImGui.PushStyleColor(ImGuiCol.ButtonActive, new Vector4(0.3f, 0.3f, 0.85f, 1f));
                ImGui.PushStyleColor(ImGuiCol.ButtonHovered, new Vector4(0.3f, 0.3f, 0.85f, 1f));
                ImGui.Button($"Delete {collections[collectionIdx]}");
                ImGui.PopStyleColor(3);

                ImGui.EndPopup();
            }
        }

        private void RenderCollections()
        {
            if (ImGui.CollapsingHeader("Collections", ImGuiTreeNodeFlags.DefaultOpen))
            {
                if (!editingNewCollection)
                {
                    ImGui.SetTooltip("Collections are useful to organize your tiles.");
                    if (ImGui.BeginCombo("Collection", collections[collectionIdx]))
                    {
                        for (int i = 0; i < collections.Length; i++)
                        {
                            var selected = collectionIdx == i;

                            if (ImGui.Selectable(collections[i], selected))
                                collectionIdx = i;

                            if (selected)
                                ImGui.SetItemDefaultFocus();
                        }

                        ImGui.Selectable(SEPARATOR, false);

                        if(ImGui.Selectable(CREATE_NEW, false))
                        {
                            editingNewCollection = true;
                            justStartedEditingNewCollection = true;
                        }

                        ImGui.EndCombo();
                    }

                    if (ImGui.Button($"Edit {collections[collectionIdx]}"))
                        ImGui.OpenPopup("collection_editor");

                    ImGui.SameLine();
                    if (ImGui.Button("Delete Empty Collections"))
                    {
                        // TODO
                    }
                }
                else
                {
                    if(justStartedEditingNewCollection)
                    {
                        ImGui.SetKeyboardFocusHere();
                        justStartedEditingNewCollection = false;
                    }

                    ImGui.InputTextWithHint("##newCollectionId", "Collection ID" ,ref newCollectionId, 128);

                    ImGui.SameLine();

                    if (ImGui.SmallButton("Save"))
                    {
                        // Add Collection
                        var items = new List<string>(collections)
                        {
                            newCollectionId
                        };

                        items.Sort();

                        collectionIdx = items.IndexOf(newCollectionId); // select it immediately
                        collections = items.ToArray();

                        editingNewCollection = false;
                        newCollectionId = "";
                    }

                    ImGui.SameLine();

                    if (ImGui.SmallButton("Cancel"))
                    {
                        editingNewCollection = false;
                        newCollectionId = "";
                    }
                }
            }
        }

        private void RenderProjects()
        {
            ImGui.PushItemWidth(30);
            if (ImGui.CollapsingHeader("Project", ImGuiTreeNodeFlags.DefaultOpen))
            {
                ImGui.PopItemWidth();

                ImGui.SameLine(); ImGui.Text("Test");
                ImGui.SameLine(); ImGui.Separator();

                ImGui.Combo("ID", ref projectIdx, projects, projects.Length);

                if (ImGui.Button("Create new project"))
                    newProjectVisible = !newProjectVisible;

                if (newProjectVisible)
                {
                    ImGui.InputTextWithHint("Static ID:", "Unique Mod ID", ref newProjectSettings.ID, 256);
                    ImGui.InputText("Author", ref newProjectSettings.author, 256);
                    ImGui.InputText("Name", ref newProjectSettings.name, 256);
                    ImGui.InputText("Description", ref newProjectSettings.description, 256);
                    ImGui.Combo("Supported", ref newProjectSettings.supportedContent, contentVersions, contentVersions.Length);

                    ImGui.PushItemWidth(30);
                    ImGui.Text("Version: ");
                    ImGui.SameLine(); ImGui.DragInt("##version0", ref newProjectSettings.versions[0]);
                    ImGui.SameLine(); ImGui.DragInt("##version1", ref newProjectSettings.versions[1]);
                    ImGui.SameLine(); ImGui.DragInt("##version2", ref newProjectSettings.versions[2]);
                    ImGui.SameLine(); ImGui.DragInt("##version3", ref newProjectSettings.versions[3]);
                    ImGui.PopItemWidth();

                    ImGui.Separator();

                    ImGui.InputTextWithHint("Min. Game version", "Leave empty if unsure", ref newProjectSettings.minGameVersion, 16);

                    if (ImGui.Button("Save"))
                    {
                        // save

                        newProjectVisible = false;
                        projectSaved = TEXT_FADE_DURATION;
                    }
                }
                else if (projectSaved > 0)
                {
                    projectSaved -= Time.deltaTime;
                    var t = projectSaved / TEXT_FADE_DURATION;
                    ImGui.TextColored(new(1, 1, 1, Mathf.Lerp(0, 1, t)), "Project saved");
                    ImGui.Spacing();
                }
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

            var path = Path.Combine(folder, fileName + ".yaml");
            YamlIO.Save(asset, path);
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

        public class NewProjectSettings
        {
            public string ID = "";
            public string author = "";
            public string name = "";
            public string description = "";
            public int supportedContent = 2;
            public string minGameVersion = "";
            public string version = "1.0.0.0";
            public int[] versions = new[] { 1, 0, 0, 0 };
        }
    }
}
