/*using FUtility;

namespace CrittersDropBones.Integration.TwitchIntegration.Commands
{
    public class SoupRainCommand
    {
        public const string ID = "CDB_SoupRain";

        public static bool Condition()
        {
            return true;
        }

        public static void Run()
        {
            var cell = OniTwitchLib.OniTwitchUtil.ClampedMouseWorldPos();

            var soup = Utils.Spawn(Items.FishSoupConfig.ID, cell);
            soup.SetActive(true);
            Utils.YeetRandomly(soup, true, 3, 10, false);
        }
    }
}
*/