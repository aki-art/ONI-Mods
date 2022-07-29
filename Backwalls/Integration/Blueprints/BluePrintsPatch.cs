using Backwalls.Buildings;
using FUtility;
using HarmonyLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Backwalls.Integration.Blueprints
{
    // compatibility with Blueprints mod
    internal class BluePrintsPatch
    {
        public static Dictionary<object, AdditionalData> blueprintsMetaData = new Dictionary<object, AdditionalData>();
        public static Dictionary<int, AdditionalData.BackwallData> wallDataCache = new Dictionary<int, AdditionalData.BackwallData>();
        public static object readingBlueprint;

        public static void TryPatch(Harmony harmony)
        {
            var bluePrintsState = Type.GetType("Blueprints.BlueprintsState, Blueprints", false, false);
            if (bluePrintsState != null)
            {
                var m_createBluePrint = bluePrintsState.GetMethod("CreateBlueprint");
                var postfix = typeof(Blueprints_BlueprintsState_CreateBlueprint_Patch).GetMethod("Postfix");
                harmony.Patch(m_createBluePrint, null, new HarmonyMethod(postfix));

                var m_useBluePrint = bluePrintsState.GetMethod("UseBlueprint");
                var postfix2 = typeof(Blueprints_BlueprintsState_UseBlueprint_Patch).GetMethod("Postfix");
                harmony.Patch(m_useBluePrint, null, new HarmonyMethod(postfix2));
            }

            var blueprintType = Type.GetType("Blueprints.Blueprint, Blueprints", false, false);
            if (blueprintType != null)
            {
                var m_ReadJson = blueprintType.GetMethod("ReadJson", BindingFlags.Instance | BindingFlags.Public);
                var prefix2 = typeof(Blueprint_ReadJson_Patch).GetMethod("Prefix", BindingFlags.Public | BindingFlags.Static);
                harmony.Patch(m_ReadJson, new HarmonyMethod(prefix2));
            }

            var buildingConfigType = Type.GetType("Blueprints.BuildingConfig, Blueprints", false, false);
            if (buildingConfigType != null)
            {
                var m_WriteJson = buildingConfigType.GetMethod("WriteJson", new[] { typeof(JsonWriter) });
                var transpiler = typeof(BuildingConfig_WriteJson_Patch).GetMethod("Transpiler", BindingFlags.Public | BindingFlags.Static);
                harmony.Patch(m_WriteJson, transpiler: new HarmonyMethod(transpiler));

                var m_ReadJson = buildingConfigType.GetMethod("ReadJson", new[] { typeof(JObject) });
                var transpiler2 = typeof(BuildingConfig_ReadJson_Patch).GetMethod("Transpiler", BindingFlags.Public | BindingFlags.Static);
                harmony.Patch(m_ReadJson, transpiler: new HarmonyMethod(transpiler2));
            }

            Log.Info("Patched Blueprints to work with backwalls.");
        }

        public class BuildingConfig_ReadJson_Patch
        {
            public static IEnumerable<CodeInstruction> Transpiler(ILGenerator generator, IEnumerable<CodeInstruction> orig)
            {
                var codes = orig.ToList();

                var m_GetBuildingDef = typeof(Assets).GetMethod("GetBuildingDef", BindingFlags.Public | BindingFlags.Static);
                var m_GetDefID = typeof(BuildingConfig_ReadJson_Patch).GetMethod("GetDefID", BindingFlags.NonPublic | BindingFlags.Static);

                var index = codes.FindIndex(c => c.opcode == OpCodes.Call && c.operand is MethodInfo m && m == m_GetBuildingDef);

                if (index == -1)
                {
                    return codes;
                }

                codes.InsertRange(index, new[]
                {
                    new CodeInstruction(OpCodes.Ldarg_1),
                    new CodeInstruction(OpCodes.Call, m_GetDefID)
                });

                Log.PrintInstructions(codes);

                return codes;
            }

            private static string[] delimiter = new[]
            {
                "__"
            };

            private static string GetDefID(string def, JObject rootObject)
            {
                Log.Debuglog("PATCHING " + def);
                if (def.StartsWith(BackwallConfig.ID))
                {
                    var split = def.Split(':');
                    Log.Debuglog("splitting");
                    if (split != null && split.Length == 3 && split[0] == BackwallConfig.ID)
                    {
                        Log.Debuglog("success");
                        foreach (var item in split)
                        {
                            Log.Debuglog(item);
                        }

                        Log.Assert("readingBlueprint", readingBlueprint);
                        if (!blueprintsMetaData.ContainsKey(readingBlueprint))
                        {
                            blueprintsMetaData.Add(readingBlueprint, new AdditionalData()
                            {
                                Data = new Dictionary<Vector2I, AdditionalData.BackwallData>()
                            });
                        }

                        var offset = Vector2I.zero;
                        var offsetToken = rootObject.SelectToken("offset");

                        if (offsetToken != null && offsetToken.Type == JTokenType.Object)
                        {
                            var x = offsetToken.Value<int>("x");
                            var y = offsetToken.Value<int>("y");
                            offset = new Vector2I(x, y);
                        }

                        Log.Debuglog(offset);
                        blueprintsMetaData[readingBlueprint].Data[offset] = new AdditionalData.BackwallData()
                        {
                            ColorHex = split[1],
                            Pattern = split[2]
                        };

                        return split[0];
                    }
                }

                return def;
            }
        }

        public class BuildingConfig_WriteJson_Patch
        {
            public static IEnumerable<CodeInstruction> Transpiler(ILGenerator generator, IEnumerable<CodeInstruction> orig)
            {
                var codes = orig.ToList();

                var f_PrefabID = typeof(Def).GetField("PrefabID");
                var m_GetCompondID = typeof(BuildingConfig_WriteJson_Patch).GetMethod("GetCompondID", BindingFlags.NonPublic | BindingFlags.Static);

                for (var i = 0; i < codes.Count; i++)
                {
                    var code = codes[i];
                    if (code.opcode == OpCodes.Ldfld && code.operand is FieldInfo f && f == f_PrefabID)
                    {
                        codes.InsertRange(i + 1, new[] {
                            new CodeInstruction(OpCodes.Ldarg_0),
                            new CodeInstruction(OpCodes.Call, m_GetCompondID)
                        });
                    }
                }

                Log.PrintInstructions(codes);
                return codes;
            }

            private static string GetCompondID(string def, object buildingConfig)
            {
                if (def == BackwallConfig.ID)
                {
                    var bluePrintsState = Type.GetType("Blueprints.BlueprintsState, Blueprints", false, false); //TODO cache
                    var blueprint = bluePrintsState.GetProperty("SelectedBlueprint").GetValue(null);

                    var offset = Traverse.Create(buildingConfig).Property<Vector2I>("Offset").Value;

                    if (blueprintsMetaData.TryGetValue(blueprint, out var data))
                    {
                        if (data.Data.TryGetValue(offset, out var backwallInfo))
                        {
                            return def + ":" + backwallInfo.ColorHex + ":" + backwallInfo.Pattern;
                        }
                    }
                }

                return def;
            }
        }

        public static class Blueprints_BlueprintsState_UseBlueprint_Patch
        {
            public static void Postfix(Vector2I topLeft)
            {
                var bluePrintsState = Type.GetType("Blueprints.BlueprintsState, Blueprints", false, false); //TODO cache
                var blueprint = bluePrintsState.GetProperty("SelectedBlueprint").GetValue(null);

                if (blueprintsMetaData.TryGetValue(blueprint, out var value))
                {
                    foreach (var data in value.Data)
                    {
                        var cell = Grid.PosToCell(topLeft + data.Key);
                        wallDataCache[cell] = data.Value;
                    }
                }
            }
        }

        public static class Blueprints_BlueprintsState_CreateBlueprint_Patch
        {
            public static void Postfix(object __result, Vector2I topLeft, Vector2I bottomRight)
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();

                var t_bluePrint = Traverse.Create(__result);
                var BuildingConfiguration = t_bluePrint.Property<object>("BuildingConfiguration").Value as IList;

                var metadata = new AdditionalData
                {
                    Data = new Dictionary<Vector2I, AdditionalData.BackwallData>()
                };

                blueprintsMetaData[__result] = metadata;

                foreach (var config in BuildingConfiguration)
                {
                    var t_config = Traverse.Create(config);
                    var def = t_config.Property<BuildingDef>("BuildingDef").Value;

                    if (def.PrefabID == BackwallConfig.ID)
                    {
                        var offset = t_config.Property<Vector2I>("Offset").Value;
                        var cell = Grid.XYToCell(topLeft.x + offset.x, bottomRight.y + offset.y);

                        Log.Debuglog($"SAVING AT {cell} \t\toffset: {offset} \t\ttopLeft: {topLeft} \t\tbottomRight: {bottomRight}");

                        if (Grid.ObjectLayers[(int)ObjectLayer.Backwall].TryGetValue(cell, out var go))
                        {
                            var backwall = go.GetComponent<Backwall>();

                            if (backwall == null)
                            {
                                Log.Warning("this tile has no backwall");
                                return;
                            }

                            metadata.Data.Add(offset, new AdditionalData.BackwallData()
                            {
                                ColorHex = backwall.colorHex,
                                Pattern = backwall.pattern
                            });
                        }
                        else
                        {
                            Log.Debuglog("no building at cell??");
                        }
                    }
                }

                stopWatch.Stop();
                Log.Debuglog($"Patch took {stopWatch.ElapsedMilliseconds} ms");
            }
        }

        internal class Blueprint_ReadJson_Patch
        {
            public static void Prefix(object __instance)
            {
                readingBlueprint = __instance;
            }
        }
    }

}
