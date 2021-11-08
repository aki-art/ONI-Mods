using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DecorPackA.Buildings.Aquarium
{
    class FishReceptable : KMonoBehaviour
    {
        public GameObject Occupant { get; set; }

        [SerializeField]
        public Vector3 occupyingObjectRelativePosition = Vector3.zero;
    }
}
