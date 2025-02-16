/*using FUtility;
using ONITwitchLib;
using System;
using System.Collections;
using System.Reflection;
using Twitchery.Content.Scripts;

namespace Twitchery.Content.Events.EventTypes
{
	public class AllOfTheOthersEvent : TwitchEventBase
	{
		public const string ID = "AllOfTheOthers";

		private readonly FieldInfo f_votes;
		private readonly PropertyInfo p_CurrentVote;
		private readonly PropertyInfo p_EventId;
		private readonly PropertyInfo p_EventNamespace;
		private readonly PropertyInfo p_eventInfo;

		private readonly object voteController;

		public override bool Condition() => !Mod.isTimerHere && AkisTwitchEvents.VotesPerTurn > 2;

		public AllOfTheOthersEvent() : base(ID)
		{
			var t_VoteController = Type.GetType("ONITwitch, ONITwitch.Voting.VoteController");

			if (t_VoteController == null)
			{
				Log.Warning("t_VoteController is null");
				return;
			}

			var f_Instance = t_VoteController.GetField("Instance");

			if (f_Instance == null)
			{
				Log.Warning("f_Instance is null");
				return;
			}

			voteController = f_Instance.GetValue(null);
			p_CurrentVote = t_VoteController.GetProperty("CurrentVote");
			p_EventId = ONITwitchLib.Core.CoreTypes.EventInfoType.GetProperty("EventId");
			p_EventNamespace = ONITwitchLib.Core.CoreTypes.EventInfoType.GetProperty("EventNamespace");

			var t_Vote = Type.GetType("ONITwitch, ONITwitch.Voting.Vote");

			if (t_Vote == null)
			{
				Log.Warning("t_Vote is null");
				return;
			}
			f_votes = t_Vote.GetField("votes");

			var t_VoteCount = Type.GetType("ONITwitch, ONITwitch.Voting.Vote.VoteCount");

			if (t_VoteCount == null)
			{
				Log.Warning("t_VoteCount is null");
				return;
			}
			p_eventInfo = t_VoteCount.GetProperty("t_VoteCount");
		}

		public override Danger GetDanger() => Danger.None;

		public override int GetWeight() => Consts.EventWeight.Rare;

		public override void Run()
		{
			if (!Condition())
			{
				ToastManager.InstantiateToast("Warning", "This event requires an active vote.");
				//return;
			}

			var currentVote = p_CurrentVote.GetValue(voteController);

			if (f_votes.GetValue(currentVote) is not IList votes)
			{
				Log.Warning("Null votes");
				return;
			}

			foreach (var vote in votes)
			{
				Log.Debug("processing votes");
				var eventInfo = p_eventInfo.GetValue(vote);

				var eventId = p_EventId.GetValue(eventInfo) as string;
				var eventNamespace = p_EventNamespace.GetValue(eventInfo) as string;

				if (eventNamespace == "Twitchery" && eventId == ID)
					continue;

				Log.Debug($"event: {eventNamespace}.{eventId}");
				var ev = EventManager.Instance.GetEventByID(eventNamespace, eventId);

				var data = DataManager.Instance.GetDataForEvent(ev);
				ev.Trigger(data);
			}
		}
	}
}
*/