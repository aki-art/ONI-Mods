using Database;
using FUtility;
using HarmonyLib;
using System.Collections.Generic;

namespace Slag.Patches
{
    public class TechTreeTitlesPatch
    {
        //[HarmonyPatch(typeof(TechTreeTitles), "Load")]
        public class TechTreeTitles_Load_Patch
        {
            public static void Postfix(TechTreeTitles __instance)
            {
                var id = ModAssets.Techs.ADVANCED_INSULATION_ID;

                Log.Debuglog("ITEMS");
                foreach (var item in __instance.resources)
                {
                    Log.Debuglog(item.Id);
                }

                var temperatureModulation = __instance.Get(Consts.TECH.GASES.TEMPERATURE_MODULATION);

                Log.Assert("temp modulation resource", temperatureModulation);
                var temperatureModulationNode = Traverse.Create(temperatureModulation).Field<ResourceTreeNode>("node").Value;
                Log.Assert("temperatureModulationNode", temperatureModulationNode);
                Log.Assert("temperatureModulationNode.edges", temperatureModulationNode.edges);

                var node = new ResourceTreeNode
                {
                    height = temperatureModulationNode.height,
                    width = temperatureModulationNode.width,
                    nodeX = temperatureModulationNode.nodeX,
                    nodeY = temperatureModulationNode.nodeY,
                    edges = new List<ResourceTreeNode.Edge>(temperatureModulationNode.edges),
                    references = new List<ResourceTreeNode>() { temperatureModulationNode },
                    Disabled = false,
                    Id = id,
                    Name = id
                };

                new TechTreeTitle(id, __instance, Strings.Get("STRINGS.RESEARCH.TREES.TITLE" + id.ToUpper()), node);
            }
        }
    }
}
