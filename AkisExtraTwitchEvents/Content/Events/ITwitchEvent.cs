namespace Twitchery.Content.Events
{
    public interface ITwitchEvent
    {
        public string GetID();

        public void Run(object data);

        public bool Condition(object data);

        public int GetWeight();
    }
}
