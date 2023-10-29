using FUtility;
using Twitchery.Content.Scripts;

namespace Twitchery.Content.Events.EventTypes
{
    public class RetroVisionEvent : ITwitchEvent
    {
        public const string ID = "RetroVision";

        public bool Condition(object data) => true;

        public int GetWeight() => TwitchEvents.Weights.COMMON;

        public string GetID() => ID;

        public void Run(object data)
        {
            if (AETE_DitherPostFx.Instance == null)
            {
                Log.Warning("AETE_CameraPostFx.Instance is null.");
                return;
            }

            AETE_DitherPostFx.Instance.DoDither();

            ONITwitchLib.ToastManager.InstantiateToast(
                STRINGS.AETE_EVENTS.RETRO_VISION.TOAST,
                STRINGS.AETE_EVENTS.RETRO_VISION.DESC);
        }
    }
}
