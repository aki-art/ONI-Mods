using ONITwitchLib;
using System.Collections.Generic;
using System.Linq;
using Twitchery.Utils;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class LadderHatingBeesEvent() : TwitchEventBase(ID)
	{
		public const string ID = "LadderHatingBees";

		public override bool Condition() => Components.Ladders.items.Count > 5;

		public override Danger GetDanger() => Danger.High;

		public override int GetWeight() => Consts.EventWeight.Common;

		public override void Run()
		{
			var ladders = new List<Ladder>();
			foreach (var ladder in Components.Ladders.GetWorldItems(ClusterManager.Instance.activeWorldId))
			{
				if (AGridUtil.protectedCells.Contains(ladder.NaturalBuildingCell()))
					continue;

				if (ladder.HasTag(GameTags.Gravitas))
					continue;

				ladders.Add(ladder);
			}

			if (ladders.Count < 5)
			{
				ladders.Clear();
				foreach (var ladder in Components.Ladders.items)
				{
					ladders.Add(ladder);
				}
			}


			if (ladders.Count == 0)
			{
				ToastManager.InstantiateToast(STRINGS.AETE_EVENTS.LADDERHATINGBEES.TOAST, STRINGS.AETE_EVENTS.LADDERHATINGBEES.LADDERLESS);
				return;
			}

			ladders.Shuffle();
			var laddersToBreak = Random.Range(5, 20);
			laddersToBreak = Mathf.Min(laddersToBreak, ladders.Count);

			var selection = ladders
				.Take(laddersToBreak)
				.ToList();

			for (var i = selection.Count - 1; i >= 0; i--)
			{
				var ladder = ladders[i];
				ladder.GetComponent<Deconstructable>().ForceDestroyAndGetMaterials();
			}

			ToastManager.InstantiateToast(STRINGS.AETE_EVENTS.LADDERHATINGBEES.TOAST, STRINGS.AETE_EVENTS.LADDERHATINGBEES.DESC);
		}
	}
}
