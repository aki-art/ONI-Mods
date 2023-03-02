using GravitasBigStorage.Content;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace GravitasBigStorage.Patches
{
    internal class ResourceTreeLoaderPatch
    {

        [HarmonyPatch(typeof(ResourceTreeLoader<ResourceTreeNode>), MethodType.Constructor, typeof(TextAsset))]
        public class ResourceTreeLoader_Load_Patch
        {
            public static void Postfix(ResourceTreeLoader<ResourceTreeNode> __instance, TextAsset file)
            {
                if (file.name != "TechTree_Expansion1_Generated")
                {
                    return;
                }

                AddNode(__instance);
            }

            private static void AddNode(ResourceTreeLoader<ResourceTreeNode> tech_tree_nodes)
            {
                var node = new ResourceTreeNode
                {
                    height = 0,
                    width = 0,
                    nodeX = 0,
                    nodeY = 0,
                    edges = new List<ResourceTreeNode.Edge>(),
                    references = new List<ResourceTreeNode>() { },
                    Disabled = false,
                    Id = GBSTechs.BIG_BOY_STORAGE,
                    Name = GBSTechs.BIG_BOY_STORAGE
                };

                tech_tree_nodes.resources.Add(node);
            }
        }
    }
}
