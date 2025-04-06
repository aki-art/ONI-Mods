using Klei.AI;
using UnityEngine;

namespace DecorPackB.Content.Scripts
{
	public class InspireAll : KMonoBehaviour
	{
		[SerializeField] public string effectId;

		public override void OnSpawn()
		{
			base.OnSpawn();
			Subscribe((int)GameHashes.NewDay, _ => ApplyEffectToDupes(true));
			ApplyEffectToDupes(true);
		}

		private void ApplyEffectToDupes(bool emote)
		{
			var worldId = this.GetMyWorldId();

			foreach (var identity in Components.LiveMinionIdentities.GetWorldItems(worldId, true))
			{
				if (identity is null)
					continue;

				if (emote)
					identity.GetComponent<Facing>().Face(transform.position.x);

				identity.TryGetComponent(out Effects effects);
				{
					if (effects.HasEffect(effectId))
						effects.Remove(effectId);

					effects.Add(effectId, true);
				}
			}
		}
	}
}
