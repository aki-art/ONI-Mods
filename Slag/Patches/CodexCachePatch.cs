using FUtility;
using HarmonyLib;
using Slag.Content.Critters;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Slag.Patches
{
    public class CodexCachePatch
    {

        [HarmonyPatch(typeof(CodexEntryGenerator), "GenerateCreatureEntries")]
        public class CodexEntryGenerator_GenerateCreatureEntries_Patch
        {
            private static void Postfix(Dictionary<string, CodexEntry> __result)
            {
                var codexEntryGenerator = Traverse.Create(typeof(CodexEntryGenerator));

                var slagmites = new HashSet<Tag>()
                {
                    SlagmiteConfig.ID
                };

                CreateEntry(
                    ModAssets.Tags.Species.slagmite,
                    slagmites,
                    STRINGS.CREATURES.FAMILY_PLURAL.SLAGMITESPECIES,
                    __result,
                    codexEntryGenerator.Method("GenerateImageContainers", new[] { typeof(Sprite[]), typeof(List<ContentContainer>), typeof(ContentContainer.ContentLayout) }),
                    codexEntryGenerator.Method("GenerateImageContainers", new[] { typeof(Sprite), typeof(List<ContentContainer>) }),
                    codexEntryGenerator.Method("GenerateCreatureDescriptionContainers", new[] { typeof(GameObject), typeof(List<ContentContainer>) }));
            }

            private static void CreateEntry(
                Tag speciesTag,
                HashSet<Tag> tags,
                string name,
                Dictionary<string, CodexEntry> results,
                Traverse m_GenerateImageContainers_a,
                Traverse m_GenerateImageContainers_b,
                Traverse m_GenerateCreatureDescriptionContainers)
            {
                bool hasFoundFirstValidCreature = false;
                var containers = new List<ContentContainer>();
                var entry = new CodexEntry("CREATURES", containers, name);

                foreach (var tag in tags)
                {
                    var critter = Assets.GetPrefab(tag);

                    Sprite spriteBaby = null;

                    var brain = critter.GetComponent<CreatureBrain>();

                    if (!hasFoundFirstValidCreature)
                    {
                        hasFoundFirstValidCreature = true;

                        var contentContainer = new ContentContainer(new List<ICodexWidget>
                    {
                        new CodexSpacer(),
                        new CodexSpacer()
                    },
                        ContentContainer.ContentLayout.Vertical);

                        containers.Add(contentContainer);

                        entry.parentId = "CREATURES";
                        CodexCache.AddEntry(speciesTag.ToString(), entry, null);
                        results.Add(speciesTag.ToString(), entry);
                    }

                    var subEntryContainers = new List<ContentContainer>();
                    var prefix = brain.symbolPrefix;
                    var spriteGrown = Def.GetUISprite(critter, prefix + "ui").first;

                    if (spriteBaby)
                    {
                        m_GenerateImageContainers_a.GetValue(new Sprite[]
                        {
                        spriteGrown,
                        spriteBaby
                        },
                        subEntryContainers,
                        ContentContainer.ContentLayout.Horizontal);
                    }
                    else
                    {
                        m_GenerateImageContainers_b.GetValue(spriteGrown, subEntryContainers);
                    }

                    m_GenerateCreatureDescriptionContainers.GetValue(critter, subEntryContainers);

                    var subEntry = new SubEntry(
                        brain.PrefabID().ToString(),
                        speciesTag.ToString(),
                        subEntryContainers,
                        brain.GetProperName())
                    {
                        icon = spriteGrown,
                        iconColor = Color.white
                    };

                    entry.subEntries.Add(subEntry);
                }
            }
        }

        [HarmonyPatch(typeof(CodexCache), "CollectEntries")]
        public static class CodexCache_CollectEntries_Patch
        {
            public static void Postfix(string folder, List<CodexEntry> __result)
            {
                if (folder == "Creatures")
                {
                    var extraEntries = CodexCache.CollectEntries(Path.Combine(Utils.ModPath, "codex", "Creatures"));

                    foreach (var entry in extraEntries)
                    {
                        entry.category = "CREATURES";
                    }

                    __result.AddRange(extraEntries);
                }
            }
        }

        [HarmonyPatch(typeof(CodexCache), "CollectSubEntries")]
        public static class CodexCache_CollectSubEntries_Patch
        {
            public static void Postfix(string folder, List<SubEntry> __result)
            {
                if (folder == "")
                {
                    var extraEntries = CodexCache.CollectSubEntries(Path.Combine(Utils.ModPath, "codex", "Creatures", "Slagmites"));
                    __result.AddRange(extraEntries);
                }
            }
        }
    }
}
