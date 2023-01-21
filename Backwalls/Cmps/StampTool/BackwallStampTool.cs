/*using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Backwalls.Cmps.StampTool
{
    public class BackwallStampTool : InterfaceTool
    {
        public const string NAME = "DecorPackB_BackwallStamperTool";

        public static BackwallStampTool Instance;

        private Orientation orientation = Orientation.Neutral;

        [Serialize]
        public List<Tag> selectedElements;

        [SerializeField]
        public List<Tag> defaultElements;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            viewMode = OverlayModes.None.ID;
            Instance = this;
        }

        public static void DestroyInstance()
        {
            Instance = null;
        }

        protected override void OnActivateTool()
        {
            orientation = Orientation.Neutral;

            if (selectedElements == null)
            {
                selectedElements = new List<Tag>(defaultElements);
            }

            PlayerController.Instance.ActivateTool(this);

            SanitizeSelectedElements();
            base.OnActivateTool();
        }

        private void SanitizeSelectedElements()
        {
            for (var i = 0; i < selectedElements.Count; i++)
            {
                var element = selectedElements[i];
                if (Assets.TryGetPrefab(element) == null)
                {
                    selectedElements[i] = defaultElements[i];
                }
            }
        }
    }
}
*/