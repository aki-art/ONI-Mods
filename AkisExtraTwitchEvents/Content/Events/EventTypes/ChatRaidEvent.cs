using HarmonyLib;
using ONITwitchLib;
using ONITwitchLib.Core;
using ONITwitchLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class ChatRaidEvent() : TwitchEventBase(ID)
	{
		public const string ID = "ChatRaid";

		public override Danger GetDanger() => Danger.None;

		public override int GetWeight() => Consts.EventWeight.Common;

		public override bool Condition() => Components.LiveMinionIdentities.Items.Count < Mod.Settings.MaxDupes - 3;

		private static List<string> placeholderNames;

		public override void Run()
		{
			Log.Debug(TwitchSettings.GetSettingsDictionary()["DisallowedDupeNames"].GetType());

			var dupeCount = Components.LiveMinionIdentities.Items.Count;

			var maxDupesToSpawn = Mod.Settings.MaxDupes - dupeCount;
			var previousDupeCount = dupeCount;

			for (int i = 0; i < maxDupesToSpawn; i++)
			{
				if (previousDupeCount == Components.LiveMinionIdentities.Items.Count)
					break;
			}

			var voteController = Type.GetType("ONITwitch.Voting.VoteController, ONITwitch");

			if (voteController == null)
			{
				ToastManager.InstantiateToast("", "Something went wrong. :(");
				Log.Warning("Could not proceed with Chat raid event, VoteController class not found");
				return;
			}

			// static field
			var p_Instance = AccessTools.Field(voteController, "Instance");
			// instanced property
			var p_CurrentVote = AccessTools.Property(voteController, "CurrentVote");
			var t_Vote = Type.GetType("ONITwitch.Voting.Vote, ONITwitch");
			var m_GetUserVotes = AccessTools.Method(t_Vote, "GetUserVotes", []);
			var instance = p_Instance.GetValue(null);
			var currentVote = p_CurrentVote.GetValue(instance);

			if (currentVote == null)
			{
				ToastManager.InstantiateToast(
					"Twitch Connection Error",
					 "Have these guys instead.\n\n(No current ongoing vote.)");

				SendMadeUpChatters();

				return;
			}

			var votes = m_GetUserVotes.Invoke(currentVote, []);

			if (votes is not System.Collections.IEnumerable collection)
			{
				return;
			}

			var disallowedDupeNamesJArray = TwitchSettings.GetSettingsDictionary()["DisallowedDupeNames"] as Newtonsoft.Json.Linq.JArray;
			var disallowedDupeNames = disallowedDupeNamesJArray?.ToObject<List<string>>();

			if (disallowedDupeNames == null)
				Log.Debug($"DisallowedDupeNames is null {TwitchSettings.GetSettingsDictionary().Keys.Join()}");

			var spawnQueue = new List<(string name, Color32? color)>();

			foreach (var voter in collection)
			{
				var entryType = voter.GetType();
				var keyProp = entryType.GetProperty("Key");
				var key = keyProp?.GetValue(voter);

				var p_DisplayName = key.GetType().GetProperty("DisplayName");
				var p_NameColor = key.GetType().GetProperty("NameColor");

				if (p_DisplayName.GetValue(key) is not string name
					|| (disallowedDupeNames != null && disallowedDupeNames.Contains(name)))
					continue;

				spawnQueue.Add((name, p_NameColor.GetValue(key) as Color32?));
			}

			Vector3 pos;

			if (spawnQueue.Count == 0)
			{
				ToastManager.InstantiateToast(
					"Twitch Connection Error",
					 " Chat sent this Meep instead, chat sent him.\n\n(Empty user list.)");

				SendMeep();
				return;
			}

			// First try to find a printing pod, since that should always be in a safe location.
			var pods = Components.Telepads.Items;
			if (pods.Count > 0)
			{
				pos = pods.GetRandom().transform.position;
			}
			else
			{
				Log.Debug("Unable to find any Telepads, using a random dupe's location instead");
				pos = Components.LiveMinionIdentities.items.GetRandom().transform.position;
			}

			var cell = Grid.PosToCell(pos);

			foreach (var (name, color) in spawnQueue)
			{
				SpawnDupe(cell, name, color);
			}

			GameScheduler.Instance.ScheduleNextFrame("toast", _ =>
			{
				ToastManager.InstantiateToast(
					STRINGS.AETE_EVENTS.CHATRAID.TOAST,
					 STRINGS.AETE_EVENTS.CHATRAID.DESC);
			});
		}

		private void GatherPlaceholderNames()
		{
			if (placeholderNames != null)
				return;

			placeholderNames = [];
			var flags = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static;

			var names = typeof(global::STRINGS.NAMEGEN.DUPLICANT.NAME.NB).GetFields(flags)
				.Union(typeof(global::STRINGS.NAMEGEN.DUPLICANT.NAME.MALE).GetFields(flags))
				.Union(typeof(global::STRINGS.NAMEGEN.DUPLICANT.NAME.FEMALE).GetFields(flags));

			placeholderNames = [.. names.Select(n => n.GetValue(null) as LocString)];
		}

		private void SendMadeUpChatters()
		{
			var cell = GetActiveCell();
			GatherPlaceholderNames();

			for (int i = 0; i < UnityEngine.Random.Range(2, 5); i++)
			{
				SpawnDupe(cell, placeholderNames.Count == 0 ? null : placeholderNames.GetRandom(), null);
			}
		}

		private void SendMeep()
		{
			var cell = GetActiveCell();
			SpawnDupe(cell, "Meep", null, "MEEP");
		}

		private static int GetActiveCell()
		{
			Vector3 pos;
			var pods = Components.Telepads.Items;
			if (pods.Count > 0)
			{
				pos = pods.GetRandom().transform.position;
			}
			else
			{
				Log.Debug("Unable to find any Telepads, using a random dupe's location instead");
				pos = Components.LiveMinionIdentities.items.GetRandom().transform.position;
			}

			return Grid.PosToCell(pos);
		}

		private void SpawnDupe(int cell, string name, Color32? color, string setPersonality = null)
		{
			var prefab = Assets.GetPrefab(MinionConfig.ID);
			var gameObject = Util.KInstantiate(prefab);
			gameObject.name = prefab.name;
			Immigration.Instance.ApplyDefaultPersonalPriorities(gameObject);
			Vector3 posCbc = Grid.CellToPosCBC(cell, Grid.SceneLayer.Move);
			gameObject.transform.SetLocalPosition(posCbc);
			gameObject.SetActive(true);

			switch (name)
			{
				case "asquared31415":
					setPersonality = "NAILS";
					break;
				case "Aki_Senkinn":
					setPersonality = "GOSSMANN";
					break;
				case "Sgt_Imalas":
					setPersonality = "NIKOLA";
					break;
			}

			if (setPersonality != null)
			{
				var personalities = Db.Get().Personalities;

				var personality = personalities.TryGet(setPersonality);
				if (personality == null || !Game.IsDlcActiveForCurrentSave(personality.requiredDlcId))
				{
					personality = personalities.GetRandom(GameTags.Minions.Models.Standard, true, false);
				}

				new MinionStartingStats(personality).Apply(gameObject);
			}
			else
			{
				new MinionStartingStats(GameTags.Minions.Models.Standard, false).Apply(gameObject);
			}

			gameObject.GetMyWorld().SetDupeVisited();

			var useTwitchNameColors = (bool)TwitchSettings.GetSettingsDictionary()["UseTwitchNameColors"];
			var col = color ?? ColorUtil.GetRandomTwitchColor();

			if (gameObject.TryGetComponent(out MinionIdentity identity) && name != null)
			{
				if (useTwitchNameColors)
					identity.SetName($"<color=#{col.ToHexString()}>{name}</color>");
				else
					identity.SetName(name);

				if (name == "Aki_Senkinn")
					identity.SetGender("MALE");
			}


			if (gameObject.TryGetComponent(out AETE_MinionStorage storage))
			{
				Color32? hairColor = null;

				if (useTwitchNameColors)
				{
					hairColor = col;
					if (name == "Aki_Senkinn")
						hairColor = Util.ColorFromHex("d5deeb");
				}

				storage.TwitchBorn(hairColor);
			}
		}
	}
}
