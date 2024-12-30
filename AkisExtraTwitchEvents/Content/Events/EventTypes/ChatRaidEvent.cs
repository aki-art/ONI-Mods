using FUtility;
using HarmonyLib;
using ONITwitchLib;
using ONITwitchLib.IRC;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class ChatRaidEvent() : TwitchEventBase(ID)
	{
		public const string ID = "ChatRaid";

		public override Danger GetDanger() => Danger.None;

		public override int GetWeight() => 99999;// Consts.EventWeight.Uncommon;

		public override bool Condition() => Components.LiveMinionIdentities.Items.Count < Mod.Settings.MaxDupes - 3;

		public override void Run()
		{
			Log.Debug(0);

			var dupeCount = Components.LiveMinionIdentities.Items.Count;

			var maxDupesToSpawn = Mod.Settings.MaxDupes - dupeCount;
			var previousDupeCount = dupeCount;

			Log.Debug(1);
			for (int i = 0; i < maxDupesToSpawn; i++)
			{
				if (previousDupeCount == Components.LiveMinionIdentities.Items.Count)
					break;


			}
			var voteController = Type.GetType("ONITwitch.Voting.VoteController, ONITwitch");

			Log.Debug(2);
			if (voteController == null)
			{
				ToastManager.InstantiateToast("", "Something went wrong. :(");
				Log.Warning("Could not proceed with Chat raid event, VoteController class not found");
				return;
			}


			var p_Instance = AccessTools.Field(voteController, "Instance");
			var p_CurrentVote = AccessTools.Property(voteController, "CurrentVote");
			var t_Vote = Type.GetType("ONITwitch.Voting.Vote, ONITwitch");
			Log.Assert("t_Vote", t_Vote);
			Log.Debug(t_Vote.GetType().Name);
			var m_GetUserVotes = AccessTools.Method(t_Vote, "GetUserVotes", []);

			Log.Debug(4);

			Log.Assert("p_Instance", p_Instance);
			Log.Assert("p_CurrentVote", p_CurrentVote);
			Log.Assert("m_GetUserVotes", m_GetUserVotes);

			var instance = p_Instance.GetValue(null);
			Log.Assert("instance", instance);

			var currentVote = p_CurrentVote.GetValue(instance);
			Log.Assert("currentVote", currentVote);

			var votes = m_GetUserVotes.Invoke(currentVote, []);
			Log.Assert("votes", votes);

			var collection = votes as IReadOnlyDictionary<object, int>;

			Log.Assert("collection", collection);

			if (collection == null || collection.Count == 0)
			{
				Log.Warning("Empty votes");
				return;
			}

			foreach (var voter in collection.Keys)
			{
				var casted = (TwitchUserInfo)voter;
				SpawnDupe(casted.DisplayName, casted.NameColor);
			}

			//	var votes = VoteController.Instance.CurrentVote.GetUserVotes().ToList();
			ToastManager.InstantiateToast(
				STRINGS.AETE_EVENTS.CHATRAID.TOAST,
				 STRINGS.AETE_EVENTS.CHATRAID.DESC);
		}

		private void SpawnDupe(string name, Color32? color)
		{
			Log.Debug($"Spawning a dupe:{name}");
		}
	}
}
