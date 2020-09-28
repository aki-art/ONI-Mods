using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace InteriorDecorationv1.Buildings.Aquarium
{
    class FishSideScreen : SideScreenContent
    {
        public override bool IsValidForTarget(GameObject target) => target.GetComponent<Aquarium>() != null;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            titleKey = "HOW IS THE FISH DOING?";
        }
    }
}
