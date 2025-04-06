using Database;
using DecorPackB.Content.ModDb;
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

		public override void OnSpawn()
		{
			base.OnSpawn();
			Subscribe((int)DPIIHashes.FossilStageSet, OnStageChanged);
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
				global::Db.Get().ChoreTypes.Emote,
				5,
				2);

			reactable.SetEmote(global::Db.Get().Emotes.Minion.ThumbsUp);
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
					AddReactionEffect(obj, DPEffects.INSPIRED_LOW);
					break;
				case ArtableStatuses.ArtableStatusType.LookingOkay:
					AddReactionEffect(obj, DPEffects.INSPIRED_OKAY);
					break;
				case ArtableStatuses.ArtableStatusType.LookingGreat:
					AddReactionEffect(obj, DPEffects.INSPIRED_GREAT);
					break;
			}
		}

		private void AddReactionEffect(GameObject reactor, string effect)
		{
			var effects = reactor.GetComponent<Effects>();

			var hasSmall = effects.HasEffect(DPEffects.INSPIRED_LOW);
			var hasMedium = effects.HasEffect(DPEffects.INSPIRED_OKAY);
			var hasSuper = effects.HasEffect(DPEffects.INSPIRED_GREAT);

			switch (effect)
			{
				case DPEffects.INSPIRED_LOW:
					if (!hasMedium && !hasSuper)
					{
						effects.Add(DPEffects.INSPIRED_LOW, true);
					}

					break;
				case DPEffects.INSPIRED_OKAY:
					effects.Remove(DPEffects.INSPIRED_LOW);

					if (!hasSuper)
					{
						effects.Add(DPEffects.INSPIRED_OKAY, true);
					}

					break;
				case DPEffects.INSPIRED_GREAT:
					effects.Remove(DPEffects.INSPIRED_LOW);
					effects.Remove(DPEffects.INSPIRED_OKAY);
					effects.Add(DPEffects.INSPIRED_GREAT, true);
					break;
				default:
					Log.Warning($"Something went wrong trying to add an Inspired Reaction effect. Effect ({effect}) is invalid.");
					break;
			};
		}
	}
}
