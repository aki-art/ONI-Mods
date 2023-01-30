using Twitchery.Content.Scripts;

namespace Twitchery.Content.Events
{
    public class LongWormEvent : ITwitchEvent
    {
        public const string ID = "LongWorm";
        public const float SPAWN_DELAY_CYCLES = 100;

        public bool Condition(object data)
        {
            if(!DlcManager.IsExpansion1Active())
            {
                return false;
            }

            var hasEnoughTimePassed = GameClock.Instance.GetTimeInCycles() > AkisTwitchEvents.Instance.lastLongBoiSpawn + SPAWN_DELAY_CYCLES;
            return hasEnoughTimePassed;
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

            var prefab = Assets.GetPrefab(DivergentWormConfig.ID);

            var go = GameUtil.KInstantiate(prefab, Grid.CellToPos(cell), Grid.SceneLayer.Creatures);
            go.GetComponent<KPrefabID>().AddTag(TTags.longBoi, true);
            go.SetActive(true);

            AkisTwitchEvents.Instance.lastLongBoiSpawn = GameClock.Instance.GetTimeInCycles();
        }
    }
}
