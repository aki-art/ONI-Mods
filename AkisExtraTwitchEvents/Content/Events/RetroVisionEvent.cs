using FUtility;
using Twitchery.Content.Scripts;

namespace Twitchery.Content.Events
{
    public class RetroVisionEvent : ITwitchEvent
    {
        public const string ID = "RetroVision";

        public bool Condition(object data) => true;

        public string GetID() => ID;

        public void Run(object data)
        {
            if(AETE_DitherPostFx.Instance == null)
            {
                Log.Warning("AETE_CameraPostFx.Instance is null.");
                return;
            }

            AETE_DitherPostFx.Instance.DoDither();

            ONITwitchLib.ToastManager.InstantiateToast("Retro Vision", "Your video card has been downgraded.");
        }
    }
}
