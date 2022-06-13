using FUtility.SaveData;

namespace Kigurumis
{
    public class Config : IUserSetting
    {
        public HoodieState HoodieDefaultState { get; set; } = HoodieState.AlwaysOn;

        public enum HoodieState
        {
            AlwaysOn,
            HatsHavePriority,
            Never
        }
    }
}
