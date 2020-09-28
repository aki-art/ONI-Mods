using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace InteriorDecorationv1.Buildings.Aquarium
{
    class FakeFish : KMonoBehaviour
    {
        [SerializeField]
        public float age;
        [SerializeField]
        public string KPrefabID;

        protected override void OnSpawn()
        {
            base.OnSpawn();
        }
    }
}
