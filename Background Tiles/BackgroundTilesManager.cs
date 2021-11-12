using BackgroundTiles.BackwallTile;
using BackgroundTiles.Buildings;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BackgroundTiles
{
    public class BackgroundTilesManager : KMonoBehaviour
    {
        public static BackgroundTilesManager Instance;
        public Dictionary<BuildingDef, BuildingDef> tiles = new Dictionary<BuildingDef, BuildingDef>();
        private static Dictionary<Tag, Sprite> uiSprites = new Dictionary<Tag, Sprite>();
        private static HashSet<Tag> wallIDs = new HashSet<Tag>();

        private GameObject baseTemplate;

        protected override void OnPrefabInit() => Instance = this;

        public void DestroyInstance() => Instance = null;

        public List<string> GetTileIDs() => tiles.Select(t => t.Key.Tag.ToString()).ToList();

        public static bool IsBackwall(BuildingDef def) => wallIDs.Contains(def.Tag);

        public static Sprite GetSprite(BuildingDef def)
        {
            return uiSprites.TryGetValue(def.Tag, out Sprite sprite) ? sprite : def.GetUISprite();
        }

        // get the base template used for instantiating buildings
        public void SetBaseTemplate()
        {
            baseTemplate = Traverse.Create(BuildingConfigManager.Instance).Field<GameObject>("baseTemplate").Value;
        }

        public void RegisterTile(IBuildingConfig original, BuildingDef originalDef)
        {
            // Check for DLC
            if (!DlcManager.IsDlcListValidForCurrentContent(original.GetDlcIds())) return;

            BuildingDef def = BuildingDefTemplate.CreateBackwallTileDef(originalDef, GetIDForDef(originalDef), original.GetDlcIds());

            // Adding strings early, some things below will already try to access them
            RegisterStrings(originalDef.PrefabID, def.PrefabID);

            // Create building game object
            BuildingHelper.CreateFromBaseTemplate(def, baseTemplate);

            // Add to tech
            // important to do before def.PostProcess()
            RegisterTech(originalDef.PrefabID, def.PrefabID);

            def.PostProcess();

            BuildingHelper.DoPostConfigureComplete(def.BuildingComplete);
            BuildingHelper.DoPostConfigureUnderConstruction(def.BuildingUnderConstruction);

            Assets.AddBuildingDef(def);
            tiles.Add(def, originalDef);
            wallIDs.Add(def.Tag);
            uiSprites.Add(def.Tag, SpriteHelper.GetSpriteForDef(def));
        }

        private string GetIDForDef(BuildingDef def) => Mod.ID + "_" + def.Tag + "Wall";

        private void RegisterStrings(string original, string newTag)
        {
            string key = $"STRINGS.BUILDINGS.PREFABS.{newTag.ToUpperInvariant()}";
            string originalKey = $"STRINGS.BUILDINGS.PREFABS.{original.ToUpperInvariant()}";

            Strings.Add(key + ".NAME", $"Backwall ({Strings.Get(originalKey + ".NAME")})"); // todo: also translatable
            Strings.Add(key + ".DESC", Strings.Get(originalKey + ".DESC"));
            Strings.Add(key + ".EFFECT", Strings.Get(originalKey + ".EFFECT"));
        }

        private void RegisterTech(string originalID, string ID)
        {
            if (Db.Get().Techs is null) return;

            Tech tech = Db.Get().Techs.TryGetTechForTechItem(originalID);
            if (tech is object) tech.AddUnlockedItemIDs(ID);
        }
    }
}
