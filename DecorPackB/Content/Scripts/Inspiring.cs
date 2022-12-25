using Database;
using FUtility;
using Klei.AI;
using UnityEngine;

namespace DecorPackB.Content.Scripts
{
    public class Inspiring : KMonoBehaviour
    {
        private EmoteReactable reactable;

        [MyCmpReq]
        private Exhibition exhibition;

        protected override void OnSpawn()
        {
            base.OnSpawn();
            Subscribe((int)ModHashes.FossilStageSet, OnStageChanged);
        }

        private void OnStageChanged(object obj)
        {
            if (obj is ArtableStatuses.ArtableStatusType stage)
            {
                if (stage != ArtableStatuses.ArtableStatusType.AwaitingArting)
                {
                    CreateReactable(stage);
                }
                else
                {
                    RemoveReactable();
                }
            }
        }

        private void RemoveReactable()
        {
            if (reactable != null)
            {
                reactable.Cleanup();
            }

            reactable = null;
        }

        private Reactable CreateReactable(ArtableStatuses.ArtableStatusType statusItem)
        {
            Log.Debuglog("created reacatable");

            reactable = new EmoteReactable(
                gameObject,
                "DecorPackB_Reactable_Inspired",
                Db.Get().ChoreTypes.Emote,
                5,
                2);

            reactable.SetEmote(Db.Get().Emotes.Minion.ThumbsUp);
            reactable.RegisterEmoteStepCallbacks("react", go => OnEmote(go, statusItem), null);
            reactable.AddPrecondition(ReactorIsOnFloor);
            reactable.preventChoreInterruption = true;

            return reactable;
        }

        private bool ReactorIsOnFloor(GameObject reactor, Navigator.ActiveTransition transition)
        {
            return transition.end == NavType.Floor;
        }

        private void OnEmote(GameObject obj, ArtableStatuses.ArtableStatusType status)
        {
            Log.Debuglog("EMOTED");

            switch (status)
            {
                case ArtableStatuses.ArtableStatusType.LookingUgly:
                    AddReactionEffect(obj, ModDb.Effects.INSPIRED_LOW);
                    break;
                case ArtableStatuses.ArtableStatusType.LookingOkay:
                    AddReactionEffect(obj, ModDb.Effects.INSPIRED_OKAY);
                    break;
                case ArtableStatuses.ArtableStatusType.LookingGreat:
                    AddReactionEffect(obj, ModDb.Effects.INSPIRED_GREAT);
                    break;
            }
        }

        private void AddReactionEffect(GameObject reactor, string effect)
        {
            var effects = reactor.GetComponent<Effects>();

            var hasSmall = effects.HasEffect(ModDb.Effects.INSPIRED_LOW);
            var hasMedium = effects.HasEffect(ModDb.Effects.INSPIRED_OKAY);
            var hasSuper = effects.HasEffect(ModDb.Effects.INSPIRED_GREAT);

            switch (effect)
            {
                case ModDb.Effects.INSPIRED_LOW:
                    if (!hasMedium && !hasSuper)
                    {
                        effects.Add(ModDb.Effects.INSPIRED_LOW, true);
                    }

                    break;
                case ModDb.Effects.INSPIRED_OKAY:
                    effects.Remove(ModDb.Effects.INSPIRED_LOW);

                    if (!hasSuper)
                    {
                        effects.Add(ModDb.Effects.INSPIRED_OKAY, true);
                    }

                    break;
                case ModDb.Effects.INSPIRED_GREAT:
                    effects.Remove(ModDb.Effects.INSPIRED_LOW);
                    effects.Remove(ModDb.Effects.INSPIRED_OKAY);
                    effects.Add(ModDb.Effects.INSPIRED_GREAT, true);
                    break;
                default:
                    Log.Warning($"Something went wrong trying to add an Inspired Reaction effect. Effect ({effect}) is invalid.");
                    break;
            };
        }
    }
}
