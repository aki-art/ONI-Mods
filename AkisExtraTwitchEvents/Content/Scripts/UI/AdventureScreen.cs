using FUtility;
using FUtility.FUI;
using HarmonyLib;
using System.Collections.Generic;

namespace Twitchery.Content.Scripts.UI
{
	public class AdventureScreen : FScreen
	{
		public Card cardPrefab;
		public Adventure currentAdventure;
		public List<StoryCard> deck;
		public int progression;
		public int cardsPerLevel = 3;
		private List<Card> cards;
		public StoredMinionIdentity target;
		public string species;

		public void StartAdventure(string adventureId, StoredMinionIdentity identity)
		{
			target = identity;
			deck = new();
			cards = new List<Card>();
			progression = 0;
			currentAdventure = TDb.adventures.Get(adventureId);
			DrawCards(cardsPerLevel);
		}

		public void SetSpecies(string species)
		{
		}

		public void DrawCards(int amount)
		{
			var count = 0;
			for (int i = 0; i < amount; i++)
			{
				var card = currentAdventure.Draw(deck, progression);

				if (card == null)
					return;

				AddCard(card);
				count++;
			}

			if (count == 0)
				FinishStory();
		}

		private void AddCard(StoryCard storyCard)
		{
			var card = Instantiate(cardPrefab);
			card.SetCard(storyCard);
			card.GetComponent<FButton>().OnClick += () => OnSelectCard(storyCard);
			cards.Add(card);
		}

		public void OnSelectCard(StoryCard storyCard)
		{
			deck.Add(storyCard);
			progression++;

			if (storyCard.isEnd)
				FinishStory();
			else
				RefreshCards();
		}

		private void RefreshCards()
		{
			foreach (var card in cards)
				Destroy(card.gameObject);

			cards.Clear();

			DrawCards(cardsPerLevel);
		}

		private void FinishStory()
		{
			Log.Debug("finished story");
			Log.Debug("selected cards:");
			deck.Do(card => Log.Debug($"- {card.Id}"));

			Deactivate();
			// apply stats
		}
	}
}
