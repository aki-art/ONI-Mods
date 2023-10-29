using System;
using System.Collections.Generic;
using System.Linq;

namespace Twitchery.Content
{
	public class StoryCard : Resource
	{
		public string kanimFile;
		public string description;
		public delegate void OnStoryFinishedDelegate(MinionIdentity identity, bool wasChoosen);
		public OnStoryFinishedDelegate OnStoryFinishedFn;
		public bool allowDuplicateTraits;
		public bool isEnd;
		public string setSpecies;

		public HashSet<string> tags;
		public List<Func<List<StoryCard>, int, bool>> conditions;
		public HashSet<string> grantTraits;

		public StoryCard(string id, string kanimFile) : base(id, Strings.Get($"STRINGS.AETE.STORYCARDS.{id.ToUpperInvariant()}.NAME"))
		{
			tags = new HashSet<string>();
			conditions = new();
			allowDuplicateTraits = false;
			description = Strings.Get($"STRINGS.AETE.STORYCARDS.{id.ToUpperInvariant()}.DESCRIPTION");

			tags.Add(id);
			this.kanimFile = kanimFile;
		}

		public StoryCard FixedSpecies(string species)
		{
			setSpecies = species;
			return this;
		}

		public bool MeetsAllConditions(List<StoryCard> playerChosenCards, int progression)
		{
			if (conditions == null)
				return true;

			if (playerChosenCards.Any(card => card.Id == Id))
				return false;

			foreach (var condition in conditions)
			{
				if (!condition(playerChosenCards, progression))
					return false;
			}

			return true;
		}

		public StoryCard EndsStory()
		{
			isEnd = true;
			return this;
		}

		public StoryCard MustBeNthProgression(int progression)
		{
			bool conditionFn(List<StoryCard> _, int p) => p == progression;
			conditions.Add(conditionFn);
			return this;
		}

		public StoryCard RequiresTag(string tag)
		{
			bool conditionFn(List<StoryCard> cards, int _) => cards.Any(card => card.tags.Contains(tag));
			conditions.Add(conditionFn);
			return this;
		}

		public StoryCard PreviousTagMustBe(string tag)
		{
			bool conditionFn(List<StoryCard> cards, int _) => cards.Count > 0 && cards.Last().tags.Contains(tag);
			conditions.Add(conditionFn);

			return this;
		}

		public StoryCard Tags(params string[] tags)
		{
			foreach (var tag in tags)
				this.tags.Add(tag);

			return this;
		}

		public StoryCard AllowDuplicateTraits()
		{
			allowDuplicateTraits = true;
			return this;
		}

		public StoryCard OnFinished(OnStoryFinishedDelegate fn)
		{
			OnStoryFinishedFn += fn;
			return this;
		}

		public StoryCard GrantTrait(string trait)
		{
			grantTraits ??= new HashSet<string>();
			grantTraits.Add(trait);

			return this;
		}

		public StoryCard GiveItem(string prefabId, int count = 1)
		{
			// spawn items
			return this;
		}

		private void AddTrait(MinionIdentity minion, bool isChosen, string trait)
		{
			// add trait here
		}
	}
}
