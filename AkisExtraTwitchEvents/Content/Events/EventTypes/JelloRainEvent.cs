using ONITwitchLib;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
    public class JelloRainEvent : ITwitchEvent
    {
        public const string ID = "JelloRain";

        public int GetWeight() => TwitchEvents.Weights.COMMON;

        public bool Condition(object data) => true;

        public string GetID() => ID;

        public void Run(object data)
        {
            var go = new GameObject("jello cloud spawner");

            var rain = go.AddComponent<LiquidRainSpawner>();

            var minKcal = 30000;
            var maxKcal = 60000;

            rain.totalAmountRangeKg = (minKcal / TFoodInfos.JELLO_KCAL_PER_KG, maxKcal / TFoodInfos.JELLO_KCAL_PER_KG);
            rain.durationInSeconds = 240;
            rain.dropletMassKg = 0.01f;
            rain.elementId = Elements.Jello;
            rain.spawnRadius = 10;
            go.SetActive(true);

            GameScheduler.Instance.Schedule("jello rain", 3f, _ =>
            {
                rain.StartRaining();
                AudioUtil.PlaySound(ModAssets.Sounds.SPLAT, ModAssets.GetSFXVolume() * 0.15f); // its loud
            });

            ToastManager.InstantiateToast(
                STRINGS.AETE_EVENTS.JELLO_RAIN.TOAST,
                STRINGS.AETE_EVENTS.JELLO_RAIN.DESC);
        }
    }
}
