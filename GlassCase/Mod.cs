using FUtility;
using GlassCase.Buildings;
using HarmonyLib;
using KMod;
using System.Collections.Generic;
using UnityEngine;

namespace GlassCase
{
    public class Mod : UserMod2
    {
        public const string ID = "GlassCase";
        public static string Prefix(string name) => $"{ID}_{name}";

        public static Dictionary<int, GlassCasePiece> glassCaseElements = new Dictionary<int, GlassCasePiece>();

        public static Components.Cmps<GlassCaseValve> GlassCaseValves = new Components.Cmps<GlassCaseValve>();

        public static bool FindGlassCaseElementOnCell(int cell, out GlassCasePiece glassCaseElement)
        {
            var glassCase = Grid.Objects[cell, (int)ObjectLayer.FoundationTile];
            if (glassCase is GameObject && glassCase.TryGetComponent(out GlassCasePiece gc))
            {
                glassCaseElement = gc;
                return true;
            }

            glassCaseElement = null;
            return false;
        }

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
        }
    }
}
