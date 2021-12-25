using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using static ComplexRecipe;
using Random = UnityEngine.Random;

namespace FUtility
{
    public class Utils
    {
        public static string ModPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        /// <summary> Spawns one entity by tag.</summary>
        public static GameObject Spawn(Tag tag, Vector3 position, Grid.SceneLayer sceneLayer = Grid.SceneLayer.Creatures, bool setActive = true)
        {
            var prefab = global::Assets.GetPrefab(tag);
            if (prefab == null) return null;
            var go = GameUtil.KInstantiate(global::Assets.GetPrefab(tag), position, sceneLayer);
            go.SetActive(setActive);
            return go;
        }

        /// <summary> Spawns one entity by tag. </summary>
        public static GameObject Spawn(Tag tag, GameObject atGO, Grid.SceneLayer sceneLayer = Grid.SceneLayer.Creatures, bool setActive = true)
        {
            return Spawn(tag, atGO.transform.position, sceneLayer, setActive);
        }

        public static void YeetRandomly(GameObject go, bool onlyUp, float minDistance, float maxDistance, bool rotate, bool stopOnLand = true)
        {
            var vec = Random.insideUnitCircle.normalized;
            if (onlyUp)
            {
                vec.y = Mathf.Abs(vec.y);
            }

            vec += new Vector2(0f, Random.Range(0, 1f));
            vec *= Random.Range(minDistance, maxDistance);

            Yeet(go, minDistance, rotate, vec, stopOnLand);
        }

        public static void YeetAtAngle(GameObject go, float angle, float distance, bool rotate, bool stopOnLand = true)
        {
            var vec = DegreeToVector2(angle) * distance;
            Yeet(go, distance, rotate, vec, stopOnLand);
        }

        private static void Yeet(GameObject go, float distance, bool rotate, Vector2 vec, bool stopOnLand = true)
        {
            if (GameComps.Fallers.Has(go))
                GameComps.Fallers.Remove(go);

            GameComps.Fallers.Add(go, vec);

            if (rotate)
            {
                Rotator rotator = go.AddOrGet<Rotator>();
                rotator.minDistance = distance;
                rotator.SetVec(vec);
                rotator.stopOnLand = stopOnLand;
            }
        }

        public static Vector2 RadianToVector2(float radian) => new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
        public static Vector2 DegreeToVector2(float degree) => RadianToVector2(degree * Mathf.Deg2Rad);

        public static ComplexRecipe AddRecipe(string fabricatorID, RecipeElement input, RecipeElement output, string desc, int sortOrder = 0, float time = 40f)
        {
            var i = new RecipeElement[] { input };
            var o = new RecipeElement[] { output };

            string recipeID = ComplexRecipeManager.MakeRecipeID(fabricatorID, i, o);

            var recipe = new ComplexRecipe(recipeID, i, o)
            {
                time = time,
                description = desc,
                nameDisplay = RecipeNameDisplay.IngredientToResult,
                fabricators = new List<Tag> { TagManager.Create(fabricatorID) }
            };

            return recipe;
        }

        public static ComplexRecipe AddRecipe(string fabricatorID, RecipeElement[] input, RecipeElement[] output, string desc, RecipeNameDisplay display = RecipeNameDisplay.IngredientToResult, int sortOrder = 0, float time = 40f)
        {
            string recipeID = ComplexRecipeManager.MakeRecipeID(fabricatorID, input, output);

            var recipe = new ComplexRecipe(recipeID, input, output)
            {
                time = time,
                description = desc,
                nameDisplay = display,
                fabricators = new List<Tag> { TagManager.Create(fabricatorID) }
            };

            return recipe;
        }
    }
}
