using UnityEngine;

namespace Twitchery.Content.Scripts.UI
{
	public class Card : KMonoBehaviour
	{
		[SerializeField] public LocText label;
		[SerializeField] public KBatchedAnimController bg;
		[SerializeField] public KBatchedAnimController mg;
		[SerializeField] public KBatchedAnimController fg;

		public void SetCard(StoryCard card)
		{
			label.SetText(card.Name);
		}
	}
}
