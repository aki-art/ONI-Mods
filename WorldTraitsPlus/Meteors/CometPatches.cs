using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace WorldTraitsPlus.Meteors
{
    class CometPatches
    {
        [HarmonyPatch(typeof(Comet), "Explode")]
        public static class Comet_Explode_Patch
        {
            // Make ice comets leave behind both snow and crushed ice
            public static void Prefix(Comet __instance, ref Element element)
            {
                if (__instance.gameObject.name == IceCometConfig.ID && UnityEngine.Random.value > 0.5f)
                    element = ElementLoader.GetElement(SimHashes.Snow.CreateTag());
            }

           

            // spawns random resource for space debris
            public static void Postfix(Comet __instance, Vector3 pos, Vector2 ___velocity)
            {
                if (__instance.gameObject.name == SpaceDebrisConfig.ID && UnityEngine.Random.value <= Tuning.SpaceDebris.itemChance)
                {
                    Vector2 vector = -___velocity.normalized;
                    Vector2 a = new Vector2(vector.y, -vector.x);
                    Vector2 normalized = (vector + a * UnityEngine.Random.Range(-1f, 1f)).normalized;
                    Vector3 velocity = normalized * UnityEngine.Random.Range(__instance.explosionSpeedRange.x, __instance.explosionSpeedRange.y);
                    Vector3 targetPos = normalized * 0.75f;
                    targetPos += new Vector3(0f, 0.55f, 0f) + pos;

                    var prefab = Assets.GetPrefab(ProcGen.WeightedRandom.Choose(Tuning.SpaceDebris.items, WorldEventManager.Instance.random).name);
                    GameObject go = Util.KInstantiate(prefab, targetPos);
                    go.SetActive(true);
                    GameComps.Fallers.Add(go, velocity);
                }
            }
        }


        // Add custom meteor seasons
        [HarmonyPatch(typeof(SeasonManager), "OnSpawn")]
        public static class SeasonManager_OnSpawn_Patch
        {
            public static void Postfix(ref Dictionary<string, Season> ___seasons, ref string[] ___SeasonLoop)
            {
                Season meteorShowerIce = new Season
                {
                    name = "MeteorShowerIce",
                    durationInCycles = 4,
                    secondsBombardmentOff = new MathUtil.MinMax(800f, 1200f),
                    secondsBombardmentOn = new MathUtil.MinMax(50f, 100f),
                    secondsBetweenBombardments = new MathUtil.MinMax(0.3f, 0.5f),
                    meteorBackground = true,
                    bombardmentInfo = new BombardmentInfo[]
                    {
                            new BombardmentInfo
                            {
                                prefab = IceCometConfig.ID,
                                weight = 2f
                            }
                    }
                };


                Season meteorShowerAbyssalite = new Season
                {
                    name = "MeteorShowerAbyssalite",
                    durationInCycles = 4,
                    secondsBombardmentOff = new MathUtil.MinMax(800f, 1200f),
                    secondsBombardmentOn = new MathUtil.MinMax(50f, 100f),
                    secondsBetweenBombardments = new MathUtil.MinMax(0.3f, 0.5f),
                    meteorBackground = true,
                    bombardmentInfo = new BombardmentInfo[]
                    {
                            new BombardmentInfo
                            {
                                prefab = AbyssaliteCometConfig.ID,
                                weight = 1f
                            },

                            new BombardmentInfo
                            {
                                prefab = TungstenCometConfig.ID,
                                weight = 0.1f
                            }
                    }
                };

                ___seasons.Add("MeteorShowerIce", meteorShowerIce);
                ___seasons.Add("MeteorShowerAbyssalite", meteorShowerAbyssalite);

                var loop = new List<string>(___SeasonLoop);
                loop.Insert(0, "MeteorShowerIce");
                loop.Insert(0, "MeteorShowerAbyssalite");

                ___SeasonLoop = loop.ToArray();
            }
        }

        public struct BombardmentInfo
        {
            public string prefab;
            public float weight;
        }

        public struct Season
        {
            public string name;
            public int durationInCycles;
            public MathUtil.MinMax secondsBombardmentOff;
            public MathUtil.MinMax secondsBombardmentOn;
            public MathUtil.MinMax secondsBetweenBombardments;
            public bool meteorBackground;
            public BombardmentInfo[] bombardmentInfo;
        }

        // Changes the ice comets trail to be steam
        [HarmonyPatch(typeof(Comet), "Sim33ms")]
        public static class Comet_Sim33ms_Patch
        {
            public static IEnumerable<CodeInstruction> Transpiler(ILGenerator generator, IEnumerable<CodeInstruction> orig)
            {
                var codes = orig.ToList();
                var index = codes.FindIndex(ci => ci.operand is int i && i == (int)SimHashes.CarbonDioxide);
                var gameObject = AccessTools.Property(typeof(Comet), "gameObject").GetGetMethod();
                var name = AccessTools.Property(typeof(GameObject), "name").GetGetMethod();
                var stringEquals = AccessTools.Method(typeof(string), "Equals", new Type[] { typeof(string) });

                var COLabel = generator.DefineLabel();
                var startLabel = generator.DefineLabel();

                codes[index].labels.Add(COLabel);
                codes[index + 1].labels.Add(startLabel);
                codes.Insert(index++, new CodeInstruction(OpCodes.Ldarg_0));
                codes.Insert(index++, new CodeInstruction(OpCodes.Call, gameObject));
                codes.Insert(index++, new CodeInstruction(OpCodes.Call, name));
                codes.Insert(index++, new CodeInstruction(OpCodes.Ldstr, IceCometConfig.ID));
                codes.Insert(index++, new CodeInstruction(OpCodes.Call, stringEquals));
                codes.Insert(index++, new CodeInstruction(OpCodes.Brfalse, COLabel));
                codes.Insert(index++, new CodeInstruction(OpCodes.Ldc_I4, (int)SimHashes.Steam));
                codes.Insert(index++, new CodeInstruction(OpCodes.Br, startLabel));
                /*
                                foreach (var codeInstruction in codes)
                                {
                                    Log.Debuglog(codeInstruction);
                                }*/

                return codes;
            }
        }
    }
}