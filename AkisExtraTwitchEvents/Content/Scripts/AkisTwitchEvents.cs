using FUtility;
using KSerialization;
using System;
using Twitchery.Content.Defs;

namespace Twitchery.Content.Scripts
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class AkisTwitchEvents : KMonoBehaviour
    {
        public static AkisTwitchEvents Instance;

        [Serialize]
        public float lastLongBoiSpawn;
        internal bool hasRaddishSpawnedBefore;

        [Serialize]
        public bool hasUnlockedPizzaRecipe;

        public static string pizzaRecipeID;

        public AkisTwitchEvents()
        {
            lastLongBoiSpawn = float.PositiveInfinity;
        }

        public override void OnPrefabInit()
        {
            Instance = this;
        }

        public override void OnSpawn()
        {
            base.OnSpawn();
        }

        public override void OnCleanUp()
        {
            base.OnCleanUp();
            Instance = null;
        }

    }
}
