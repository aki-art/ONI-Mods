/*using System.Collections.Generic;
using UnityEngine;

namespace Twitchery.Content.Events
{
    public class ColorScrambleEvent : ITwitchEvent
    {
        public string GetID() => "ColorScramble";

        private readonly Dictionary<SimHashes, Colors> originalColors;

        public struct Colors
        {
            public Color main;
            public Color conduit;
        }

        public ColorScrambleEvent()
        {
            originalColors = new Dictionary<SimHashes, Colors>();

            var substanceTable = Assets.instance.substanceTable;
            foreach (var substance in substanceTable.GetList())
            {
                var element = ElementLoader.FindElementByHash(substance.elementID);
                if (element != null)
                {
                    originalColors[substance.elementID] = new Colors()
                    {
                        main = substance.colour,
                        conduit = substance.conduitColour
                    };
                }
            }
        }

        public void Run(object data)
        {
            var substanceTable = Assets.instance.substanceTable;

            foreach(var substance in substanceTable.GetList())
            {
                if(originalColors.ContainsKey(substance.elementID))
                {
                    var color = Random.ColorHSV();
                    substance.colour = color;
                    substance.conduitColour = color;
                }
            }

            GameScheduler.Instance.Schedule("Resetcolors", 30f, ResetColors);

            ONITwitchLib.ToastManager.InstantiateToast("sdsd", "sfsd");
        }

        private void ResetColors(object obj)
        {
            var substanceTable = Assets.instance.substanceTable;

            foreach (var colorData in originalColors)
            {
                var substance = substanceTable.GetSubstance(colorData.Key);
                if(substance != null)
                {
                    substance.colour = colorData.Value.main;
                    substance.conduitColour = colorData.Value.conduit;
                }
            }
        }

        public bool Condition(object data)
        {
            return true;
        }
    }
}
*/