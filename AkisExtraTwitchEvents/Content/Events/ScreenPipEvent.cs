/*using Twitchery.Content.Scripts;

namespace Twitchery.Content.Events
{
    public class ScreenPipEvent : ITwitchEvent
    {
        public const string ID = "ScreenPip";

        public bool Condition(object data) => !AETEScreenPipmanager.Instance.HasActivePip;

        public string GetID() => ID;

        public void Run(object data)
        {
            AETEScreenPipmanager.Instance.CreatePip();
        }
    }
}
*/