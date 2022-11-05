using FUtility;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace SnowSculptures.Patches
{
    public class BuildToolPatch
    {
        private const float DEFAULT_TEMPERATURE = 293.15f;

        public static void Patch(Harmony harmony)
        {
            var original = AccessTools.Method(typeof(BuildTool), "TryBuild");
            var transpiler = AccessTools.Method(typeof(BuildTool_TryBuild_Patch), "Transpiler");
            harmony.Patch(original, transpiler: new HarmonyMethod(transpiler));
        }

        // if a building is placed in debug mode, this sets the default temperature of it to something that doesn't melt immediately
        public class BuildTool_TryBuild_Patch
        {
            public static IEnumerable<CodeInstruction> Transpiler(ILGenerator generator, IEnumerable<CodeInstruction> orig)
            {
                var m_GetDefaultTemperatureForDef = AccessTools.Method(typeof(BuildTool_TryBuild_Patch), "GetDefaultTemperatureForDef", new[] { typeof(IList<Tag>) });
                var m_GetDefaultTemperatureForDef2 = AccessTools.Method(typeof(BuildTool_TryBuild_Patch), "GetDefaultTemperatureForDef", new System.Type[] { });
                var f_selectedElements = AccessTools.Field(typeof(BuildTool), "selectedElements");

                var codes = orig.ToList();
                var index = codes.FindIndex(code => code.LoadsConstant(DEFAULT_TEMPERATURE));

                if (index != -1)
                {

                    codes.RemoveAt(index);
                    codes.InsertRange(index, new[]
                    {
                        new CodeInstruction(OpCodes.Ldarg_0), // load instance
                        new CodeInstruction(OpCodes.Ldfld, f_selectedElements), // load selectedElements field on instance
                        new CodeInstruction(OpCodes.Call, m_GetDefaultTemperatureForDef)// GetDefaultTemperatureForDef(selectedElements)
                    });
                }

                Log.PrintInstructions(codes);

                return codes;
            }

            private static float GetDefaultTemperatureForDef(IList<Tag> selectedElements)
            {
                if (selectedElements == null || selectedElements.Count == 0)
                {
                    return DEFAULT_TEMPERATURE;
                }

                var element = ElementLoader.GetElement(selectedElements[0]);

                return element.lowTemp <= DEFAULT_TEMPERATURE || element.highTemp >= DEFAULT_TEMPERATURE
                    ? element.defaultValues.temperature
                    : DEFAULT_TEMPERATURE;
            }
        }
    }
}
