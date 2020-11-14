/*using FUtility;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static global::STRINGS.BUILDINGS.PREFABS;

namespace DecorExpansion
{
    class Patches
    {
        public class Overrides
        {
            public Dictionary<string, ArtableOverride> ice;

        }
        public static Dictionary<string, Dictionary<string, ArtableOverride>> overrides;

        static Dictionary<string, ArtableOverride> artableOverrides;
        public struct ArtableOverride
        {
            public KAnimFile anim;
            public string targetID;
            public bool valid;

            public ArtableOverride(string animName, string targetID)
            {
                this.targetID = targetID;
                anim = Assets.Anims.FirstOrDefault(a => a.name == animName);
                valid = anim != default;
            }
        }

        public static class Mod_OnLoad
        {
            public static void OnLoad(string path)
            {
                ModAssets.ModPath = path;
                Log.PrintVersion();
            }
        }

        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Postfix()
            {
                artableOverrides = new Dictionary<string, ArtableOverride>();
                overrides = new Dictionary<string, Dictionary<string, ArtableOverride>>
                {
                    { IceSculptureConfig.ID, GatherAllStates(IceSculptureConfig.ID, "icesculpture", "testsculpture") }
                };
            }
        }

        private static Dictionary<string, ArtableOverride> GatherAllStates(string id, params string[] anims)
        {
            var result = new Dictionary<string, ArtableOverride>();
            foreach(var anim in anims)
                AddOverride(id, anim, result);

            return result;
        }

        private static void AddOverride(string id, string anim, Dictionary<string, ArtableOverride> result)
        {
            foreach (var stage in GetArtable(id).stages)
            {
                result.Add(ModAssets.PREFIX + anim + stage.id, new ArtableOverride(anim + "_kanim", id));
            }
        }

        static void AddVariants(string target, string variant)
        {
            Artable targetArtable = GetArtable(target);
            Artable newArtable = GetArtable(variant);
            List<Artable.Stage> stages = new List<Artable.Stage>( newArtable.stages );
            stages.RemoveAll(a => a.id == "Default");
            targetArtable.stages.AddRange(stages);

            foreach(var stage in targetArtable.stages)
                artableOverrides.Add(stage.id, variant);
        }

        static Artable GetArtable(string name)
        {
            BuildingDef def = Assets.GetBuildingDef(name);
            return def.BuildingComplete.GetComponent<Artable>();
        }


        [HarmonyPatch(typeof(Artable), "SetStage")]
        public static class Artable_SetStage_Patch
        {
            public static bool Prefix(string stage_id)
            {
                return !artableOverrides.ContainsValue(stage_id);
            }

            public static void Postfix(Artable __instance, string stage_id)
            {
                if (artableOverrides.TryGetValue(stage_id, out string newPrefab))
                {
                    GameObject newobject = Util.KInstantiate(Assets.GetPrefab(newPrefab));
                    Artable artable = newobject.GetComponent<Artable>();
                    artable.SetStage(stage_id, true);
                    newobject.SetActive(true);
                    Util.KDestroyGameObject(__instance.gameObject);
                }
            }
        }
    }
}*/