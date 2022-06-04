using Klei;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace FUtility
{
    // just a bunch of random helper methods
    public abstract class ElementsBase
    {
        // Credit for enum patch: Heinermann
        private static readonly Dictionary<SimHashes, string> SimHashNameLookup = new Dictionary<SimHashes, string>();
        private static readonly Dictionary<string, object> ReverseSimHashNameLookup = new Dictionary<string, object>();

        public static SimHashes RegisterSimHash(string name)
        {
            var simHash = (SimHashes)Hash.SDBMLower(name);

            SimHashNameLookup.Add(simHash, name);
            ReverseSimHashNameLookup.Add(name, simHash);

            return simHash;
        }

        public static bool TryGetSimHash(string name, out object hash)
        {
            return !ReverseSimHashNameLookup.TryGetValue(name, out hash);
        }

        public static bool TryGetName(SimHashes hash, out string name)
        {
            return !SimHashNameLookup.TryGetValue(hash, out name);
        }

        public static Substance CreateSubstance(SimHashes id, string uiAnim, Element.State state, Color color)
        {
            return CreateSubstance(id, uiAnim, state, color, color, color);
        }

        public static Substance CreateSubstance(SimHashes id, string uiAnim, Element.State state, Color color, Color uiColor, Color conduitColor)
        {
            var animFile = global::Assets.Anims.Find(anim => anim.name == uiAnim);
            var material = GetMaterialForState(state);

            return ModUtil.CreateSubstance(id.ToString(), state, animFile, material, color, uiColor, conduitColor);
        }

        private static Material GetMaterialForState(Element.State state)
        {
            // (gases use liquid material)
            var material = state == Element.State.Solid ? global::Assets.instance.substanceTable.solidMaterial : global::Assets.instance.substanceTable.liquidMaterial;
            return new Material(material);
        }

        public static Material SetTextures(SimHashes id, Material newMaterial, string folder, string texture, string spec = null)
        {
            var tex = Assets.LoadTexture(texture, folder);

            return SetTextures(id, newMaterial, tex, spec.IsNullOrWhiteSpace() ? null : Assets.LoadTexture(spec, folder));
        }

        public static Material SetTextures(SimHashes id, Material newMaterial, Texture2D texture, Texture2D spec = null)
        {
            var substance = ElementLoader.FindElementByHash(id).substance;

            if (newMaterial != null)
            {
                substance.material = new Material(newMaterial);
            }

            substance.material.mainTexture = texture;

            if (spec != null)
            {
                substance.material.SetTexture("_ShineMask", spec);
            }

            return substance.material;
        }

        public static void ReadYAML(string path, ref List<ElementLoader.ElementEntry> __result)
        {
            var elementListText = ReadText(path);

            if (elementListText.IsNullOrWhiteSpace())
            {
                return;
            }

            var elementList = YamlIO.Parse<ElementLoader.ElementEntryCollection>(elementListText, new FileHandle());
            __result.AddRange(elementList.elements);
        }

        private static string ReadText(string path)
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
            {
                Log.Warning($"Element configuration could not be read: {e.Message}\n" +
                    $"Elements by {Log.modName} will not be added to the game.");

                return null;
            }
        }
    }
}
