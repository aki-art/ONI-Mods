using KSerialization;
using System;
using UnityEngine;

namespace Beached.Buildings.SeashellChime
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class Chime : KMonoBehaviour, ISim200ms
    {
        [MyCmpReq]
        private Operational operational;

        [Serialize]
        private float previousPressure;

        [Serialize]
        private float lastChime;

        [SerializeField]
        public float minPressureChange;

        [SerializeField]
        public float minDelay;

        private int cell;

        protected override void OnSpawn()
        {
            base.OnSpawn();
            cell = Grid.PosToCell(this);
            FUtility.Log.Debuglog("PRESSURE: " + minPressureChange);
            minPressureChange = 4000;
            FUtility.Log.Debuglog("PREVIOUS: " + previousPressure);
            previousPressure = 4000;
        }

        public void Sim200ms(float dt)
        {
            if (!operational.IsOperational)
            {
                return;
            }

            var pressure = Grid.Mass[cell];

            if (Math.Abs(pressure - previousPressure) > minPressureChange)
            {
                var time = GameClock.Instance.GetTimeInCycles();
                if (time - lastChime < minDelay)
                {
                    DoChime();
                    lastChime = time;
                }
            }

            previousPressure = pressure;
        }

        private void DoChime()
        {
            AcousticDisturbance.Emit(gameObject, 3);
        }
    }
}
