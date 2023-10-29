using FUtility;
using ProcGen;
using System.Collections.Generic;

namespace Twitchery.Content
{
	public class Adventure(string id) : Resource(id)
	{
		public List<WeightedCard> deck = new();

		public Adventure AddCard(string cardId, float weight = 1f)
		{
			if (TDb.storyCards.TryGet(cardId) != null)
			{
				deck.Add(new WeightedCard()
				{
					cardId = cardId,
					weight = weight
				});
			}

			return this;
		}

		public StoryCard Draw(List<StoryCard> hand, int progression)
		{
			var potentials = ListPool<WeightedCard, Adventure>.Allocate();

			foreach (var cardEntry in deck)
			{
				var card = TDb.storyCards.TryGet(cardEntry.cardId);

				if (card != null && card.MeetsAllConditions(hand, progression))
					potentials.Add(cardEntry);
			}

			var result = potentials.GetWeightedRandom();
			potentials.Recycle();

			return TDb.storyCards.Get(result.cardId);
		}

		public class WeightedCard : IWeighted
		{
			public string cardId;

			public float weight { get; set; }
		}
	}
}
