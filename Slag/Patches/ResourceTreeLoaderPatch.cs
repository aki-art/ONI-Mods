using FUtility;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Slag.Patches
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
                ResourceTreeNode tempModNode = null;
                var x = 0f;
                var y = 0f;

                foreach (var item in tech_tree_nodes)
                {
                    if (item.Id == Consts.TECH.GASES.TEMPERATURE_MODULATION)
                    {
                        tempModNode = item;
                    }
                    else if (item.Id == Consts.TECH.GASES.DIRECTED_AIR_STREAMS)
                    {
                        y = item.nodeY;
                    }
                    else if (item.Id == Consts.TECH.GASES.HVAC)
                    {
                        x = item.nodeX;
                    }
                }

                if (tempModNode == null)
                {
                    return;
                }

                var id = ModAssets.Techs.ADVANCED_INSULATION_ID;
                var node = new ResourceTreeNode
                {
                    height = tempModNode.height,
                    width = tempModNode.width,
                    nodeX = x,
                    nodeY = y,
                    edges = new List<ResourceTreeNode.Edge>(tempModNode.edges),
                    references = new List<ResourceTreeNode>() { },
                    Disabled = false,
                    Id = id,
                    Name = id
                };

                tempModNode.references.Add(node);
                tech_tree_nodes.resources.Add(node);
            }
        }
    }
}
