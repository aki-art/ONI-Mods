using FUtility.SaveData;
using System.Collections.Generic;

namespace MoreSmallSculptures
{
	public class Config : IUserSetting
	{
		public Dictionary<string, string> MoveSculptures { get; set; } = new Dictionary<string, string>();
	}
}
