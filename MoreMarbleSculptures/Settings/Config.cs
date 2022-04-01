using FUtility.SaveData;
using System.Collections.Generic;

namespace MoreMarbleSculptures.Settings
{
    public class Config : IUserSetting
    {
        public Dictionary<string, Artable.Status> MoveSculptures { get; set; } = new Dictionary<string, Artable.Status>()
        {
            { "Average", Artable.Status.Great }, // Unicorn
            { "Bad", Artable.Status.Okay } // Mushroom
        };
    }
}
