using FUtility.SaveData;
using System;
using System.Collections.Generic;

namespace MayISit
{
    public class Config : IUserSetting
    {
        public Dictionary<string, SeatEntry[]> Chairs { get; set; } = new Dictionary<string, SeatEntry[]>()
        {
            {
                "PropFacilityCouch",
                new[]
                {
                    new SeatEntry(1, 0),
                    new SeatEntry(0, 0),
                    new SeatEntry(-1, 0)
                }
            },
            {
                "PropFacilityChair",
                new[]
                {
                    new SeatEntry(0, 0, SeatEntry.FacingDirection.Left),
                }
            },
            {
                "PropFacilityChairFlip",
                new[]
                {
                    new SeatEntry(0, 0, SeatEntry.FacingDirection.Right),
                }
            }
        };

        [Serializable]
        public class SeatEntry
        {
            public float X { get; set; }

            public float Y { get; set; }

            public FacingDirection Facing { get; set; }

            public SeatEntry(float x, float y, FacingDirection facing = FacingDirection.Either)
            {
                X = x;
                Y = y;
                Facing = facing;
            }

            public enum FacingDirection
            {
                Left,
                Right,
                Either
            }
        }
    }
}
