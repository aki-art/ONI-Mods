using SketchPad.History;
using System.Collections.Generic;
using UnityEngine;

namespace SketchPad
{
    public class SketchBook : KMonoBehaviour
    {
        public HashSet<HashedString> enabledOverlayModes;
        // store data here

        EditHistory history;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            enabledOverlayModes = new HashSet<HashedString>();
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();
        }
    }
}
