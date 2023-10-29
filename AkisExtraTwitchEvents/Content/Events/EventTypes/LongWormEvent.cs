/*using Twitchery.Content.Scripts;

namespace Twitchery.Content.Events
{
    public class LongWormEvent : ITwitchEvent
    {
        public const string ID = "LongWorm";
        public const float SPAWN_DELAY_CYCLES = 100;

        public bool Condition(object data)
        {
            if (!DlcManager.IsExpansion1Active())
            {
                return false;
            }

            return true; // AkisTwitchEvents.Instance.wormies == null || AkisTwitchEvents.Instance.wormies.Count < 4;
        }

        public string GetID() => ID;

        public void Run(object data)
        {
            int cell = ONITwitchLib.Utils.PosUtil.RandomCellNearMouse();

            for (int i = 0; i < 100; i++)
            {
                if (!Grid.Solid[cell])
                {
                    break;
                }

                cell = ONITwitchLib.Utils.PosUtil.RandomCellNearMouse();
            }

            LongWormy.SpawnLongWorm(cell);
        }
    }
}
*/