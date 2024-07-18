using FUtility.SaveData;
using System.Collections.Generic;

namespace MoreMarbleSculptures.Settings
{
	public class Config : IUserSetting
	{
		public Dictionary<string, string> MoveSculptures { get; set; } = new Dictionary<string, string>()
		{
			{ "Average", "Great" }, // Unicorn
			{ "Bad", "Okay" } // Mushroom
		};
	}
}
