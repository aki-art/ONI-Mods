using ONITwitchLib;
using Twitchery.Content.Scripts;

namespace Twitchery.Content.Events.EventTypes
{
    public class InvisibleLiquidsEvent : ITwitchEvent
    {
        public bool Condition(object data) => true;

        public int GetWeight() => TwitchEvents.Weights.COMMON;

        public string GetID() => "InvisibleLiquids";

        public void Run(object data)
        {
            AkisTwitchEvents.Instance.hideLiquids = true;
            AkisTwitchEvents.Instance.ApplyLiquidTransparency(WaterCubes.Instance);

            GameScheduler.Instance.Schedule("test", ModTuning.INVISIBLE_LIQUIDS_DURATION, _ =>
            {
                AkisTwitchEvents.Instance.hideLiquids = false;
                AkisTwitchEvents.Instance.ApplyLiquidTransparency(WaterCubes.Instance);
            });

            ToastManager.InstantiateToast(
                STRINGS.AETE_EVENTS.INVISIBLE_LIQUIDS.TOAST,
                STRINGS.AETE_EVENTS.INVISIBLE_LIQUIDS.DESC);
        }
    }
}
