using FUtility.SaveData;

namespace DecorPackB.Settings
{
	public class Config : IUserSetting
	{
		public bool FunctionalFossils { get; set; }
		public float PotCapacity { get; set; } = 5000;
	}
}
